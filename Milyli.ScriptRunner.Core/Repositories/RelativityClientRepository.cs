namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Relativity.Client;

    public abstract class RelativityClientRepository : IDisposable
    {
        private IRelativityClientFactory relativityClientFactory;

        private IRSAPIClient relativityClient;

        protected RelativityClientRepository(IRelativityClientFactory relativityClientFactory)
        {
            this.relativityClientFactory = relativityClientFactory;
        }

        protected IRSAPIClient RelativityClient
        {
            get
            {
                if (this.relativityClient == null)
                {
                    this.relativityClient = this.relativityClientFactory.GetRelativityClient(ExecutionIdentity.System);
                }

                return this.relativityClient;
            }
        }

        public void Dispose()
        {
            this.relativityClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
