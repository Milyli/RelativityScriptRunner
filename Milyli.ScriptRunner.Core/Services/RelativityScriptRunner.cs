using System.Threading.Tasks;
using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Services
{
	using System;
	using System.Linq;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Models;
	using Milyli.ScriptRunner.Core.Repositories;
	using Milyli.ScriptRunner.Core.Tools;
	using Relativity.Client;

	public class RelativityScriptRunner : IRelativityScriptRunner
	{
		private IJobScheduleRepository jobScheduleRepository;
		private IRelativityClientFactory relativityClient;

		public RelativityScriptRunner(IJobScheduleRepository jobScheduleRepository, IRelativityClientFactory relativityClient)
		{
			this.jobScheduleRepository = jobScheduleRepository;
			this.relativityClient = relativityClient;
		}

		private static NLog.Logger Logger
		{
			get
			{
				return NLog.LogManager.GetLogger("Default");
			}
		}

		public void ExecuteAllJobs(DateTime executionTime)
		{
			var schedules = this.jobScheduleRepository.GetJobSchedules(executionTime);
			Logger.Trace($"found {schedules.Count} jobs to execute");
			schedules.ForEach(this.ExecuteScriptJob);
		}

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
					Logger.Trace($"Executing job {job.Id}");
					RelativityHelper.InWorkspace(
							(client, ws) =>
					{
						this.ExecuteJobInWorkspace(client, job);
					},
							workspace,
							this.relativityClient.GetRelativityClient());
				}
				catch (Exception ex)
				{
					Logger.Fatal(ex, $"Execution of job {job.Id} failed");
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
		private void ExecuteJobInWorkspace(IRSAPIClient client, JobSchedule job)
		{
			var inputs = this.jobScheduleRepository.GetJobInputs(job);
			Logger.Trace($"found ${inputs.Count} inputs for job ${job.Id}");
			// new relativity script pass in artifact id

			var maxAttempts = 3;
			var attempts = 0;
			RelativityScriptResult scriptResult = null;
			do
			{
				attempts++;
				try
				{
					var script = new kCura.Relativity.Client.DTOs.RelativityScript(job.RelativityScriptId);
					var scriptInputs = inputs.Select(i => new RelativityScriptInput(i.InputName, i.InputValue)).ToList();
					scriptResult = client.Repositories.RelativityScript.ExecuteRelativityScript(script, scriptInputs);
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

			if (scriptResult != null)
			{
				job.CurrentJobHistory.HasError = !scriptResult.Success;
				job.CurrentJobHistory.ResultText = scriptResult.Message;
				if (job.CurrentJobHistory.HasError)
				{
					Logger.Info($"Job {job.Id} failed with result {scriptResult.Message}");
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
