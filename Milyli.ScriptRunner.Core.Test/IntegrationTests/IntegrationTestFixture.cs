// <copyright file="IntegrationTestFixture.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

using Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity;

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Milyli.Framework.Relativity.TestTools;
    using Milyli.Framework.Relativity.TestTools.Extensions;
    using DependencyResolution;
    using Relativity.Client;
    using StructureMap;

    public class IntegrationTestFixture
    {
        public IntegrationTestFixture()
        {
            this.RelativityEnvironment = SimpleConfigSections.Configuration.Get<IRelativityConstants>().GetFirstEnvironmentForExecutingMachine();
            this.Container = new Container(c =>
            {
                c.AddRegistry(new RelativityDependencyRegistry(new TestIHelper(), -1));
                c.AddRegistry(new ScriptRunnerRegistry());
                c.For<IRelativityClientFactory>().Add<RsapiClientFactory>();
            });
        }

        protected IRelativityEnvironment RelativityEnvironment { get; set; }

        protected IContainer Container { get; set; }
    }
}
