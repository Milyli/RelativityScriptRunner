// <copyright file="IntegrationTestFixture.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>


using Milyli.ScriptRunner.Core.Relativity;
using Milyli.ScriptRunner.Core.Relativity.Interfaces;

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using DependencyResolution;
    using MilyliDependencies;
    using MilyliDependencies.Framework.Relativity;
    using Relativity.Client;
    using StructureMap;

    public class IntegrationTestFixture
    {
        public IntegrationTestFixture()
        {
						var constants = SimpleConfigSections.Configuration.Get<IRelativityConstants>();
						this.RelativityEnvironment = constants.GetFirstEnvironmentForExecutingMachine();
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
