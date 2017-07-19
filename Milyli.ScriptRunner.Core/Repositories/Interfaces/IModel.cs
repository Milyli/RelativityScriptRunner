namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
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
