namespace Milyli.ScriptRunner.Core.Repositories
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Core.Models;

    public interface IRelativityWorkspaceRepository
    {
        IEnumerable<RelativityWorkspace> AllWorkspaces { get; }

        RelativityWorkspace Read(int workspaceId);
    }
}
