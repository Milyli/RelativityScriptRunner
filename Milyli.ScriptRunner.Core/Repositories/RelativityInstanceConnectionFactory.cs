namespace Milyli.ScriptRunner.Core.Repositories
{
    using Relativity.Interfaces;
    using Interfaces;
    using RelativityAPI = global::Relativity.API;

    public class RelativityInstanceConnectionFactory : RelativityConnectionFactoryBase, IRelativityInstanceConnectionFactory, IInstanceConnectionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelativityInstanceConnectionFactory"/> class.
        /// </summary>
        /// <param name="helper">The Relativity helper for the instance.</param>
        /// <param name="context">The workspace wanted.</param>
        public RelativityInstanceConnectionFactory(RelativityAPI.IHelper helper, IRelativityContext context)
            : base(helper, context.GetInstanceContext())
        {
        }
    }
}
