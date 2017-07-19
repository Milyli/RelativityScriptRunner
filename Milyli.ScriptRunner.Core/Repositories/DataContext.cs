
namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Data.SqlClient;
    using LinqToDB.Data;
    using LinqToDB.DataProvider;
    using LinqToDB.DataProvider.SqlServer;
    using Interfaces;

    public abstract class DataContext : DataConnection
    {
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="factory">The <see cref="IConnectionFactory"/> to use for making connections.</param>
        protected DataContext(IConnectionFactory factory)
            : this(factory.CreateConnection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="factory">The <see cref="IConnectionFactory"/> to use for making connections.</param>
        /// <param name="dataProvider">The <see cref="IDataProvider"/> to use for making connections.</param>
        protected DataContext(IConnectionFactory factory, IDataProvider dataProvider)
            : base(dataProvider, factory.CreateConnection())
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }

        private DataContext(SqlConnection connection)
            : base(DetermineProvider(connection), connection)
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }

        private static IDataProvider DetermineProvider(SqlConnection connection)
        {
            var sqlVersion = connection.ServerVersion.StartsWith("11.", System.StringComparison.Ordinal) ?
                SqlServerVersion.v2008 :
                SqlServerVersion.v2012;
            return SqlServerTools.GetDataProvider(sqlVersion);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) below.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up resources when the class leaves scope.
        /// </summary>
        /// <param name="disposing">Indicates whether the calling method is this class' Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    base.Dispose();

                    // Need to dispose of the connection if it has not been disposed yet.
                    // This happens when the connection passed into the context was already open.
                    this.Connection?.Dispose();
                }

                this._disposedValue = true;
            }
        }
    }
}
