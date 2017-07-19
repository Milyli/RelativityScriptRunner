// Copyright © 2017 Milyli

using Milyli.ScriptRunner.Core.Relativity.Interfaces;

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity
{
    /// <summary>
    /// Information about the current Relativity context.
    /// </summary>
    public class RelativityContext : IRelativityContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelativityContext"/> class.
        /// </summary>
        /// <param name="workspaceId">The current workspace ID for the context.</param>
        public RelativityContext(int workspaceId)
        {
            this.WorkspaceId = workspaceId;
        }

        /// <inheritdoc/>
        public int WorkspaceId { get; }

        /// <inheritdoc/>
        public IRelativityContext GetWorkspaceContext(int workspaceId)
        {
            return new RelativityContext(workspaceId);
        }

        /// <inheritdoc/>
        public IRelativityContext GetInstanceContext()
        {
            return this.GetWorkspaceContext(-1);
        }
    }
}
