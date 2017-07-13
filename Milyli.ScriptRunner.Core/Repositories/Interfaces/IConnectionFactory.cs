namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    using System.Data.SqlClient;
    /// <summary>
    /// Creates connections for the data context.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Gets a SqlConnection for the given context.
        /// </summary>
        /// <returns>A connection.</returns>
        SqlConnection CreateConnection();
    }
}
