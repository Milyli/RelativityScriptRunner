namespace Milyli.ScriptRunner.Data.DependencyResolution
{
    using Milyli.Framework.Repositories;
    using Milyli.ScriptRunner.Data.DataContexts;
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
        }
    }
}
