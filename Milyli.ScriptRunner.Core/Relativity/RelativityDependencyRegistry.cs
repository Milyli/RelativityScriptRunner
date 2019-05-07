namespace Milyli.ScriptRunner.Core.Relativity
{
	using Configuration;
	using Email;
	using Factories;
	using global::Relativity.API;
	using global::Relativity.Services.Objects;
	using Interfaces;
	using Milyli.ScriptRunner.Core.Tools;
	using MilyliDependencies.Framework.Relativity;
	using Repositories;
	using Repositories.Interfaces;
	using StructureMap;

	public class RelativityDependencyRegistry : Registry
    {
        public RelativityDependencyRegistry(IHelper helper, int workspaceId)
        {
            this.For<IHelper>().Use(helper);
            this.For<IServicesMgr>().Use(helper.GetServicesManager());
            this.For<IObjectManager>().Use(ctx => ctx.GetInstance<IServicesMgr>().CreateProxy<IObjectManager>(ExecutionIdentity.System)).ContainerScoped();
            this.For<IRelativityContext>().Use(new RelativityContext(workspaceId));

            this.For<IInstanceConnectionFactory>().Use<RelativityInstanceConnectionFactory>();
            this.For<IWorkspaceConnectionFactory>().Use<RelativityWorkspaceConnectionFactory>();
            this.For<IWorkspaceDependencyFactory>().Use<RelativityWorkspaceDependencyFactory>();

            this.For<IConfigurationCryptoService>().Use<ConfigurationCryptoService>();
            this.For<IConfigurationRepository<EmailConfiguration>>().Use<ConfigurationRepository<EmailConfiguration>>();
            this.For<IEmailService>().Use<EmailService>();
            this.For<IRelativityScriptProcessor>().Use<RelativityScriptProcessor>();
            this.For<ISearchTableManager>().Use<SearchTableManager>();
		}
    }
}
