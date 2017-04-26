namespace Milyli.ScriptRunner.Data.Repositories
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Data.Models;

    public interface IRelativityWorkspaceRepository
    {
        IEnumerable<RelativityWorkspace> AllWorkspaces { get; }

        RelativityWorkspace Read(int workspaceId);
    }
}
