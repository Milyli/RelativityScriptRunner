namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models
{
	public class Script
	{
		public int RelativityScriptId { get; set; }

		public int WorkspaceId { get; set; }

		public string WorkspaceName { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int ScriptTimeout { get; set; }
	}
}
