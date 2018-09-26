namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	using System.Collections.Generic;
	using Models;

	/// <summary>
	/// Request definition create a new script run.
	/// </summary>
	public class CreateScriptRunRequest
	{
		/// <summary>
		/// Script Run to create.
		/// </summary>
		public ScriptRun ScriptRun { get; set; }

		/// <summary>
		/// Script Run inputs.
		/// </summary>
		public List<Input> ScriptInputs { get; set; }
	}
}
