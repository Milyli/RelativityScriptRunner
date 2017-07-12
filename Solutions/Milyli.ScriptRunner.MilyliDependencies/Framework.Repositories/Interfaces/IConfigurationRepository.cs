// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Repositories.Interfaces
{
    using System;

    /// <summary>
    /// Reads and writes a configuration section in Relativity's instance settings.
    /// </summary>
    /// <typeparam name="T">The type of the model representing the configuration section.</typeparam>
    public interface IConfigurationRepository<T> : IDisposable
    {
        /// <summary>
        /// Reads the configuration section specified by the model's attributes from the database.
        /// </summary>
        /// <returns>A model representing a configuration section.</returns>
        T ReadConfiguration();

        /// <summary>
        /// Updates the configuration section in the database, overwriting existing rows values but not creating new rows.
        /// </summary>
        /// <param name="configuration">The information to be written to the database.</param>
        void SetConfiguration(T configuration);
    }
}
