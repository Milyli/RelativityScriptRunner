namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;
	using Models;

	/// <summary>
	/// Base response to Script Run requests
	/// </summary>
	public class ScriptRunResponse
	{
		public ScriptRun ScriptRun { get; set; }

		public List<Input> ScriptInputs { get; set; } 
	}
}
