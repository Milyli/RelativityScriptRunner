namespace Milyli.ScriptRunner.Core.Models
{
    public class RelativityWorkspace
    {
        public const int AdminWorkspaceId = -1;

        public const string AdminWorkspaceName = "Admin Workspace";

        public RelativityWorkspace()
        {
        }

        public static RelativityWorkspace AdminWorkspace
        {
            get
            {
                return new RelativityWorkspace()
                {
                    Name = AdminWorkspaceName,
                    WorkspaceId = AdminWorkspaceId
                };
            }
        }

        public int WorkspaceId { get; set; }

        public string Name { get; set; }
    }
}
