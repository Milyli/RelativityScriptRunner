// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Configuration
{
    using System;

    /// <summary>
    /// Attribute specifying that a configuration value should be stored in encrypted form.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// </summary>
        /// <param name="configurationSection">The name of the configuration section.</param>
        public ConfigurationSectionAttribute(string configurationSection)
        {
            this.ConfigurationSection = configurationSection;
        }

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        public string ConfigurationSection { get; private set; }
    }
}
