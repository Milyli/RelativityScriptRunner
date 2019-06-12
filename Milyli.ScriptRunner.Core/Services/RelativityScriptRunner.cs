namespace Milyli.ScriptRunner.Core.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using global::Relativity.API;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Models;
	using Milyli.ScriptRunner.Core.Repositories;
	using Milyli.ScriptRunner.Core.Repositories.Interfaces;
	using Milyli.ScriptRunner.Core.Tools;
	using Relativity.Client;

	public class RelativityScriptRunner : IRelativityScriptRunner
	{
		private readonly IJobScheduleRepository jobScheduleRepository;
		private readonly IRelativityClientFactory relativityClient;
		private readonly IRelativityScriptRepository relativityScriptRepository;
		private readonly IRelativityScriptProcessor relativityScriptProcessor;
		private readonly ISearchTableManager searchTableManager;
		private readonly IHelper helper;

		public RelativityScriptRunner(
			IJobScheduleRepository jobScheduleRepository,
			IRelativityClientFactory relativityClient,
			IRelativityScriptRepository relativityScriptRepository,
			IRelativityScriptProcessor relativityScriptProcessor,
			ISearchTableManager searchTableManager,
			IHelper helper)
		{
			this.jobScheduleRepository = jobScheduleRepository;
			this.relativityClient = relativityClient;
			this.relativityScriptRepository = relativityScriptRepository;
			this.relativityScriptProcessor = relativityScriptProcessor;
			this.searchTableManager = searchTableManager;
			this.helper = helper;
		}

		private static NLog.Logger Logger => NLog.LogManager.GetLogger("Default");

		/// <inheritdoc />
		public void ExecuteAllJobs(DateTime executionTime)
		{
			var schedules = this.jobScheduleRepository.GetJobSchedules(executionTime);
			Logger.Trace($"found {schedules.Count} jobs to execute");
			foreach (var schedule in schedules)
			{
				if (schedule.DirectSql)
				{
					this.ExecuteDirectSqlJob(schedule);
				}
				else
				{
					this.ExecuteScriptJob(schedule);
				}
			}
		}

		/// <inheritdoc />
		public void ExecuteDirectSqlJob(JobSchedule job)
		{
			var workspace = new RelativityWorkspace { WorkspaceId = job.WorkspaceId };
			var searchIds = new List<int>();
			var activationStatus = this.jobScheduleRepository.StartJob(job);

			if (activationStatus == JobActivationStatus.Started)
			{
				// Not a fan of the double try-catch, but the outer clause simply replicates the existing
				// pattern of job execution in ExecuteScriptJob. If agent execution is refactored in the future the outer clause should be removed.
				try
				{
					Logger.Info($"Executing script {job.Name} via direct SQL.");
					var inputs = this.jobScheduleRepository.GetJobInputs(job);
					var script = RelativityHelper.InWorkspace((client, _) => client.Repositories.RelativityScript.ReadSingle(job.RelativityScriptId), workspace, this.relativityClient.GetRelativityClient());
					var originalInputs = RelativityHelper.InWorkspace((client, _) => client.Repositories.RelativityScript.GetRelativityScriptInputs(script), workspace, this.relativityClient.GetRelativityClient());
					var scriptSql = script.Body.AsXmlDocument.GetElementsByTagName("action").Item(0).InnerText;
					scriptSql = this.relativityScriptProcessor.SubstituteGlobalVariables(workspace.WorkspaceId, scriptSql);
					scriptSql = this.relativityScriptProcessor.SubstituteScriptInputs(inputs, originalInputs, scriptSql);
					searchIds = this.relativityScriptProcessor.GetSavedSearchIds(inputs, originalInputs).ToList();

					var searchTables = this.searchTableManager.CreateTablesAsync(workspace.WorkspaceId, searchIds, job.Id, job.MaximumRuntime).GetAwaiter().GetResult();
					scriptSql = this.relativityScriptProcessor.SubstituteSavedSearchTables(inputs, originalInputs, searchTables, scriptSql);

					try
					{
						this.helper.GetDBContext(workspace.WorkspaceId).ExecuteNonQuerySQLStatement(scriptSql, job.MaximumRuntime);
					}
					finally
					{
						this.searchTableManager.DeleteTables(workspace.WorkspaceId, searchTables.Values);
					}
					Logger.Info($"Direct SQL Execution of script {job.Name} complete.");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, $"Execution of job {job.Id} failed");
					job.CurrentJobHistory.ResultText = "Exception: " + ex.ToString();
					job.CurrentJobHistory.HasError = true;
				}
				finally
				{
					this.jobScheduleRepository.FinishJob(job);
				}
			}
		}

		/// <inheritdoc />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "TODO: this is probably a good idea")]
		public void ExecuteScriptJob(JobSchedule job)
		{
			var activationStatus = this.jobScheduleRepository.StartJob(job);
			var workspace = new RelativityWorkspace()
			{
				WorkspaceId = job.WorkspaceId
			};

			if (activationStatus == JobActivationStatus.Started)
			{
				try
				{
					Logger.Info($"Executing {job.Name}.");
					RelativityHelper.InWorkspace(
							(client, ws) =>
					{
						this.ExecuteJobInWorkspace(client, job, ws);
					},
							workspace,
							this.relativityClient.GetRelativityClient());
					Logger.Info($"Execution of {job.Name} complete.");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, $"Execution of job {job.Id} - {job.Name} failed");
					job.CurrentJobHistory.ResultText = "Exception: " + ex.ToString();
					job.CurrentJobHistory.HasError = true;
				}
				finally
				{
					this.jobScheduleRepository.FinishJob(job);
				}
			}
		}

		/// <summary>
		/// Runs the relativity script job
		/// </summary>
		/// <param name="client">the relativity client</param>
		/// <param name="job">the scheduled job</param>
		/// <param name="workspace">the workspace we exect to execute the script in</param>
		private void ExecuteJobInWorkspace(IRSAPIClient client, JobSchedule job, RelativityWorkspace workspace)
		{
			var inputs = this.jobScheduleRepository.GetJobInputs(job);
			Logger.Trace($"found ${inputs.Count} inputs for job ${job.Id}");

			var maxAttempts = 3;
			var attempts = 0;
			ExecuteResult executeResult = null;
			do
			{
				attempts++;
				try
				{
					executeResult = this.relativityScriptRepository.ExecuteRelativityScript(job.RelativityScriptId, inputs, workspace);
				}
				catch (Exception ex)
				{
					if (attempts < maxAttempts)
					{
						Task.Delay(1000 * attempts);
						Logger.Info($"Attempt ${attempts} failed for job ${job.Id} with exception ${ex.Message}");
						continue;
					}
					else
					{
						throw ex;
					}
				}
			} while (attempts < maxAttempts);

			if (executeResult != null)
			{
				job.CurrentJobHistory.HasError = !executeResult.Success;
				job.CurrentJobHistory.ResultText = executeResult.Message;
				if (job.CurrentJobHistory.HasError)
				{
					Logger.Info($"Job {job.Id} failed with result {executeResult.Message}");
				}
			}
			else
			{
				job.CurrentJobHistory.HasError = true;
				job.CurrentJobHistory.ResultText = "scriptResult Not initialized";
			}
		}
	}
}
