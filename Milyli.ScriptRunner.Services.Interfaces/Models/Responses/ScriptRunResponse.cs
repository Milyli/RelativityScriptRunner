namespace Milyli.ScriptRunner.Services.Interfaces.Models.Responses
{
	using System.Collections.Generic;

	public class ScriptRunResponse
	{
		public ScriptRun ScriptRun { get; set; }

		public List<Input> ScriptInputs { get; set; } 
	}
}
