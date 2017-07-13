namespace Milyli.ScriptRunner.Core.Relativity.Configuration
{
    using System;
    /// <summary>
    /// Attribute specifying that a configuration value should be stored in encrypted form.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConfigurationEncryptedAttribute : Attribute
    {
    }
}
