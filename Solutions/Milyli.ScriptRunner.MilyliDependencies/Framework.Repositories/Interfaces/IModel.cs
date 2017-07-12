// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Repositories.Interfaces
{
    public interface IModel<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Gets the id of the model.
        /// </summary>
        TKey Id { get; }
    }
}
