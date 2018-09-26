namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
{
	using System.Collections.Generic;
	using Models;

	public class CreateScriptRunRequest
	{
		public ScriptRun ScriptRun { get; set; }

		public List<Input> ScriptInputs { get; set; }
	}
}
