namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Models;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityWorkspaceRepository : IRelativityWorkspaceRepository
    {
        private const string NAME_FIELD = "Name";
        private IRSAPIClient relativityClient;
        private Lazy<Dictionary<int, RelativityWorkspace>> relativityWorkspaceCollection;

        public RelativityWorkspaceRepository(IRSAPIClient relativityClient)
        {
            this.relativityClient = relativityClient;
            this.relativityWorkspaceCollection = new Lazy<Dictionary<int, RelativityWorkspace>>(this.GetWorkspaceDictionary, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<RelativityWorkspace> AllWorkspaces
        {
            get
            {
                return this.relativityWorkspaceCollection.Value.Values;
            }
        }

        public RelativityWorkspace Read(int workspaceId)
        {
            return this.relativityWorkspaceCollection.Value.ContainsKey(workspaceId) ?
                    this.relativityWorkspaceCollection.Value[workspaceId] : null;
        }

        private Dictionary<int, RelativityWorkspace> GetWorkspaceDictionary()
        {
            var workspaces = this.relativityClient.Repositories.Workspace.Query(new DTOs.Query<DTOs.Workspace>()
            {
                Fields = new List<DTOs.FieldValue>() { new DTOs.FieldValue(NAME_FIELD) }
            });
            if (workspaces.Success)
            {
                return workspaces.Results.Select(ws => new RelativityWorkspace(ws.Artifact)).ToDictionary(rws => rws.WorkspaceId);
            }

            // TODO throw instead of return?
            return new Dictionary<int, RelativityWorkspace>();
        }
    }
}
