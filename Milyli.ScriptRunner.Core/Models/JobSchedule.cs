namespace Milyli.ScriptRunner.Core.Models
{
    using System;
    using LinqToDB.Mapping;
    using Milyli.Framework.Repositories.Interfaces;
    using Milyli.ScriptRunner.Core.Tools;

    public enum JobStatus
    {
        Idle = 0,
        Waiting = 1,
        Running = 2
    }

    [Table(Name = "JobSchedule")]
    public class JobSchedule : IModel<int>
    {
        public const int NO_RUNTIME_LIMIT = -1;

        [PrimaryKey]
        [Identity]
        [Column(Name = "JobScheduleId")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID that points to the relativity script to execute
        /// </summary>
        [Column(Name = "RelativityScriptId")]
        public int RelativityScriptId { get; set; }

        [Column(Name = "WorkspaceId")]
        public int WorkspaceId { get; set; }

        [Column(Name = "LastExecutionTime")]
        public DateTime? LastExecutionTime { get; set; }

        [Column(Name = "NextExecutionTime")]
        public DateTime? NextExecutionTime { get; set; }

        [Column(Name = "JobStatus")]
        public int JobStatus { get; set; } = (int)Models.JobStatus.Idle;

        [Column(Name = "JobEnabled")]
        public bool JobEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum runtime (in seconds) for the job
        /// </summary>
        [Column(Name = "MaximumRuntime")]
        public int MaximumRuntime { get; set; } = NO_RUNTIME_LIMIT;

        /// <summary>
        /// Gets or sets the bitmask that represents the schedule.  Only the first 7 bits (0x01 through 0x7F) are used, the LSB represents Sunday, the 7th bit represents Saturday
        /// </summary>
        [Column(Name = "ExecutionSchedule")]
        public int ExecutionSchedule { get; set; }

        /// <summary>
        /// Gets or sets the execution Time-of-day (in seconds).
        /// </summary>
        [Column(Name = "ExecutionTime")]
        public int ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets CurrentJobHistory. When running in the current thread, this field will get set to the current job history
        /// </summary>
        public JobHistory CurrentJobHistory { get; set; }

        public static int TimeSeconds(DateTime runtime)
        {
            return runtime.Second + ((runtime.Minute + (runtime.Hour * 60)) * 60);
        }

        /// <summary>
        /// Takes a Time-of-Day and returns a corresponding TimeSpan
        /// </summary>
        /// <param name="timeSeconds">the Time-of-Day in seconds</param>
        /// <returns>a TimeSpan representing the duration since midnight</returns>
        public static TimeSpan TimeOfDay(int timeSeconds)
        {
            var hours = timeSeconds / 3600;
            var minutes = (timeSeconds / 60) % 60;
            var seconds = timeSeconds % 60;
            return new TimeSpan(hours, minutes, seconds);
        }

        /// <summary>
        /// Gets the NextExecution Date for this job
        /// </summary>
        /// <param name="runtime">The timestamp of the current execution</param>
        /// <returns>A DateTime for the next execution date, or null if the schedule is invalid</returns>
        public DateTime? GetNextExecution(DateTime runtime)
        {
            var runtimeMask = BitmaskHelper.RotateRight(this.ExecutionSchedule, (int)runtime.DayOfWeek + 1, 7);
            var numberOfDays = 1;
            while (((runtimeMask & 1) == 0) && numberOfDays < 7)
            {
                numberOfDays++;
                runtimeMask = runtimeMask >> 1;
            }

            return ((runtimeMask & 1) == 0) ? default(DateTime?) : runtime.Date.AddDays(numberOfDays).Add(TimeOfDay(this.ExecutionTime));
        }

        internal void UpdateExecutionTimes()
        {
            this.LastExecutionTime = this.CurrentJobHistory.StartTime;
            this.NextExecutionTime = this.GetNextExecution(this.CurrentJobHistory.StartTime);
        }
    }
}
