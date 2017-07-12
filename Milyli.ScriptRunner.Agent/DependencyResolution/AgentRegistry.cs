// <copyright file="AgentRegistry.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.DependencyResolution
{
    using Core.DependencyResolution;
    using Core.Relativity.Client;
    using Core.Services;
    using MilyliDependencies.Framework.Relativity;
    using MilyliDependencies.Framework.Relativity.Interfaces;
    using MilyliDependencies.Framework.Repositories;
    using MilyliDependencies.Framework.Repositories.Interfaces;
    using Relativity.API;
    using StructureMap;

    public class AgentRegistry : Registry
    {
        public AgentRegistry(IHelper helper)
        {
            this.For<IHelper>().Use(helper);
            this.For<IRelativityContext>().Use(new RelativityContext(-1));
            this.For<IRelativityClientFactory>().Use<RsapiClientFactory>();
            this.For<IInstanceConnectionFactory>().Use<RelativityInstanceConnectionFactory>();
            this.For<IRelativityScriptRunner>().Use<RelativityScriptRunner>();
            this.IncludeRegistry(new ScriptRunnerRegistry());
        }
    }
}
