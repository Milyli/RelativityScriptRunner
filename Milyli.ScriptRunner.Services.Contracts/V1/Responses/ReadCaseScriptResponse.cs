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
		/// Collection of scripts
		/// </summary>
		public List<Script> CaseScripts { get; set; }
	}
}
