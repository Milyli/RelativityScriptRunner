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
		/// <param name="searchTablePrepend">String to prepend to all tables storing saved search values. This should be unique generated for each call.</param>
		/// <param name="workspaceId">Id of the workspace to create the table in.</param>
		/// <param name="savedSearchids">List of saved searches to create tables for.</param>
		/// <param name="scriptRunnerJobId">Id of the associated script runner job.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		Task CreateTablesAsync(
			string searchTablePrepend,
			int workspaceId,
			IEnumerable<int> savedSearchids,
			int scriptRunnerJobId);

		/// <summary>
		/// Deletes all created tables corresponding to a saved search for the given prepend.
		/// </summary>
		/// <param name="searchTablePrepend">String to prepend to the table storing saved search values.</param>
		/// <param name="workspaceId">Id of the workspace to create the table in.</param>
		/// <param name="savedSearchids">List of saved searches to create tables for.</param>
		/// <param name="scriptRunnerJobId">Id of the associated script runner job.</param>
		void DeleteTables(
			string searchTablePrepend,
			int workspaceId,
			IEnumerable<int> savedSearchids,
			int scriptRunnerJobId);
	}
}
