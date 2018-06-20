namespace Milyli.ScriptRunner.Web.DependencyResolution
{
	using Relativity.API;
	using System.Linq;

	public static class RelativityHelperUtil
	{
		public static IHelper GetHelper()
		{
#if DEBUG
			Milyli.ScriptRunner.ExternalIHelper.ExternalIHelper.Settings environment = null;
			try
			{
				environment = Milyli.ScriptRunner.ExternalIHelper.ExternalIHelperConfigSettingsFactory.ForCurrentEnvironment();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(ex.ToString());
			}
			if (environment != null)
			{
				return new Milyli.ScriptRunner.ExternalIHelper.ExternalIHelper(environment);
			}
#endif

			return Relativity.CustomPages.ConnectionHelper.Helper();
		}
	}
}