namespace Milyli.ScriptRunner.Data.Services
{
    using System;
    using System.Linq;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Data.Models;
    using Milyli.ScriptRunner.Data.Repositories;
    using Milyli.ScriptRunner.Data.Tools;

    public class RelativityScriptRunner : IRelativityScriptRunner
    {
        private IJobScheduleRepository jobScheduleRepository;
        private IRSAPIClient relativityClient;

        public RelativityScriptRunner(IJobScheduleRepository jobScheduleRepository, IRSAPIClient relativityClient)
        {
            this.jobScheduleRepository = jobScheduleRepository;
            this.relativityClient = relativityClient;
        }

        public void ExecuteAllJobs(DateTime exectionTime)
        {
            this.jobScheduleRepository.GetJobSchedules(exectionTime).ForEach(this.ExecuteScriptJob);
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
                    // TODO log the exception
                    job.CurrentJobHistory.ResultText = "Exception: " + ex.ToString();
                    job.CurrentJobHistory.Errored = true;
                    throw;
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
            var scriptInputs = inputs.Select(i => new RelativityScriptInput(i.InputName, i.InputValue)).ToList();
            var scriptResult = client.ExecuteRelativityScript(client.APIOptions, job.RelativityScriptId, scriptInputs);

            job.CurrentJobHistory.Errored = !scriptResult.Success;
            job.CurrentJobHistory.ResultText = scriptResult.Message;
        }
    }
}
