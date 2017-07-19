namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IRelativityWorkspaceRepository
    {
        IEnumerable<RelativityWorkspace> AllWorkspaces { get; }

        RelativityWorkspace Read(int workspaceId);
    }
}
