namespace Milyli.ScriptRunner.ExternalIHelper.Config
{
	using System;
	using System.Configuration;
	using System.Linq;

	public class EnvironmentsElementCollection : ConfigurationElementCollection
	{
		public EnvironmentElement ForEnvironment(string serverName)
			=> this.Cast<EnvironmentElement>().FirstOrDefault(
				x => string.Equals(x.ExecutingMachineName, serverName, StringComparison.InvariantCultureIgnoreCase));

		protected override ConfigurationElement CreateNewElement()
			=> new EnvironmentElement();

		protected override object GetElementKey(ConfigurationElement element)
			=> ((EnvironmentElement)element).ExecutingMachineName;
	}
}
