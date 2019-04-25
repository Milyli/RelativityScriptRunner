namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class SearchTableManager : ISearchTableManager
	{
		public Task CreateTablesAsync(string searchTablePrepend, int workspaceId, IEnumerable<int> savedSearchids)
		{
			throw new NotImplementedException();
		}

		public void DeleteTables(string searchTablePrepend, int workspaceId, IEnumerable<int> savedSearchids)
		{
			throw new NotImplementedException();
		}
	}
}
