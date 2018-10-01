namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	/// <summary>
	/// Request definition to read a script run.
	/// </summary>
	public class ReadScriptRunRequest
	{
		/// <summary>
		/// Script Run identifier.
		/// </summary>
		public int ScriptRunId { get; set; }
	}
}
