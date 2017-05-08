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
        private ExecutionIdentity defaultExecutionIdentity;

        public RsapiClientFactory(IHelper helper, IRelativityContext context)
        {
            this.helper = helper;
            this.context = context;
            this.defaultExecutionIdentity = ExecutionIdentity.CurrentUser;
        }

        public RsapiClientFactory(IHelper helper, IRelativityContext context, ExecutionIdentity defaultExecutionIdentity)
            : this(helper, context)
        {
            this.defaultExecutionIdentity = defaultExecutionIdentity;
        }

        public IRSAPIClient GetRelativityClient()
        {
            return this.GetRelativityClient(this.defaultExecutionIdentity);
        }

        public IRSAPIClient GetRelativityClient(ExecutionIdentity executionIdentity)
        {
            var servicesManager = this.helper.GetServicesManager();
            var rsapiClient = servicesManager.CreateProxy<IRSAPIClient>(executionIdentity);
            rsapiClient.APIOptions.StrictMode = true;
            rsapiClient.APIOptions.WorkspaceID = this.context.WorkspaceId;
            return rsapiClient;
        }
    }
}
#pragma warning restore CA1704
