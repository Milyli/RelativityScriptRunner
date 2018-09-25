namespace Milyli.ScriptRunner.Services.Interfaces.Models.Responses
{
	public class ReadScriptResponse
	{
		public const int ScriptArtifactTypeId = 28;

		public int RelativityScriptId { get; set; }

		public int WorkspaceId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int ScriptTimeout { get; set; }
	}
}
