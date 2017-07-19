namespace Milyli.ScriptRunner.Core.Relativity.Factories
{
    using System.Data.SqlClient;
    using Interfaces;
    using Repositories.Interfaces;
    using global::Relativity.API;

    /// <summary>
    /// Base factory for Relativity connections
    /// </summary>
    public abstract class RelativityConnectionFactoryBase : IConnectionFactory
    {
        private IHelper helper;
        private IRelativityContext context;

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
