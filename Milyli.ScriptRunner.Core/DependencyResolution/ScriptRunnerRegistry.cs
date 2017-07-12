
namespace Milyli.ScriptRunner.Core.DependencyResolution
{

    using DataContexts;
    using Repositories;
    using Services;
    using StructureMap;
    using MilyliDependencies.Framework.Repositories;

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
