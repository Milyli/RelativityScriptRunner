namespace Milyli.ScriptRunner.Core.DependencyResolution
{
    using System;
    using Framework.Relativity.Interfaces;
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Relativity.Client;

#pragma warning disable CA1704
    public class RsapiClientFactory : IRelativityClientFactory
    {
        private readonly IRelativityContext context;
        private readonly IHelper helper;

        public RsapiClientFactory(IHelper helper, IRelativityContext context)
        {
            this.helper = helper;
            this.context = context;
        }

        public IRSAPIClient GetRelativityClient()
        {
            var servicesManager = this.helper.GetServicesManager();
            var rsapiClient = servicesManager.CreateProxy<IRSAPIClient>(ExecutionIdentity.System);
            rsapiClient.APIOptions.StrictMode = true;
            rsapiClient.APIOptions.WorkspaceID = this.context.WorkspaceId;
            return rsapiClient;
        }
    }
}
#pragma warning enable CA1704
