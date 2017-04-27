namespace Milyli.ScriptRunner.Data.Services
{
    using System;
    using Milyli.ScriptRunner.Data.Models;

    public interface IRelativityScriptRunner
    {
        void ExecuteScriptJob(JobSchedule job);

        void ExecuteAllJobs(DateTime exectionTime);
    }
}
