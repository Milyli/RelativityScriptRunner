namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	using System.Collections.Generic;
	using V1;

	/// <summary>
	/// Request definition to update a script run.
	/// </summary>
	public class UpdateScriptRunRequest
	{
		/// <summary>
		/// Updated script run.
		/// </summary>
		public ScriptRun ScriptRun { get; set; }

		/// <summary>
		/// Updated script run inputs.
		/// </summary>
		public List<Input> ScriptInputs { get; set; }
	}
}
