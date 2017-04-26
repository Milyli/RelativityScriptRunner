namespace Milyli.ScriptRunner.Data.Models
{
    using Milyli.Framework.Repositories.Interfaces;

    public class JobScheduleWorkspace : IModel<int>
    {
        public int Id { get; set; }

        public int JobScheduleId { get; set; }

        // GUID of the Workspace Artifact
        public int WorkspaceId { get; set; }
    }
}
