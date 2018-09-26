namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;

	public class ReadCaseScriptResponse
	{
		public int CaseId { get; set; }

		public string CaseName { get; set; }

		public List<Script> CaseScripts { get; set; }
	}
}
