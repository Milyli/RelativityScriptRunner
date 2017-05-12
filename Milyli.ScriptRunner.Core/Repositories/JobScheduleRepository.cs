namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using LinqToDB;
    using LinqToDB.Data;
    using Milyli.Framework.Repositories;
    using Milyli.ScriptRunner.Core.DataContexts;
    using Milyli.ScriptRunner.Core.Models;

    public enum JobActivationStatus
    {
        InvalidJob = -1,
        Idle = 0,
        Started = 1,
        AlreadyRunning = 2,
        Waiting = 3
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose method not exposed")]
    public class JobScheduleRepository : BaseReadWriteRepository<InstanceDataContext, JobSchedule, int>, IJobScheduleRepository
    {
        // One day, in seconds
        private const int DEFAULT_MAX_OFFSET = 86400;

        public JobScheduleRepository(InstanceDataContext dataContext)
            : base(dataContext)
        {
        }

        private static NLog.Logger Logger
        {
            get
            {
                return NLog.LogManager.GetLogger("Default");
            }
        }

        // Returns a list of Jobs to run, filtered by NextExecution constrained to NextExecutionTimes between maxOffset seconds before runtime and runtime
        public List<JobSchedule> GetJobSchedules(DateTime runtime, int maxOffset)
        {
            var end = runtime;
            var offset = maxOffset < int.MaxValue ? maxOffset : int.MaxValue - 1;
            var start = runtime.AddSeconds(-1 * offset);
            var result = this.DataContext.JobSchedule
                .Where(s =>
                    ((start <= s.NextExecutionTime && s.NextExecutionTime <= end)
                    && s.JobEnabled && s.JobStatus == (int)JobStatus.Idle)
                    || s.JobStatus == (int)JobStatus.Waiting)
                .ToList();
            return result;
        }

        public JobActivationStatus ActivateJob(JobSchedule jobSchedule)
        {
            using (var connection = (DataConnection)this.DataContext)
            using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
            {
                var activationStatus = LockJobSchedule(jobSchedule, transaction);
                if (activationStatus == JobActivationStatus.Idle)
                {
                    Logger.Trace($"Marking {jobSchedule.Id} 'Waiting For Activation'");
                    jobSchedule.JobStatus = (int)JobStatus.Waiting;
                    this.Update(jobSchedule);
                    transaction.Commit();
                    return JobActivationStatus.Waiting;
                }
                else
                {
                    transaction.Rollback();
                    return JobActivationStatus.AlreadyRunning;
                }
            }
        }

        public List<JobSchedule> GetJobSchedules(DateTime runtime)
        {
            return this.GetJobSchedules(runtime, DEFAULT_MAX_OFFSET);
        }

        public List<JobSchedule> GetJobSchedules(RelativityWorkspace relativityWorkspace)
        {
            return this.DataContext.JobSchedule
                .Where(s => s.WorkspaceId == relativityWorkspace.WorkspaceId)
                .ToList();
        }

        public List<JobSchedule> GetJobSchedules(RelativityScript relativityScript)
        {
            return this.DataContext.JobSchedule
                .Where(s => s.WorkspaceId == relativityScript.WorkspaceId && s.RelativityScriptId == relativityScript.RelativityScriptId)
                .ToList();
        }

        public JobActivationStatus StartJob(JobSchedule jobSchedule)
        {
            Logger.Trace($"Trying to start schedule {jobSchedule.Id}");
            using (var connection = (DataConnection)this.DataContext)
            using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
            {
                try
                {
                    var activationStatus = LockJobSchedule(jobSchedule, transaction);
                    if (activationStatus == JobActivationStatus.Idle)
                    {
                        Logger.Trace($"Marking {jobSchedule.Id} running");
                        this.EnterRunningStatus(jobSchedule);
                        transaction.Commit();
                        return JobActivationStatus.Started;
                    }
                    else
                    {
                        transaction.Rollback();
                        return activationStatus;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, $"Staring {jobSchedule.Id} failed");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes all jobs for a given relativity script.  Used in testing
        /// </summary>
        /// <param name="relativityScriptId">the artifactId for the relativity script</param>
        /// <returns>the number of rows removed</returns>
        public int DeleteAllJobs(int relativityScriptId)
        {
            int result;
            using (var transaction = this.DataContext.BeginTransaction())
            {
                try
                {
                    transaction.DataConnection.Execute(
                        @"DELETE jh FROM JobHistory jh 
                            INNER JOIN JobSchedule js ON jh.JobScheduleId = js.JobScheduleId
                            WHERE js.RelativityScriptId = @relativityScriptId",
                        new DataParameter("relativityScriptId", relativityScriptId));
                    transaction.DataConnection.Execute(
                        @"DELETE jsi FROM JobScriptInput jsi 
                            INNER JOIN JobSchedule js ON jsi.JobScheduleId = js.JobScheduleId
                            WHERE js.RelativityScriptId = @relativityScriptId",
                    new DataParameter("relativityScriptId", relativityScriptId));

                    result = this.DataContext.JobSchedule.Where(s => s.RelativityScriptId == relativityScriptId).Delete();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Ends a job run and updates the scheudle and the corresponding history.  This has a side-effect in that the CurrentJobHistory
        /// field is cleared upon commit
        /// </summary>
        /// <param name="jobSchedule">the job that is completing</param>
        public void FinishJob(JobSchedule jobSchedule)
        {
            Logger.Trace($"Finishing {jobSchedule.Id}");
            jobSchedule.CurrentJobHistory.UpdateRuntime();
            jobSchedule.UpdateExecutionTimes();
            jobSchedule.JobStatus = (int)JobStatus.Idle;
            using (var transaction = this.DataContext.BeginTransaction())
            {
                try
                {
                    this.DataContext.Update(jobSchedule.CurrentJobHistory);
                    this.DataContext.Update(jobSchedule);
                    transaction.Commit();
                    jobSchedule.CurrentJobHistory = null;
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, $"Could not mark {jobSchedule.Id} as finished");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// returns the most recent execution history for jobSchedule
        /// </summary>
        /// <param name="jobSchedule">The job to get the history for</param>
        /// <returns>a JobHistory record</returns>
        public JobHistory GetLastJobExecution(JobSchedule jobSchedule)
        {
            return this.DataContext.JobHistory
                .Take(1)
                .Where(h => h.JobScheduleId == jobSchedule.Id)
                .OrderByDescending(h => h.StartTime)
                .FirstOrDefault();
        }

        /// <summary>
        /// returns all history entries for the current job
        /// </summary>
        /// <param name="jobSchedule">the job we are getting history for</param>
        /// <param name="currentPage">the current page</param>
        /// <param name="pageSize">the number of records to retreive at one time</param>
        /// <returns>the history of the job</returns>
        public List<JobHistory> GetJobHistory(JobSchedule jobSchedule, out int resultCount, int currentPage = 0, int pageSize = 10)
        {
            var offset = currentPage * pageSize;
            resultCount = this.DataContext.JobHistory.Where(h => h.JobScheduleId == jobSchedule.Id).Count();
            return this.DataContext.JobHistory
                .Take(offset + pageSize)
                .Skip(offset)
                .Where(h => h.JobScheduleId == jobSchedule.Id)
                .OrderByDescending(h => h.StartTime).ToList();
        }

        public List<JobScriptInput> GetJobInputs(JobSchedule job)
        {
            return this.DataContext.JobScriptInput
                .Where(i => i.JobScheduleId == job.Id)?
                .ToList();
        }

        public int SaveJobSchedule(JobSchedule jobSchedule, List<JobScriptInput> jobScriptInputs)
        {
            using (var transaction = this.DataContext.BeginTransaction())
            {
                if (jobSchedule.Id > 0)
                {
                    LockJobSchedule(jobSchedule, transaction);
                    this.Update(jobSchedule);
                }
                else
                {
                    jobSchedule.Id = this.Create(jobSchedule);
                    jobScriptInputs.ForEach(jsi => jsi.JobScheduleId = jobSchedule.Id);
                }

                this.DataContext.JobScriptInput
                    .Where(jsi => jsi.JobScheduleId == jobSchedule.Id)
                    .Delete();
                this.DataContext.BulkCopy(jobScriptInputs);
                transaction.Commit();
            }

            return jobSchedule.Id;
        }

        // Critical section:
        // this method locks the schedule row for update, in order to coordinate job running.
        private static JobActivationStatus LockJobSchedule(JobSchedule jobSchedule, DataConnectionTransaction transaction)
        {
            Logger.Trace($"Locking {jobSchedule.Id}");
            using (var reader = transaction.DataConnection.ExecuteReader(
                @"SELECT JobStatus
                            FROM JobSchedule WITH(UPDLOCK, ROWLOCK)
                            WHERE JobScheduleId = @jobScheduleId", new DataParameter("jobScheduleId", jobSchedule.Id)))
            {
                if (reader.Reader.Read())
                {
                    return reader.Reader.GetInt32(0) > 1 ? JobActivationStatus.AlreadyRunning : JobActivationStatus.Idle;
                }
                else
                {
                    return JobActivationStatus.InvalidJob;
                }
            }
        }

        private void EnterRunningStatus(JobSchedule jobSchedule)
        {
            jobSchedule.JobStatus = (int)JobStatus.Running;
            this.Update(jobSchedule);
            jobSchedule.CurrentJobHistory = this.AddHistoryEntry(jobSchedule);
        }

        private JobHistory AddHistoryEntry(JobSchedule jobSchedule)
        {
            var jobHistory = new JobHistory()
            {
                JobScheduleId = jobSchedule.Id,
            };
            jobHistory.Id = Convert.ToInt32(this.DataContext.InsertWithIdentity(jobHistory), CultureInfo.InvariantCulture);
            return jobHistory;
        }
    }
}
