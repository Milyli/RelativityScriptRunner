using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relativity.API;

namespace Milyli.ScriptRunner.Core.DependencyResolution
{
	public class ServiceManagerFactory : BaseServicesFactory
	{
		public ServiceManagerFactory(IHelper helper)
			: base(helper)
		{
		}

		public T GetServiceProxy<T>()
			where T : IDisposable
		{
			return this.GetServicesProxy<T>(this.DefaultExecutionIdentity);
		}

		public T GetServicesProxy<T>(ExecutionIdentity executionIdentity)
			where T : IDisposable
		{
			return this.Helper.GetServicesManager().CreateProxy<T>(executionIdentity);
		}
	}
}
