namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	using System;
	using System.Collections.Generic;
	using Models;

	public class CreateScriptRunRequest
	{
		public int Id { get; set; }

		public int RelativityScriptId { get; set; }

		public int WorkspaceId { get; set; }

		public string WorkspaceName { get; set; }

		public string Name { get; set; }

		public DateTime? LastExecutionTime { get; set; }

		public DateTime? NextExecutionTime { get; set; }

		public int JobStatus { get; set; }

		public bool JobEnabled { get; set; } = true;

		/// <summary>
		/// Gets or sets the bitmask that represents the schedule.  Only the first 7 bits (0x01 through 0x7F) are used, the LSB represents Sunday, the 7th bit represents Saturday
		/// </summary>
		public int ExecutionSchedule { get; set; }

		/// <summary>
		/// Gets or sets the execution Time-of-day (in seconds).
		/// </summary>
		public int ExecutionTime { get; set; }

		public List<Input> ScriptInputs { get; set; }
	}
}
