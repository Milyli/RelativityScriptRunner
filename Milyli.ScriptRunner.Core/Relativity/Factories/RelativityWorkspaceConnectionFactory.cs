namespace Milyli.ScriptRunner.Core.Relativity.Factories
{
    using System;
    using Interfaces;
    using global::Relativity.API;

    /// <summary>
    /// Factory to get the database connection for a particular Relativity workspace
    /// </summary>
    public class RelativityWorkspaceConnectionFactory : RelativityConnectionFactoryBase, IRelativityWorkspaceConnectionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelativityWorkspaceConnectionFactory"/> class.
        /// </summary>
        /// <param name="helper">The Relativity helper for the instance.</param>
        /// <param name="context">The workspace wanted.</param>
        public RelativityWorkspaceConnectionFactory(IHelper helper, IRelativityContext context)
            : base(helper, VerifyContext(context))
        {
        }

        private static IRelativityContext VerifyContext(IRelativityContext context)
        {
            if (context.WorkspaceId <= 0)
            {
                throw new ArgumentException("Context does not contain a valid WorkspaceId");
            }

            return context;
        }
    }
}
