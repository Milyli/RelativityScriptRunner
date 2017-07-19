namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    /// <summary>
    /// Factory that creates dependencies that are tied to a specific workspace in Relativity.
    /// </summary>
    public interface IWorkspaceDependencyFactory
    {
        /// <summary>
        /// Gets an instance of the indicated type with all dependencies set
        /// initialized to the appropriate workspace where applicable.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance to construct.</typeparam>
        /// <param name="workspaceId">Relativity workspace id of the target workspace.</param>
        /// <returns>Instance of type <code>TInstance</code> with all dependencies set.</returns>
        TInstance GetInstance<TInstance>(int workspaceId);
    }
}
