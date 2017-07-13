
namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using LinqToDB;
    using LinqToDB.Data;
    using Interfaces;

    /// <summary>
    /// Base repository class to inherit when read and write functionality will be needed for the entity.
    /// </summary>
    /// <typeparam name="TDataContext">The data context that is used to represent the data source.</typeparam>
    /// <typeparam name="TModel">Type of model the repository is responsible for.</typeparam>
    /// <typeparam name="TKey">Type of the primary key of the model.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Necessary to specify context for child classes")]
    public abstract class BaseReadWriteRepository<TDataContext, TModel, TKey> : BaseReadRepository<TDataContext, TModel, TKey>, IReadWriteRepository<TModel, TKey>
        where TModel : class, IModel<TKey>
        where TKey : struct
        where TDataContext : IDataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseReadWriteRepository{TDataContext, TModel, TKey}"/> class.
        /// </summary>
        /// <param name="dataContext"><see cref="IDataContext"/> dependency.</param>
        protected BaseReadWriteRepository(TDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Creates a new instance of the model in the repository.
        /// </summary>
        /// <param name="model">The model to create.</param>
        /// <returns>The identity of the new instance.</returns>
        public TKey Create(TModel model)
        {
            // TODO: How does InsertWithIdentity work with GUIDs?
            var id = this.DataContext.InsertWithIdentity(model);
            return (id is TKey) ? (TKey)id : (TKey)Convert.ChangeType(id, typeof(TKey), CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public void CreateMany(ICollection<TModel> models)
        {
            // all non-mocked implementations inherit from this, so worth checking
            var dataContextAsConnection = this.DataContext as DataConnection;
            if (dataContextAsConnection != null)
            {
                var bulkCopyOptions = new BulkCopyOptions
                {
                    BulkCopyType = BulkCopyType.MultipleRows // generate large INSERT statement
                };
                dataContextAsConnection.BulkCopy(bulkCopyOptions, models);
            }
            else
            {
                foreach (var model in models)
                {
                    this.DataContext.Insert(model);
                }
            }
        }

        /// <inheritdoc/>
        public bool Delete(TKey key)
        {
            return ParseSingletonRowCount(this.DataContext.GetTable<TModel>().Delete(i => i.Id.Equals(key)));
        }

        /// <inheritdoc/>
        public int DeleteMany(ICollection<TKey> keys)
        {
            return this.DataContext
                .GetTable<TModel>()
                .Delete(i => keys.Contains(i.Id));
        }

        /// <inheritdoc/>
        public bool Update(TModel model)
        {
            return ParseSingletonRowCount(this.DataContext.Update(model));
        }

        private static bool ParseSingletonRowCount(int rowCount)
        {
            switch (rowCount)
            {
                case 0: return false;
                case 1: return true;
                default: throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid number of rows ({0}) for a singleton operation", rowCount), nameof(rowCount));
            }
        }
    }
}
