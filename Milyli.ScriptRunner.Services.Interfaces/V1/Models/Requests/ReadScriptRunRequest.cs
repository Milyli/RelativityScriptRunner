namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
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
