namespace Milyli.ScriptRunner.Agent.DependencyResolution
{
    using Milyli.Framework.Relativity;
    using Milyli.Framework.Relativity.Interfaces;
    using Milyli.ScriptRunner.Core.DependencyResolution;
    using Milyli.ScriptRunner.Core.Relativity.Client;
    using Relativity.API;
    using StructureMap;

    public static class ContainerBootstrapper
    {
        public static IContainer Setup(IHelper helper)
        {
            var container = new Container(c =>
            {
                c.For<IRelativityContext>().Use(new RelativityContext(-1));

                c.AddRegistry(new ScriptRunnerRegistry());

                c.ForSingletonOf<IRelativityClientFactory>()
                    .Add<RsapiClientFactory>();
            });
            return container;
        }
    }
}
