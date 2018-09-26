namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;

	/// <summary>
	/// Response to read case scripts requests
	/// </summary>
	public class ReadCaseScriptResponse
	{
		public int CaseId { get; set; }

		public string CaseName { get; set; }

		public List<Script> CaseScripts { get; set; }
	}
}
