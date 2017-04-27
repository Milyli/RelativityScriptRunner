using System;
using Milyli.ScriptRunner.Data.Models;
using kCura.Relativity.Client;
using Milyli.ScriptRunner.Data.Repositories;
using Milyli.ScriptRunner.Data.Tools;

namespace Milyli.ScriptRunner.Data.Services
{
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
            throw new NotImplementedException();
        }

        private void ExecuteJobInWorkspace(IRSAPIClient client, JobSchedule job)
        {
            
        }

        public void ExecuteScriptJob(JobSchedule job)
        {
            var activationStatus = this.jobScheduleRepository.StartJob(job);
            if (activationStatus == JobActivationStatus.Started)
            {
                try
                {
                    RelativityHelper.InWorkspace(
                        (client, workspace) =>
                    {
                        this.ExecuteJobInWorkspace(client, job);
                    },
                        this.relativityClient, new RelativityWorkspace()
                        {
                            WorkspaceId = job.WorkspaceId;
                        })   
                }
                finally
                {
                    this.jobScheduleRepository.FinishJob(job);
                }
            }                
        }
    }
}
