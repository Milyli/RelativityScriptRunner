namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;
	using Models;

	/// <summary>
	/// Base response to Script Run requests
	/// </summary>
	public class ScriptRunResponse
	{
		/// <summary>
		/// Returned Script Run.
		/// </summary>
		public ScriptRun ScriptRun { get; set; }

		/// <summary>
		/// Associated script run inputs.
		/// </summary>
		public List<Input> ScriptInputs { get; set; } 
	}
}
