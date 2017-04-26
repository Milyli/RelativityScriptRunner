namespace Milyli.ScriptRunner.Data.Relativity.Client
{
    using System;
    using kCura.Relativity.Client;

    /// <summary>
    /// RelativityServicesClientFactory provides an implementation of the IRelativityClientFactory that uses the RelativityServices instance at uri with a user defined by username and password
    /// </summary>
    public class RelativityServicesClientFactory : IRelativityClientFactory
    {
        public RelativityServicesClientFactory(string username, string password, string uri)
        {
            this.uri = new Uri(uri);
            this.credentials = new UsernamePasswordCredentials(username, password);
        }

#pragma warning disable SA1201 // Elements must appear in the correct order
        private Uri uri;

        private UsernamePasswordCredentials credentials;
#pragma warning restore SA1201 // Elements must appear in the correct order

        public IRSAPIClient GetRelativityClient()
        {
            return new RSAPIClient(this.uri, this.credentials);
        }
    }
}
