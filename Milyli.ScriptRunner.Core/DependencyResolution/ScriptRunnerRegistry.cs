namespace Milyli.ScriptRunner.Core.DependencyResolution
{
    using Milyli.Framework.Repositories;
    using Milyli.ScriptRunner.Core.DataContexts;
    using Repositories;
    using Services;
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
                s.IncludeNamespaceContainingType<IPermissionsService>();
                s.IncludeNamespaceContainingType<IPermissionRepository>();
                s.WithDefaultConventions();
            });
        }
    }
}
