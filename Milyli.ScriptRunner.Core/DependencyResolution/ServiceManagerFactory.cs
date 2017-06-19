namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	using System;
	using global::Relativity.API;

	public class ServiceManagerFactory : BaseServicesFactory, IServiceManagerFactory
	{
		public ServiceManagerFactory(IHelper helper)
			: base(helper)
		{
		}

		public T GetServiceProxy<T>()
			where T : IDisposable
		{
			return this.GetServiceProxy<T>(this.DefaultExecutionIdentity);
		}

		public T GetServiceProxy<T>(ExecutionIdentity executionIdentity)
			where T : IDisposable
		{
			return this.Helper.GetServicesManager().CreateProxy<T>(executionIdentity);
		}
	}
}
