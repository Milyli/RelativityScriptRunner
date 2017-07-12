// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Interfaces;
    using LinqToDB;

    /// <summary>
    /// Base repository class to inherit when only read functionality will be needed for the entity.
    /// </summary>
    /// <typeparam name="TDataContext">The data context that is used to represent the data source.</typeparam>
    /// <typeparam name="TModel">Type of model this repository is responsible for.</typeparam>
    /// <typeparam name="TKey">Type of primary key that identifies this model.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Necessary to specify context for child classes")]
    public abstract class BaseReadRepository<TDataContext, TModel, TKey> : IReadRepository<TModel, TKey>
        where TModel : class, IModel<TKey>
        where TKey : struct
        where TDataContext : IDataContext
    {
        /// <summary>
        /// Detects redundant calls.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseReadRepository{TDataContext, TModel, TKey}"/> class.
        /// </summary>
        /// <param name="dataContext"><see cref="IDataContext"/> dependency.</param>
        protected BaseReadRepository(TDataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        /// <summary>
        /// Gets or sets the <see cref="IDataContext"/> for the repository.
        /// The data context is visible to all repositories.
        /// </summary>
        protected TDataContext DataContext { get; set; }

        // Override a finalizer only if Dispose(bool disposing) below has code to free unmanaged resources.
        // ~BaseReadRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        /// <summary>
        /// Disposal method for Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // Uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reads a single instance or null from the database for the id given.
        /// </summary>
        /// <param name="id">Id of desired instance.</param>
        /// <returns>An instance of the model or null if the id does not exist in the repository.</returns>
        public TModel Read(TKey id)
        {
            return this.DataContext
                .GetTable<TModel>()
                .FirstOrDefault(i => i.Id.Equals(id));
        }

        /// <inheritdoc />
        public TModel ReadLatest()
        {
            return this.DataContext.GetTable<TModel>().OrderByDescending(i => i.Id).FirstOrDefault();
        }

        /// <inheritdoc />
        public List<TModel> ReadMany(ICollection<TKey> ids)
        {
            return this.DataContext
                .GetTable<TModel>()
                .Where(i => ids.Contains(i.Id))
                .ToList();
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "We need the expression to get the property and object involved.")]
        public void FillNavigationProperty(Expression<Func<TModel>> propertyExpression)
        {
            // This implementation currently handles only navigation properties on the object containing the foreign key.  Future work could handle fields in the other direction.
            var member = (MemberExpression)propertyExpression.Body;
            var memberForeignKeyAttribute = member.Member.GetCustomAttribute<ForeignKeyAttribute>();

            // we can only fill properties if we know what key to use
            if (memberForeignKeyAttribute == null)
            {
                return;
            }

            var parentMember = (MemberExpression)member.Expression.Reduce();
            var parentObject = Expression.Lambda<Func<object>>(parentMember).Compile()();
            var foreignKeyProperty = parentMember.Type.GetProperty(memberForeignKeyAttribute.Name);

            if (member.Type.IsGenericType && member.Type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // handle list navigation properties
                var foreignKeyIds = (List<TKey>)foreignKeyProperty.GetValue(parentObject, null);

                var relatedObjects = this.DataContext.GetTable<TModel>().Where(i => foreignKeyIds.Contains(i.Id)).ToList();

                var param = Expression.Parameter(typeof(List<TModel>), "value");
                var set = Expression.Lambda<Action<List<TModel>>>(Expression.Assign(member, param), param).Compile();
                set(relatedObjects);
            }
            else
            {
                // handle singular navigation properties
                var foreignKeyId = (TKey)foreignKeyProperty.GetValue(parentObject, null);

                var relatedObject = this.Read(foreignKeyId);

                var param = Expression.Parameter(typeof(TModel), "value");
                var set = Expression.Lambda<Action<TModel>>(Expression.Assign(member, param), param).Compile();
                set(relatedObject);
            }
        }

        /// <summary>
        /// Cleans up resources when the class leaves scope.
        /// </summary>
        /// <param name="isDisposing">Indicates whether the calling method is this class' Dispose method.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    // Dispose managed state (managed objects).
                    this.DataContext.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.
                // Set large fields to null.
                this.isDisposed = true;
            }
        }
    }
}
