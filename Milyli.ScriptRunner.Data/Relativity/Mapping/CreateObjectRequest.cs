namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using DTOs = kCura.Relativity.Client.DTOs;

    public class CreateObjectRequest
    {
        private const int EDDS_WORKSPACE = -1;
        private RDOMap map;
        private int workspaceId = EDDS_WORKSPACE;

        public CreateObjectRequest(RDOMap map)
        {
            this.map = map;
        }

        public CreateObjectRequest(RDOMap map, int workspaceId)
            : this(map)
        {
            this.workspaceId = workspaceId;
        }

        private DTOs.ObjectType MakeObjectType()
        {
            var objectType = new DTOs.ObjectType()
            {
                Name = this.map.Name,
                ParentArtifactTypeID = (int)ArtifactType.Workspace,
                SnapshotAuditingEnabledOnDelete = true,
                Pivot = true,
                CopyInstancesOnParentCopy = false,
                CopyInstancesOnWorkspaceCreation = false,
                Sampling = true
            };
            return objectType;
        }
    }
}
