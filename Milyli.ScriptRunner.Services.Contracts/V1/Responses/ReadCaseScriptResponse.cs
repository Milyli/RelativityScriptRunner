namespace Milyli.ScriptRunner.Services.Contracts.V1.Responses
{
	using System.Collections.Generic;
	using V1;

	/// <summary>
	/// Response to read case scripts requests
	/// </summary>
	public class ReadCaseScriptResponse
	{
		/// <summary>
		/// Workspace that contains the scripts.
		/// </summary>
		public int CaseId { get; set; }

		/// <summary>
		/// Workspace name.
		/// </summary>
		public string CaseName { get; set; }

		/// <summary>
		/// Collection of scripts
		/// </summary>
		public List<Script> CaseScripts { get; set; }
	}
}
