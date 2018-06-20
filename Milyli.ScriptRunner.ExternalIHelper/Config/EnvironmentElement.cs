namespace Milyli.ScriptRunner.ExternalIHelper.Config
{
	using System;
	using System.Configuration;
	using System.Linq;

	public class EnvironmentElement : ConfigurationElement
	{
		[ConfigurationProperty(nameof(ExecutingMachineName), IsRequired = true)]
		public string ExecutingMachineName
		{
			get => (string)this[nameof(this.ExecutingMachineName)];
			set => this[nameof(this.ExecutingMachineName)] = value;
		}

		[ConfigurationProperty(nameof(EddsServerName), IsRequired = true)]
		public string EddsServerName
		{
			get => (string)this[nameof(this.EddsServerName)];
			set => this[nameof(this.EddsServerName)] = value;
		}

		[ConfigurationProperty(nameof(DatabasePassword), IsRequired = false)]
		public string DatabasePassword
		{
			get => (string)this[nameof(this.DatabasePassword)];
			set => this[nameof(this.DatabasePassword)] = value;
		}

		[ConfigurationProperty(nameof(RsapiUserName), IsRequired = false)]
		public string RsapiUserName
		{
			get => (string)this[nameof(this.RsapiUserName)];
			set => this[nameof(this.RsapiUserName)] = value;
		}

		[ConfigurationProperty(nameof(RsapiPassword), IsRequired = false)]
		public string RsapiPassword
		{
			get => (string)this[nameof(this.RsapiPassword)];
			set => this[nameof(this.RsapiPassword)] = value;
		}

		public ExternalIHelper.Settings ToIHelperSettings()
		{
			var settings = new ExternalIHelper.Settings() { EddsServerName = this.EddsServerName };

			if (!string.IsNullOrWhiteSpace(this.DatabasePassword))
			{
				settings.DatabasePassword = this.DatabasePassword;
			}

			if (!string.IsNullOrWhiteSpace(this.RsapiUserName))
			{
				settings.RsapiUserName = this.RsapiUserName;
			}

			if (!string.IsNullOrWhiteSpace(this.RsapiPassword))
			{
				settings.RsapiPassword = this.RsapiPassword;
			}

			return settings;
		}
	}
}
