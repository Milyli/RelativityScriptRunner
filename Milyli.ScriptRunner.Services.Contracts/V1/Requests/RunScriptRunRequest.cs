namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	/// <summary>
	/// Request definition to run a single script run.
	/// </summary>
	public class RunScriptRunRequest
	{
		/// <summary>
		/// Script Run to execute.
		/// </summary>
		public int ScriptRunId { get; set; }
	}
}
