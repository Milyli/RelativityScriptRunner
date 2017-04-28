namespace Milyli.ScriptRunner.Core.Services
{
    using System;
    using System.Linq;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Core.Tools;

    public class RelativityScriptRunner : IRelativityScriptRunner
    {
        private IJobScheduleRepository jobScheduleRepository;
        private IRSAPIClient relativityClient;

        public RelativityScriptRunner(IJobScheduleRepository jobScheduleRepository, IRSAPIClient relativityClient)
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

        public void ExecuteAllJobs(DateTime exectionTime)
        {
            var schedules = this.jobScheduleRepository.GetJobSchedules(exectionTime);
            Logger.Trace($"found {schedules.Count} jobs to execute");
            schedules.ForEach(this.ExecuteScriptJob);
        }

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
                        this.relativityClient);
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, $"Execution of job {job.Id} failed");
                    job.CurrentJobHistory.ResultText = "Exception: " + ex.ToString();
                    job.CurrentJobHistory.Errored = true;
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
            var scriptInputs = inputs.Select(i => new RelativityScriptInput(i.InputName, i.InputValue)).ToList();
            var scriptResult = client.ExecuteRelativityScript(client.APIOptions, job.RelativityScriptId, scriptInputs);

            job.CurrentJobHistory.Errored = !scriptResult.Success;
            job.CurrentJobHistory.ResultText = scriptResult.Message;

            if (job.CurrentJobHistory.Errored)
            {
                Logger.Info($"Job {job.Id} failed with result {scriptResult.Message}");
            }
        }
    }
}
