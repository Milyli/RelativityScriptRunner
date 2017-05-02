namespace Milyli.ScriptRunner.Core.Models
{
    public class RelativityWorkspace
    {
        public RelativityWorkspace()
        {
        }

        public static RelativityWorkspace AdminWorkspace
        {
            get
            {
                return new RelativityWorkspace()
                {
                    WorkspaceId = -1
                };
            }
        }

        public int WorkspaceId { get; set; }

        public string Name { get; set; }
    }
}
