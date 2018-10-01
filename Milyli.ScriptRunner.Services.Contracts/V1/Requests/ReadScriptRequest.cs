namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	/// <summary>
	/// Request definition to read a single script.
	/// </summary>
	public class ReadScriptRequest
	{
		/// <summary>
		/// Workspace the script belongs to.
		/// </summary>
		public int CaseId { get; set; }

		/// <summary>
		/// Script identifier.
		/// </summary>
		public int ScriptId { get; set; }
	}
}
