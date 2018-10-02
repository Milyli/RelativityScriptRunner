namespace Milyli.ScriptRunner.Services.Contracts.V1
{
	using System;
	using Enumerations;

	/// <summary>
	/// Defines a Script Run job.
	/// </summary>
	public class ScriptRun
	{
		/// <summary>
		/// Script Run identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Identifier of the associated script.
		/// </summary>
		public int RelativityScriptId { get; set; }

		/// <summary>
		/// Identifier of the script's source workspace.
		/// </summary>
		public int WorkspaceId { get; set; }

		/// <summary>
		/// Name of the script run job.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets whether the job is enabled.
		/// Disabled Script Runs will not execute as scheduled.
		/// </summary>
		public bool JobEnabled { get; set; }

		/// <summary>
		/// Gets or sets the days of the week the execution is scheduled.
		/// </summary>
		public WeeklySchedule ExecutionSchedule { get; set; }

		/// <summary>
		/// Gets or sets the scheduled execution Time-of-day (in seconds).
		/// </summary>
		public int ExecutionTime { get; set; }

		/// <summary>
		/// Last time the script run executed, in UTC.
		/// Null for un-executed Script Runs.
		/// Read only field.
		/// </summary>
		public DateTime? LastExecutionTimeUtc { get; set; }

		/// <summary>
		/// Next time the script is scheduled to execute, in UTC.
		/// Null for un-scheduled Script Runs.
		/// Read only field.
		/// </summary>
		public DateTime? NextExecutionTimeUtc { get; set; }

		/// <summary>
		/// Status of the script run.
		/// Read only field.
		/// </summary>
		public RunStatus Status { get; set; }
	}
}
