namespace Milyli.ScriptRunner.Data.DependencyResolution
{
    using kCura.Relativity.Client;
    using Milyli.Framework.Repositories;
    using Milyli.ScriptRunner.Data.DataContexts;
    using Relativity.Client;
    using Repositories;
    using StructureMap;

    public class ScriptRunnerRegistry : Registry
    {
        public ScriptRunnerRegistry()
        {
            this.Redirect<DataContext, InstanceDataContext>();
            this.ForSingletonOf<InstanceDataContext>();
            this.Scan(s =>
            {
                s.AssemblyContainingType<IJobScheduleRepository>();
                s.IncludeNamespaceContainingType<IJobScheduleRepository>();
                s.WithDefaultConventions();
            });

            this.For<IRSAPIClient>()
                .Use(ctx => ctx.GetInstance<IRelativityClientFactory>().GetRelativityClient())
                .ContainerScoped();
        }
    }
}
