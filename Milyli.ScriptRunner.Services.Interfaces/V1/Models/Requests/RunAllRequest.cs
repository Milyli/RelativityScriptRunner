namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	using System;

	public class RunAllRequest
	{
		/// <summary>
		/// Run Time defining which Script Runs to execute.
		/// All unrun Runs scheduled for or prior to the run time should be executed.
		/// </summary>
		public DateTime RunTime { get; set; }
	}
}
