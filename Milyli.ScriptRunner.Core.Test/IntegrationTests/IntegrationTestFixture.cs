// <copyright file="IntegrationTestFixture.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>


using Milyli.ScriptRunner.Core.Relativity;
using Milyli.ScriptRunner.Core.Relativity.Interfaces;

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using DependencyResolution;
	using Milyli.ScriptRunner.ExternalIHelper;
	using MilyliDependencies;
    using MilyliDependencies.Framework.Relativity;
    using Relativity.Client;
    using StructureMap;

    public class IntegrationTestFixture
    {
        public IntegrationTestFixture()
        {
            this.Container = new Container(c =>
            {
                c.AddRegistry(new RelativityDependencyRegistry(new ExternalIHelper(), -1));
                c.AddRegistry(new ScriptRunnerRegistry());
                c.For<IRelativityClientFactory>().Add<RsapiClientFactory>();
            });
        }
        protected IContainer Container { get; set; }
    }
}
