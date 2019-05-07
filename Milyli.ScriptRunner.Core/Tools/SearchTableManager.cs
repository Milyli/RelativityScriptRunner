namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Threading.Tasks;
	using global::Relativity.API;
	using global::Relativity.Services.Objects;
	using global::Relativity.Services.Objects.DataContracts;
	using kCura.Relativity.Client;

	/// <inheritdoc />
	public class SearchTableManager : ISearchTableManager
	{
		private const int ObjectManagerQueryBatch = 1000;
		private const int BulkCopyBatch = 100000;
		private readonly IObjectManager objectManager;
		private readonly IHelper relativityHelper;

		public SearchTableManager(IObjectManager objectManager, IHelper helper)
		{
			this.objectManager = objectManager ?? throw new ArgumentNullException(nameof(objectManager));
			this.relativityHelper = helper ?? throw new ArgumentNullException(nameof(helper));
		}

		/// <inheritdoc />
		public async Task CreateTablesAsync(
			string searchTablePrepend,
			int workspaceId,
			IEnumerable<int> savedSearchids,
			int scriptRunnerJobId,
			int timeoutSeconds)
		{
			const string createTableSql = @"IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
DROP TABLE {0}
CREATE TABLE {0} (DocId int)
END
ELSE CREATE TABLE {0} (DocId int)";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var searchId in savedSearchids)
			{
				var tableName = RelativityScriptProcessor.GetSearchTableName(searchTablePrepend, searchId, scriptRunnerJobId);
				var createSearchTableSql = string.Format(createTableSql, tableName);
				dbContext.ExecuteNonQuerySQLStatement(createSearchTableSql);
				var table = new DataTable();
				table.Columns.Add("DocId", typeof(int));

				var request = new QueryRequest
				{
					Condition = $"'ArtifactId' IN SAVEDSEARCH {searchId}",
					ObjectType = new ObjectTypeRef { ArtifactTypeID = (int)ArtifactType.Document },
					Fields = new List<FieldRef>(),
				};
				var position = 0;
				var results = await this.objectManager.QuerySlimAsync(workspaceId, request, position, ObjectManagerQueryBatch);
				while (results.ResultCount > 0)
				{
					results.Objects.ForEach(o => table.Rows.Add(o.ArtifactID));
					if (table.Rows.Count > BulkCopyBatch)
					{
						BulkInsertResults(table, dbContext, tableName, timeoutSeconds);
					}

					position = results.CurrentStartIndex + ObjectManagerQueryBatch;
					results = await this.objectManager.QuerySlimAsync(workspaceId, request, position, ObjectManagerQueryBatch);
				}

				if (table.Rows.Count > 0)
				{
					BulkInsertResults(table, dbContext, tableName, timeoutSeconds);
				}
			}
		}

		/// <inheritdoc />
		public void DeleteTables(
			string searchTablePrepend,
			int workspaceId,
			IEnumerable<int> savedSearchids,
			int scriptRunnerJobId)
		{
			const string deleteTableSql = @"IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
DROP TABLE {0}
END";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var searchId in savedSearchids)
			{
				var tableName = RelativityScriptProcessor.GetSearchTableName(searchTablePrepend, searchId, scriptRunnerJobId);
				dbContext.ExecuteNonQuerySQLStatement(string.Format(deleteTableSql, tableName));
			}
		}

		private static void BulkInsertResults(DataTable dataTable, IDBContext dbContext, string destinationTable, int timeoutSeconds)
		{
			using (var connection = dbContext.GetConnection())
			{
				var bulkCopy = new SqlBulkCopy(connection);
				bulkCopy.BulkCopyTimeout = timeoutSeconds;
				bulkCopy.DestinationTableName = destinationTable;
				bulkCopy.WriteToServer(dataTable);
			}

			dataTable.Rows.Clear();
		}
	}
}
