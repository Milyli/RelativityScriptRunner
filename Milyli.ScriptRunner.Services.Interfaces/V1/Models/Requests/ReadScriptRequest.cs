namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
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
