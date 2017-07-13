
using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Models
{
    public class JobScheduleWorkspace : IModel<int>
    {
        public int Id { get; set; }

        public int JobScheduleId { get; set; }

        // GUID of the Workspace Artifact
        public int WorkspaceId { get; set; }
    }
}
