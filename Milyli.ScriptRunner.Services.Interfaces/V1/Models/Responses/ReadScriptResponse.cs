namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Responses
{
	using System.Collections.Generic;
	using Models;

	public class ReadScriptResponse
	{
		public const int ScriptArtifactTypeId = 28;

		public int RelativityScriptId { get; set; }

		public int WorkspaceId { get; set; }

		public string WorkspaceName { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int ScriptTimeout { get; set; }

		public List<ScriptRun> ScriptRuns { get; set; }
	}
}
