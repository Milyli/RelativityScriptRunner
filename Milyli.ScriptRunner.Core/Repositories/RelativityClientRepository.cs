namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Relativity.Client;

    public abstract class RelativityClientRepository : IDisposable
    {
        private IRelativityClientFactory relativityClientFactory;

        private IRSAPIClient relativityClient;

        private bool disposedValue = false;

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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.RelativityClient?.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
