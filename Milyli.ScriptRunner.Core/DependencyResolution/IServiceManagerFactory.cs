namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	using System;
	using global::Relativity.API;

	public interface IServiceManagerFactory
	{
		T GetServiceProxy<T>()
			where T : IDisposable;

		T GetServiceProxy<T>(ExecutionIdentity executionIdentity)
			where T : IDisposable;
	}
}
