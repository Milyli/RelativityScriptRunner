namespace Milyli.ScriptRunner.Core.Relativity.Interfaces
{
    using System;
    using global::Relativity.API;
    using global::Relativity.Services.ServiceProxy;
    using SimpleConfigSections;
    public class TestIHelper : IHelper
    {
        private readonly string _eddsServerName;
        private readonly string _databasePassword = "Test1234!";

        public TestIHelper(string eddsServerName)
        {
            _eddsServerName = eddsServerName;
        }

        public TestIHelper()
            : this(Configuration.Get<IRelativityConstants>().GetFirstEnvironmentForExecutingMachine())
        {
        }

        public TestIHelper(IRelativityEnvironment relativityEnvironment)
        {
            _eddsServerName = relativityEnvironment.Server;
            if (!string.IsNullOrWhiteSpace(relativityEnvironment.DatabasePassword))
            {
                _databasePassword = relativityEnvironment.DatabasePassword;
            }
        }

        public IDBContext GetDBContext(int caseID)
        {
            return new DBContext(GetRowDataGatewayContext(caseID));
        }

        public kCura.Data.RowDataGateway.Context GetRowDataGatewayContext(int caseId)
        {
            return new kCura.Data.RowDataGateway.Context(string.Format(
                "Data Source={0};Initial Catalog=EDDS{1};User Id=eddsdbo;Password={2}",
                _eddsServerName,
                caseId == -1 ? string.Empty : caseId.ToString(),
                _databasePassword
            ));
        }

        public IServicesMgr GetServicesManager()
        {
            var settings = new ServiceFactorySettings(
                new Uri($"https://{_eddsServerName}.milyli.net/Relativity.Services"),
                new Uri($"https://{_eddsServerName}.milyli.net/Relativity.Rest/api"), //not needed by us, but specified
                new UsernamePasswordCredentials("relativity.admin@kcura.com", "Test1234!"));

            return new OnDemandServiceFactory(settings);
        }

        public class OnDemandServiceFactory : IServicesMgr
        {
            private readonly ServiceFactory _factory;

            public OnDemandServiceFactory(ServiceFactorySettings settings)
            {
                _factory = new ServiceFactory(settings);
            }

            public T CreateProxy<T>(ExecutionIdentity ident) where T : IDisposable
            {
                return _factory.CreateProxy<T>();
            }

            public Uri GetRESTServiceUrl()
            {
                throw new NotImplementedException();
            }

            public Uri GetServicesURL()
            {
                throw new NotImplementedException();
            }
        }

        public IUrlHelper GetUrlHelper()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public ILogFactory GetLoggerFactory()
        {
            throw new NotImplementedException();
        }

        public string ResourceDBPrepend()
        {
            throw new NotImplementedException();
        }

        public string ResourceDBPrepend(IDBContext context)
        {
            throw new NotImplementedException();
        }

        public string GetSchemalessResourceDataBasePrepend(IDBContext context)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int workspaceID, int artifactID)
        {
            throw new NotImplementedException();
        }
    }
}
