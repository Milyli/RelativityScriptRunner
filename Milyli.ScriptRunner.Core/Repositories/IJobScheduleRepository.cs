namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using Milyli.Framework.Repositories.Interfaces;
    using Milyli.ScriptRunner.Core.Models;

    public interface IJobScheduleRepository : IReadWriteRepository<JobSchedule, int>
    {
        /// <summary>
        /// Gets the list of jobs ready for execution at the current time
        /// </summary>
        /// <param name="runtime">the effective execution time</param>
        /// <returns>a list of JobSchedules for jobs ready to run</returns>
        List<JobSchedule> GetJobSchedules(DateTime runtime);

        /// <summary>
        /// Returns the list of jobs scheduled in the current workspace
        /// </summary>
        /// <param name="relativityWorkspace">the relativity workspace</param>
        /// <returns>the list of currently scheduled jobs in that workspace</returns>
        List<JobSchedule> GetJobSchedules(RelativityWorkspace relativityWorkspace);

        /// <summary>
        /// Starts a job, ensuring that no other parties have started job execution
        /// </summary>
        /// <param name="jobSchedule">the job to execute</param>
        /// <returns>AlreadyStarted for a job that's currently executing, Started for a job that can be invoked in the current thread of execution, or Invalid</returns>
        JobActivationStatus StartJob(JobSchedule jobSchedule);

        /// <summary>
        /// Returns a list of jobschedules for the given <see cref="RelativityScript" />
        /// </summary>
        /// <param name="relativityScript">the script to return schedules for</param>
        /// <returns>a list of all schedules for a script</returns>
        List<JobSchedule> GetJobSchedules(RelativityScript relativityScript);

        /// <summary>
        /// Returns the most recent JobHistory record for the current job
        /// </summary>
        /// <param name="jobSchedule">the job to get the history for</param>
        /// <returns>the most recent JobHistory entry</returns>
        JobHistory GetLastJobExecution(JobSchedule jobSchedule);

        /// <summary>
        /// Returns the entire history for a particular job
        /// </summary>
        /// <param name="jobSchedule">the job to get the history for</param>
        /// <param name="resultCount">outputs the size of the result set</param>
        /// <param name="currentPage">the page currently in view</param>
        /// <param name="pageSize">the number of entries we want to view</param>
        /// <returns>the full set of job history records</returns>
        List<JobHistory> GetJobHistory(JobSchedule jobSchedule, out int resultCount, int currentPage = 0, int pageSize = 10);

        /// <summary>
        /// Get the list of script input values used for deferred execution of the given job
        /// </summary>
        /// <param name="job">The job to get inputs for</param>
        /// <returns>a list of JobScriptInput records, which contain the InputName and Value</returns>
        List<JobScriptInput> GetJobInputs(JobSchedule job);

        /// <summary>
        /// Deletes the set of job schedules for the given Script Id
        /// </summary>
        /// <param name="relativityScriptId">the ArtifactId of the relativityScript</param>
        /// <returns>the number of records deleted</returns>
        int DeleteAllJobs(int relativityScriptId);

        /// <summary>
        /// Creates the input settings used for deferred execution of the given relativity script
        /// </summary>
        /// <param name="jobSchedule">the job schedule to create/update</param>
        /// <param name="jobScriptInputs">The list of inputs to create</param>
        /// <returns>the id of the jobSchedule, which may be new if the schedule is newly inserted</returns>
        int SaveJobSchedule(JobSchedule jobSchedule, List<JobScriptInput> jobScriptInputs);

        /// <summary>
        /// Marks a job as done and updates the JobHistory record
        /// </summary>
        /// <param name="jobSchedule">The job to complete.  The jobSchedule object is expected to have a JobHistory record in the CurrentJobHistory property</param>
        void FinishJob(JobSchedule jobSchedule);

        /// <summary>
        /// Marks a job as "Waiting for activation"
        /// </summary>
        /// <param name="jobSchedule">the job schedule to mark</param>
        JobActivationStatus ActivateJob(JobSchedule jobSchedule);
    }
}
