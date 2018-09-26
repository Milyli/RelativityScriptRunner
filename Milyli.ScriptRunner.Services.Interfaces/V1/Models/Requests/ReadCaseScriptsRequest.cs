namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	/// <summary>
	/// Request definition to read all scripts in a workspace.
	/// </summary>
	public class ReadCaseScriptsRequest
	{
		/// <summary>
		/// Workspace identifier.
		/// </summary>
		public int CaseId { get; set; }
	}
}
