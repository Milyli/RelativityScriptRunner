namespace Milyli.ScriptRunner.Services.Contracts.V1.Responses
{
	using System.Collections.Generic;
	using V1;

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
