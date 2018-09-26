namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	using System.Collections.Generic;
	using Models;

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
