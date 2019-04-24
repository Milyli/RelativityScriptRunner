namespace Milyli.ScriptRunner.ExternalIHelper
{
	using System;
	using System.Data.SqlClient;
	using global::Relativity.API;
	using global::Relativity.Services.ServiceProxy;
	using Milyli.ScriptRunner.ExternalIHelper.Logging;

	/// <summary>
	/// A testing helper for working with Relativity's databases and helpers.
	/// </summary>
	public class ExternalIHelper : IHelper
	{
		private readonly string eddsServerName;
		private readonly string databasePassword;
		private readonly UsernamePasswordCredentials credentials;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalIHelper"/> class.
		/// Creates a new test helper for the current machine's environment.
		/// </summary>
		public ExternalIHelper()
			: this(ExternalIHelperConfigSettingsFactory.ForCurrentEnvironment())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalIHelper"/> class.
		/// Creates a new test helper for the given database server name.
		/// </summary>
		/// <param name="eddsServerName">The database server name</param>
		public ExternalIHelper(string eddsServerName)
			: this(new Settings { EddsServerName = eddsServerName })
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalIHelper"/> class.
		/// Creates a new test helper using the given <see cref="Settings"/>
		/// </summary>
		/// <param name="settings">The </param>
		public ExternalIHelper(Settings settings)
		{
			this.eddsServerName = settings.EddsServerName;
			this.databasePassword = settings.DatabasePassword;
			this.credentials = new UsernamePasswordCredentials(settings.RsapiUserName, settings.RsapiPassword);
		}

		/// <summary>
		/// Creates a new Relativity data context object.
		/// </summary>
		/// <param name="caseID">Workspace of the <see cref="IDBContext"/> scope.</param>
		/// <returns>A new data context for the given workspace id.</returns>
		public IDBContext GetDBContext(int caseID)
		{
			// required type is actually inside the Relativity.API DLL, just not public.
			var type = typeof(IDBContext).Assembly.GetType("Relativity.API.DBContext");
			return (IDBContext)Activator.CreateInstance(type, this.GetRowDataGatewayContext(caseID));
		}

		/// <summary>
		/// Creates a new RowDataGateway for the given workspace id.
		/// </summary>
		/// <param name="caseId">Workspace id to scope the <see cref="kCura.Data.RowDataGateway"/>.</param>
		/// <returns>A new <see cref="kCura.Data.RowDataGateway.Context"/>.</returns>
		public kCura.Data.RowDataGateway.Context GetRowDataGatewayContext(int caseId)
		{
			var initDB = caseId == -1 ? "EDDS" : $"EDDS{caseId}";
			return new kCura.Data.RowDataGateway.Context(
				new SqlConnectionStringBuilder
				{
					DataSource = this.eddsServerName,
					InitialCatalog = initDB,
					UserID = "eddsdbo",
					Password = this.databasePassword,
					ConnectTimeout = 30 // prevents falling back to non-existing kCura.Config value
				}.ToString());
		}

		/// <summary>
		/// Creates a new <see cref="IServicesMgr"/>.
		/// </summary>
		/// <returns>Returns an <see cref="IServicesMgr"/> for the defined EDDS server.</returns>
		public IServicesMgr GetServicesManager()
		{
			var settings = new ServiceFactorySettings(
				 new Uri($"https://{this.eddsServerName}.milyli.net/Relativity.Services"),
				 new Uri($"https://{this.eddsServerName}.milyli.net/Relativity.Rest/api"), // not needed by us, but specified
				 this.credentials);

			return new OnDemandServiceFactory(settings);
		}

		/// <summary>
		/// Gets an <see cref="IUrlHelper"/>.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public IUrlHelper GetUrlHelper()
		{
            // required type is actually inside the Relativity.API DLL, just not public.
            var type = typeof(IUrlHelper).Assembly.GetType("Relativity.API.UrlHelper");
            Func<string> baseWebsiteUrl = () => $"https://{this.eddsServerName}.milyli.net/";

            return (IUrlHelper)Activator.CreateInstance(type, baseWebsiteUrl);
		}

		/// <summary>
		/// Disposes the TestHelper.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Gets an <see cref="ILogFactory"/>.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public ILogFactory GetLoggerFactory()
		{
			return new TestLogFactory();
		}

		/// <summary>
		/// Gets an <see cref="ILogFactory"/>.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public string ResourceDBPrepend()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets an <see cref="ILogFactory"/>.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public string ResourceDBPrepend(IDBContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the resource database prepend without schema.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public string GetSchemalessResourceDataBasePrepend(IDBContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the application GUID.
		/// </summary>
		/// <returns>A Not Implemented Exception</returns>
		public Guid GetGuid(int workspaceID, int artifactID)
		{
			throw new NotImplementedException();
		}

		public ISecretStore GetSecretStore()
		{
			throw new NotImplementedException();
		}

		public IInstanceSettingsBundle GetInstanceSettingBundle()
		{
			throw new NotImplementedException();
		}

#if V9_6_26_97 || V9_6_85_9
		public ISecretStore GetSecretStore()
		{
			throw new NotImplementedException();
		}
#endif

#if V9_6_85_9
		public IInstanceSettingsBundle GetInstanceSettingBundle()
		{
			throw new NotImplementedException();
		}
#endif
		public class Settings
		{
			public string EddsServerName { get; set; }

			public string DatabasePassword { get; set; } = "Test1234!";

			public string RsapiUserName { get; set; } = "relativity.admin@kcura.com";

			public string RsapiPassword { get; set; } = "Test1234!";
		}

		private class OnDemandServiceFactory : IServicesMgr
		{
			private readonly ServiceFactory factory;
			private readonly ServiceFactorySettings settings;

			public OnDemandServiceFactory(ServiceFactorySettings settings)
			{
				this.settings = settings;
				this.factory = new ServiceFactory(settings);
			}

			public T CreateProxy<T>(ExecutionIdentity ident)
				where T : IDisposable
			{
				return this.factory.CreateProxy<T>();
			}

			public Uri GetRESTServiceUrl()
			{
				return this.settings.RelativityRestUri;
			}

			public Uri GetServicesURL()
			{
				return this.settings.RelativityServicesUri;
			}
		}
	}
}
