using System;
using System.Linq;
using System.Reflection;

namespace Milyli.ScriptRunner.Core.Tools
{
	public static class KcuraAssemblyResolver
	{
		public static Assembly ResolveKcuraAssembly(object sender, ResolveEventArgs args)
		{
			var assemblyDllNames = new string[]
			{
				"kCura.Crypto",
				"kCura.Data.RowDataGateway",
				"kCura.Utility",
				"kCura.Config",
			};

			return assemblyDllNames.Contains(args.Name) ? Assembly.Load("kCura") : null;
		}
	}
}