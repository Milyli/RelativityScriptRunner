// <copyright file="AgentRegistry.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.DependencyResolution
{
    using Core.Services;
    using Milyli.Framework.Relativity;
    using Milyli.Framework.Relativity.Factories;
    using Milyli.Framework.Relativity.Interfaces;
    using Milyli.Framework.Repositories.Interfaces;
    using Milyli.ScriptRunner.Core.DependencyResolution;
    using Milyli.ScriptRunner.Core.Relativity.Client;
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
