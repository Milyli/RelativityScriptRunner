namespace Milyli.ScriptRunner.Core.Services
{
    using System;
    using Milyli.ScriptRunner.Core.Models;

    public interface IRelativityScriptRunner
    {
        void ExecuteScriptJob(JobSchedule job);

        void ExecuteAllJobs(DateTime exectionTime);
    }
}
