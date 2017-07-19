
using Milyli.ScriptRunner.Core.Relativity.Interfaces;

namespace Milyli.ScriptRunner.Core.DependencyResolution
{
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Relativity.Client;

#pragma warning disable CA1704
    public class RsapiClientFactory : BaseServicesFactory, IRelativityClientFactory
    {
        private readonly IRelativityContext context;

        public RsapiClientFactory(IHelper helper, IRelativityContext context)
            : base(helper)
        {
            this.context = context;
        }

        public RsapiClientFactory(IHelper helper, IRelativityContext context, ExecutionIdentity defaultExecutionIdentity)
            : base(helper, defaultExecutionIdentity)
        {
            this.context = context;
        }

        public IRSAPIClient GetRelativityClient()
        {
            return this.GetRelativityClient(this.DefaultExecutionIdentity);
        }

        public IRSAPIClient GetRelativityClient(ExecutionIdentity executionIdentity)
        {
            var servicesManager = this.Helper.GetServicesManager();
            var rsapiClient = servicesManager.CreateProxy<IRSAPIClient>(executionIdentity);
            rsapiClient.APIOptions.StrictMode = true;
            rsapiClient.APIOptions.WorkspaceID = this.context.WorkspaceId;
            return rsapiClient;
        }
    }
}
#pragma warning restore CA1704
