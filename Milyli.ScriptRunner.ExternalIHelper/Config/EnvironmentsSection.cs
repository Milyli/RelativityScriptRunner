namespace Milyli.ScriptRunner.ExternalIHelper.Config
{
	using System;
	using System.Configuration;
	using System.Linq;

	public class EnvironmentsSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(EnvironmentsElementCollection), AddItemName = "add")]
		public EnvironmentsElementCollection EnvironmentsElements
			=> (EnvironmentsElementCollection)this[""];
	}
}
