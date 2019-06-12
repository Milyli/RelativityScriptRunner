namespace Milyli.ScriptRunner.Core.Tools
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Handles operations around creating and deleting temporary tables storing documents in a saved search.
	/// </summary>
	public interface ISearchTableManager
	{
		/// <summary>
		/// Creates tables containing artifact Ids of documents in a saved search.
		/// </summary>
		/// <param name="workspaceId">Id of the workspace to create the table in.</param>
		/// <param name="savedSearchIds">List of saved searches to create tables for.</param>
		/// <param name="scriptRunnerJobId">Id of the associated script runner job.</param>
		/// <param name="timeoutSeconds">Number of seconds to wait before a SQL timeout.</param>
		/// <returns>A dictionary containing names of populated save search tables indexed by searchId.</returns>
		Task<IDictionary<int, string>> CreateTablesAsync(
			int workspaceId,
			IEnumerable<int> savedSearchIds,
			int scriptRunnerJobId,
			int timeoutSeconds);

		/// <summary>
		/// Deletes all created tables corresponding to a saved search for the given prepend.
		/// </summary>
		/// <param name="workspaceId">Id of the workspace to create the table in.</param>
		/// <param name="tableNames">List tables to delete.</param>
		void DeleteTables(int workspaceId, IEnumerable<string> tableNames);
	}
}
