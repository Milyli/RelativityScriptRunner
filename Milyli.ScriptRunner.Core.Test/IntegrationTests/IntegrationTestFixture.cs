namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using Milyli.Framework.Relativity.IOC;
    using Milyli.Framework.Relativity.TestTools;
    using Milyli.Framework.Relativity.TestTools.Extensions;
    using Milyli.ScriptRunner.Core.DependencyResolution;
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
