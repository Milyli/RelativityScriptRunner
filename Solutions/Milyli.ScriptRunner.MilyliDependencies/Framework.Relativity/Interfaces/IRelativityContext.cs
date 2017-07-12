// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Interfaces
{
    /// <summary>
    /// Information about the current Relativity context.
    /// </summary>
    public interface IRelativityContext
    {
        /// <summary>
        /// Gets the current workspace ID
        /// </summary>
        int WorkspaceId { get; }

        /// <summary>
        /// Gets a new context, identical to the old one, except in a different workspace.
        /// </summary>
        /// <param name="workspaceId">the workspace ID to switch to.</param>
        /// <returns>The new context</returns>
        IRelativityContext GetWorkspaceContext(int workspaceId);

        /// <summary>
        /// Gets a new context, identical to the old one, except in the EDDS workspace.
        /// </summary>
        /// <returns>The new context</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024: UsePropertiesWhereAppropriate",
            Justification = "Needs to be an instance member for injection")]
        IRelativityContext GetInstanceContext();
    }
}
