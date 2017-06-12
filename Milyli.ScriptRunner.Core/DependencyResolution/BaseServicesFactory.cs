namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	using global::Relativity.API;

	/// <summary>
	/// Base class for anything needing IHelper to generate services proxies (eg RSAPI)
	/// </summary>
	public abstract class BaseServicesFactory
	{
		protected BaseServicesFactory(IHelper helper)
		{
			this.Helper = helper;
			this.DefaultExecutionIdentity = ExecutionIdentity.CurrentUser;
		}

		protected BaseServicesFactory(IHelper helper, ExecutionIdentity defaultExecutionIdentity)
		{
			this.Helper = helper;
			this.DefaultExecutionIdentity = defaultExecutionIdentity;
		}

		protected IHelper Helper { get; private set; }

		protected ExecutionIdentity DefaultExecutionIdentity { get; private set; }
	}
}
