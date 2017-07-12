// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Factories
{
    using StructureMap;
    using Repositories.Interfaces;
    using Interfaces;
    public class RelativityWorkspaceDependencyFactory : IWorkspaceDependencyFactory
    {
        /// <summary>
        /// The container used to resolve dependencies.
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativityWorkspaceDependencyFactory"/> class.
        /// </summary>
        /// <param name="container">The container used to resolve dependencies.</param>
        public RelativityWorkspaceDependencyFactory(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Resolves dependencies for a given type, set up for a specific Relativity workspace.
        /// </summary>
        /// <typeparam name="TInstance">The type to resolve.</typeparam>
        /// <param name="workspaceId">The workspaceId for which the type is needed</param>
        /// <returns>The instance of the type.</returns>
        public TInstance GetInstance<TInstance>(int workspaceId)
        {
            var newContext = this.container
                .GetInstance<IRelativityContext>()
                .GetWorkspaceContext(workspaceId);
            return this.container
                .With(newContext)
                .GetInstance<TInstance>();
        }
    }
}
