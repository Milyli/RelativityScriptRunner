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

			const string createStagingTableSqlTemplate = @"
IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
	DROP TABLE {0};
	CREATE TABLE {0} (DocId INT);
END ELSE 
BEGIN
	CREATE TABLE {0} (DocId INT);
END";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var searchId in savedSearchIds.Distinct())
			{
				var tableName = GenerateTableName(searchId, scriptRunnerJobId);
				var stagingName = $"{tableName}_Staging";
				var createStagingTableSql = string.Format(createStagingTableSqlTemplate, stagingName);
				dbContext.ExecuteNonQuerySQLStatement(createStagingTableSql);

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
						BulkInsertResults(table, dbContext, stagingName, timeoutSeconds);
					}

					position = results.CurrentStartIndex + ObjectManagerQueryBatch;
					results = await this.objectManager.QuerySlimAsync(workspaceId, request, position, ObjectManagerQueryBatch);
				}

				if (table.Rows.Count > 0)
				{
					BulkInsertResults(table, dbContext, stagingName, timeoutSeconds);
				}

				const string createSearchTableSqlTemplate = @"
IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
	DROP TABLE {0};
	CREATE TABLE {0} (DocId INT NOT NULL PRIMARY KEY);
END ELSE 
BEGIN
	CREATE TABLE {0} (DocId INT NOT NULL PRIMARY KEY);
END";
				var createSearchTableSql = string.Format(createSearchTableSqlTemplate, tableName);
				dbContext.ExecuteNonQuerySQLStatement(createSearchTableSql);

				var unstageSavedSearchSqlTemplate = @"
INSERT INTO {0} (DocId)
SELECT DISTINCT DocId FROM {1} WHERE DocId IS NOT NULL
";
				var unstageSavedSearchSql = string.Format(unstageSavedSearchSqlTemplate, tableName, stagingName);
				dbContext.ExecuteNonQuerySQLStatement(createSearchTableSql);

				tableDictionary.Add(searchId, tableName);
			}

			return tableDictionary;
		}

		/// <inheritdoc />
		public void DeleteTables(int workspaceId, IEnumerable<string> tableNames)
		{
			const string deleteTablesSqlTemplate = @"
IF OBJECT_ID('{0}') IS NOT NULL
BEGIN
	DROP TABLE {0};
END

IF OBJECT_ID('{0}_Staging') IS NOT NULL
BEGIN
	DROP TABLE {0}_Staging;
END
";
			var dbContext = this.relativityHelper.GetDBContext(workspaceId);
			foreach (var table in tableNames)
			{
				dbContext.ExecuteNonQuerySQLStatement(string.Format(deleteTablesSqlTemplate, table));
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
