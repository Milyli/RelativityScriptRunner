namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	using DataContexts;
	using Milyli.ScriptRunner.Core.Tools;
	using Repositories;
	using Repositories.Interfaces;
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
                s.IncludeNamespaceContainingType<IRelativityScriptProcessor>();
                s.IncludeNamespaceContainingType<IPermissionsService>();
                s.IncludeNamespaceContainingType<IPermissionRepository>();
                s.IncludeNamespace("Milyli.ScriptRunner.Core.Repositories");
                s.WithDefaultConventions();
            });
        }
    }
}
