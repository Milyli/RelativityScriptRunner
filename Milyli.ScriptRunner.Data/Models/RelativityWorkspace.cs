namespace Milyli.ScriptRunner.Data.Models
{
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityWorkspace
    {
        public RelativityWorkspace()
        {
        }

        public RelativityWorkspace(DTOs.Artifact artifact)
        {
            this.WorkspaceId = artifact.ArtifactID;
        }

        public int WorkspaceId { get; set; }

        public string WorkspaceName { get; set; }
    }
}
