namespace Milyli.ScriptRunner.ExternalIHelper
{
	using System;
	using System.Configuration;
	using Milyli.ScriptRunner.ExternalIHelper.Config;

	public sealed class ExternalIHelperConfigSettingsFactory
	{
		private static EnvironmentsSection config;

		static ExternalIHelperConfigSettingsFactory()
		{
			config = (EnvironmentsSection)ConfigurationManager.GetSection("ExternalIHelperEnvironments");
		}

		private ExternalIHelperConfigSettingsFactory()
		{
		}

		public static ExternalIHelper.Settings ForEnvironment(string serverName)
			=> config.EnvironmentsElements.ForEnvironment(serverName)?.ToIHelperSettings();

		public static ExternalIHelper.Settings ForCurrentEnvironment() => ForEnvironment(Environment.MachineName);
	}
}
