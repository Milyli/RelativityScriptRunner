namespace Milyli.ScriptRunner.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using Milyli.Framework.Repositories.Interfaces;
    using Milyli.ScriptRunner.Data.Models;

    public interface IJobScheduleRepository : IReadWriteRepository<JobSchedule, int>
    {
        List<JobSchedule> GetJobSchedules(DateTime runtime);

        JobActivationStatus StartJob(JobSchedule jobSchedule);

        JobHistory GetLastJobExecution(JobSchedule jobSchedule);

        List<JobHistory> GetJobHistory(JobSchedule jobSchedule);

        int Delete(Guid relativityScriptGuid);

        void FinishJob(JobSchedule jobSchedule);
    }
}
