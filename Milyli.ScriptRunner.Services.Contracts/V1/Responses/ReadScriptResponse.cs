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
		/// Relativity script.
		/// </summary>
		public Script Script { get; set; }

		/// <summary>
		/// Script's input fields.
		/// </summary>
		public List<Input> ScriptInputs { get; set; }

		/// <summary>
		/// Script runs using the script.
		/// </summary>
		public List<ScriptRun> ScriptRuns { get; set; }
	}
}
