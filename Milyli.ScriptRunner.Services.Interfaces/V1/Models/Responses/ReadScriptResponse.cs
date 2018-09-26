namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;
	using Models;

	/// <summary>
	/// Response to script read requests
	/// </summary>
	public class ReadScriptResponse
	{
		/// <summary>
		/// Relativity script
		/// </summary>
		public Script Script { get; set; }

		/// <summary>
		/// Script runs using the returned script.
		/// </summary>
		public List<ScriptRun> ScriptRuns { get; set; }
	}
}
