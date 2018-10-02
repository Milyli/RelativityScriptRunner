namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	using System.Collections.Generic;
	using V1;

	/// <summary>
	/// Request definition to create or update a script run.
	/// </summary>
	public class ScriptRunRequest
	{
		/// <summary>
		/// User-defined Script Run.
		/// </summary>
		public ScriptRun ScriptRun { get; set; }

		/// <summary>
		/// User-defined Script Run inputs.
		/// </summary>
		public List<Input> ScriptInputs { get; set; }
	}
}
