namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    using System.Collections.Generic;
    public interface IReadWriteRepository<TModel, TKey> : IReadRepository<TModel, TKey>
        where TModel : class, IModel<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Creates a new instance of the model in the repository.
        /// </summary>
        /// <param name="model">The instance of the model to create.</param>
        /// <returns>The identity of the new instance.</returns>
        TKey Create(TModel model);

        /// <summary>
        /// Creates new instances of the model in the repository.
        /// </summary>
        /// <param name="models">The instances of the now model to create</param>
        void CreateMany(ICollection<TModel> models);

        /// <summary>
        /// Deletes an existing instance of the model in the repository.
        /// </summary>
        /// <param name="key">The key of the model to delete.</param>
        /// <returns>Whether the model to delete was found (and deleted).</returns>
        bool Delete(TKey key);

        /// <summary>
        /// Deletes existing instances of models in the repository.
        /// </summary>
        /// <param name="keys">The keys of the models to delete</param>
        /// <returns>The number of models found (and deleted).</returns>
        int DeleteMany(ICollection<TKey> keys);

        /// <summary>
        /// Updates an existing instance of the model in the repository.
        /// </summary>
        /// <param name="model">The instance of the model to update.</param>
        /// <returns>Whether the model to delete was found (and updated).</returns>
        bool Update(TModel model);
    }
}
