// Copyright © 2017 Milyli


namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Repositories
{
    using System.Data.SqlClient;
    using global::Relativity.API;
    using Interfaces;
    using Relativity.Interfaces;

    public abstract class RelativityConnectionFactoryBase : IConnectionFactory
    {
        private readonly IHelper helper;
        private readonly IRelativityContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativityConnectionFactoryBase"/> class.
        /// </summary>
        /// <param name="helper">The Relativity helper for the instance.</param>
        /// <param name="context">The workspace wanted.</param>
        protected RelativityConnectionFactoryBase(IHelper helper, IRelativityContext context)
        {
            this.helper = helper;
            this.context = context;
        }

        /// <inheritdoc/>
        public SqlConnection CreateConnection()
        {
            return this.helper.GetDBContext(this.context.WorkspaceId).GetConnection();
        }
    }
}
