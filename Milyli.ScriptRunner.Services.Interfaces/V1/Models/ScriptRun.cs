namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models
{
	using System;

	public class ScriptRun
	{
		public int Id { get; set; }

		public int RelativityScriptId { get; set; }

		public int WorkspaceId { get; set; }

		public string Name { get; set; }

		public DateTimeOffset? LastExecutionTimeUTC { get; set; }

		public DateTimeOffset? NextExecutionTimeUTC { get; set; }

		public int JobStatus { get; set; }

		/// <summary>
		/// Gets or sets whether the job is enabled.
		/// Disabled Script Runs will not execute as scheduled.
		/// </summary>
		public bool JobEnabled { get; set; } = true;

		/// <summary>
		/// Gets or sets the days of the week the execution is scheduled.
		/// </summary>
		public WeeklySchedule ExecutionSchedule { get; set; }

		/// <summary>
		/// Gets or sets the execution Time-of-day (in seconds).
		/// </summary>
		public int ExecutionTime { get; set; }
	}
}
