using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Models;
	using Milyli.ScriptRunner.Core.Tools;
	using Relativity.Client;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityWorkspaceRepository : RelativityClientRepository, IRelativityWorkspaceRepository
    {
        private const string NAME_FIELD = "Name";
        private Lazy<Dictionary<int, RelativityWorkspace>> relativityWorkspaceCollection;

        public RelativityWorkspaceRepository(IRelativityClientFactory relativityClientFactory)
            : base(relativityClientFactory)
        {
            this.relativityWorkspaceCollection = new Lazy<Dictionary<int, RelativityWorkspace>>(this.GetWorkspaceDictionary, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<RelativityWorkspace> AllWorkspaces
        {
            get
            {
                yield return RelativityWorkspace.AdminWorkspace;
                foreach (var workspace in this.relativityWorkspaceCollection.Value.Values)
                {
                    yield return workspace;
                }
            }
        }

        public RelativityWorkspace Read(int workspaceId)
        {
            return this.relativityWorkspaceCollection.Value.ContainsKey(workspaceId) ?
                    this.relativityWorkspaceCollection.Value[workspaceId] : null;
        }

        private Dictionary<int, RelativityWorkspace> GetWorkspaceDictionary()
        {
			List<DTOs.Workspace> results;
			try
			{
				results = this.RelativityClient.Repositories.Workspace.Query(new DTOs.Query<DTOs.Workspace>()
				{
					Fields = new List<DTOs.FieldValue>() { new DTOs.FieldValue(NAME_FIELD) }
				}).GetResults();
			}
			catch
			{
				// TODO throw instead of return?
				return new Dictionary<int, RelativityWorkspace>();
			}

			return results
                .Select(ws => new RelativityWorkspace()
                {
                    WorkspaceId = ws.ArtifactID,
                    Name = ws.Name
                })
                .ToDictionary(rws => rws.WorkspaceId);
        }
    }
}
