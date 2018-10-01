namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	using System;

	/// <summary>
	/// Request definition to run all scheduled scripts.
	/// </summary>
	public class RunAllRequest
	{
		/// <summary>
		/// Run Time defining which Script Runs to execute.
		/// All unrun Runs scheduled for or prior to the run time should be executed.
		/// </summary>
		public DateTimeOffset RunTimeUTC { get; set; }
	}
}
