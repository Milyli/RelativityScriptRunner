// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    public interface IReadRepository<TModel, TKey> : IDisposable
        where TModel : class, IModel<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Reads the model instance from the database corresponding to the given id.
        /// </summary>
        /// <param name="id">Id of the instance to read.</param>
        /// <returns>The instance with the given id or null if one does not exist.</returns>
        TModel Read(TKey id);

        /// <summary>
        /// Reads a model instance from the database with the greatest available Id.
        /// </summary>
        /// <returns>The model instance from the database with the greatest available Id.</returns>
        TModel ReadLatest();

        /// <summary>
        /// Reads the models instances from the database corresponding to the given ids
        /// </summary>
        /// <param name="ids">Ids of the instances to read.</param>
        /// <returns>Any instances found with one of the given ids.</returns>
        List<TModel> ReadMany(ICollection<TKey> ids);

        /// <summary>
        /// Populate the specified property from the database.
        /// </summary>
        /// <param name="propertyExpression">The property to populate.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "We need the expression to get the property and object involved.")]
        void FillNavigationProperty(Expression<Func<TModel>> propertyExpression);
    }
}
