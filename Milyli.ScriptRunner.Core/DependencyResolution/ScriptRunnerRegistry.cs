namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	using kCura.Relativity.Client;
	using Milyli.Framework.Repositories;
	using Milyli.ScriptRunner.Core.DataContexts;
	using Relativity.Client;
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
				s.WithDefaultConventions();
            });
        }
    }
}
