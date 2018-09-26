namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;
	using Models;

	/// <summary>
	/// Response to script read requests
	/// </summary>
	public class ReadScriptResponse
	{
		public Script Script { get; set; }

		public List<ScriptRun> ScriptRuns { get; set; }
	}
}
