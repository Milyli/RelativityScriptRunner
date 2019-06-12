namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
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
		public async Task<IDictionary<int, string>> CreateTablesAsync(
			int workspaceId,
			IEnumerable<int> savedSearchIds,
			int scriptRunnerJobId,
			int timeoutSeconds)
		{
			var tableDictionary = new Dictionary<int, string>();
			const string createTableSql = @"IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
DROP TABLE {0};
CREATE TABLE {0} (DocId INT NOT NULL PRIMARY KEY);
END
ELSE 
BEGIN
CREATE TABLE {0} (DocId INT NOT NULL PRIMARY KEY);
END";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var searchId in savedSearchIds.Distinct())
			{
				var tableName = GenerateTableName(searchId, scriptRunnerJobId);
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
				var position = 1;
				var results = await this.objectManager.QuerySlimAsync(workspaceId, request, position, ObjectManagerQueryBatch);
				using (var connection = dbContext.GetConnection())
				{
					while (results.ResultCount > 0)
					{
						results.Objects.ForEach(o => table.Rows.Add(o.ArtifactID));
						if (table.Rows.Count > BulkCopyBatch)
						{
							BulkInsertResults(table, connection, tableName, timeoutSeconds);
						}

						position = results.CurrentStartIndex + ObjectManagerQueryBatch;
						results = await this.objectManager.QuerySlimAsync(workspaceId, request, position, ObjectManagerQueryBatch);
					}

					if (table.Rows.Count > 0)
					{
						BulkInsertResults(table, connection, tableName, timeoutSeconds);
					}
				}

				tableDictionary.Add(searchId, tableName);
			}

			return tableDictionary;
		}

		/// <inheritdoc />
		public void DeleteTables(int workspaceId, IEnumerable<string> tableNames)
		{
			const string deleteTableSql = @"IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
DROP TABLE {0};
END";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var table in tableNames)
			{
				dbContext.ExecuteNonQuerySQLStatement(string.Format(deleteTableSql, table));
			}
		}

		private static void BulkInsertResults(DataTable dataTable, SqlConnection connection, string destinationTable, int timeoutSeconds)
		{
			var bulkCopy = new SqlBulkCopy(connection);
			bulkCopy.BulkCopyTimeout = timeoutSeconds;
			bulkCopy.DestinationTableName = destinationTable;
			bulkCopy.WriteToServer(dataTable);

			dataTable.Rows.Clear();
		}

		/// <summary>
		/// Generates a name for a generated table containing a list of documents in a saved search.
		/// </summary>
		/// <param name="searchId">Id of the saved search.</param>
		/// <param name="scriptRunnerJobId">Id of the script runner job.</param>
		/// <returns>Generated table for the specified saved search.</returns>
		private static string GenerateTableName(int searchId, int scriptRunnerJobId) =>
			"SavedSearch_" + searchId.ToString() + "_" + scriptRunnerJobId.ToString() + "_" + DateTime.UtcNow.ToFileTimeUtc();
	}
}
