namespace Milyli.ScriptRunner.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Milyli.ScriptRunner.Data.Models;
    using Milyli.ScriptRunner.Data.Relativity.Client;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityWorkspaceRepository : IRelativityWorkspaceRepository
    {
        private const string NAME_FIELD = "Name";
        private IRelativityClientFactory relativityClientFactory;
        private Lazy<Dictionary<int, RelativityWorkspace>> relativityWorkspaceCollection;

        public RelativityWorkspaceRepository(IRelativityClientFactory relativityClientFactory)
        {
            this.relativityClientFactory = relativityClientFactory;
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
            using (var relativityClient = this.relativityClientFactory.GetRelativityClient())
            {
                var workspaces = relativityClient.Repositories.Workspace.Query(new DTOs.Query<DTOs.Workspace>()
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
}
