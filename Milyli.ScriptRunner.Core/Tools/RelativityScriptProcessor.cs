namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using global::Relativity.API;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Models;

	/// <inheritdoc />
	public class RelativityScriptProcessor : IRelativityScriptProcessor
	{
		private static List<string> textDataTypes = new List<string>
		{
			"date",
			"datetime",
			"text"
		};

		private readonly IHelper relativityHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelativityScriptProcessor"/> class.
		/// </summary>
		/// <param name="relativityHelper"><see cref="IHelper"/> dependency from Relativity.</param>
		public RelativityScriptProcessor(IHelper relativityHelper)
		{
			this.relativityHelper = relativityHelper ?? throw new ArgumentNullException(nameof(relativityHelper));
		}

		/// <summary>
		/// Gets name for a generated table containing a list of documents in a saved search.
		/// </summary>
		/// <param name="tablePrepend">Prepend used when creating the table.</param>
		/// <param name="searchId">Id of the saved search.</param>
		/// <returns>Generated table for the specified saved search.</returns>
		public static string GetSearchTableName(string tablePrepend, int searchId) => tablePrepend + "_" + searchId.ToString();

		/// <inheritdoc />
		public string SubstituteGlobalVariables(int workspaceId, string inputSql)
		{
			inputSql = Regex.Replace(
				inputSql,
				Regex.Escape("#CaseArtifactID#"),
				workspaceId.ToString(),
				RegexOptions.IgnoreCase);
			var workspaceDbContext = this.relativityHelper.GetDBContext(workspaceId);
			var masterPrepend = "[EDDS].[EDDSDBO].";
			if (!workspaceDbContext.IsMasterDatabase)
			{
				var workspaceServerName = workspaceDbContext.ServerName;
				var masterDbContext = this.relativityHelper.GetDBContext(-1);

				// Use four part identifier if server is not same server as EDDS main
				masterPrepend = workspaceServerName == masterDbContext.ServerName ? masterPrepend : $"[{masterDbContext.ServerName}].{masterPrepend}";
			}

			return Regex.Replace(
				inputSql,
				Regex.Escape("#MasterDatabasePrepend#"),
				masterPrepend,
				RegexOptions.IgnoreCase);
		}

		/// <inheritdoc />
		public string SubstituteScriptInputs(
			IEnumerable<JobScriptInput> populatedInputs,
			IEnumerable<RelativityScriptInputDetails> relativityScriptInputDetails,
			string inputSql,
			string searchTablePrepend)
		{
			var mappedInputs = populatedInputs.Join(
				relativityScriptInputDetails,
				p => p.InputId,
				d => d.Id,
				(p, d) => new
				{
					p.InputValue,
					d.Attributes,
					d.Id,
					d.InputType
				});

			foreach (var populatedInput in mappedInputs)
			{
				string replaceString = string.Empty;
				switch (populatedInput.InputType)
				{
					// Field, Sql, Object, Search provider input types are all simply direct substitution
					// In the case of SQL inputs ScriptRunner has already saved the generated value.
					case RelativityScriptInputDetailsScriptInputType.Field:
					case RelativityScriptInputDetailsScriptInputType.Sql:
					case RelativityScriptInputDetailsScriptInputType.SearchProvider:
						replaceString = populatedInput.InputValue;
						break;
					case RelativityScriptInputDetailsScriptInputType.Constant:
						// Constant inputs are typed as strings or directly substituted depending on the underying type
						var containsDataType = populatedInput.Attributes.ContainsKey("DataType");
						if (containsDataType && textDataTypes.Contains(populatedInput.Attributes["DataType"]))
						{
							replaceString = $"'{populatedInput.InputValue}'";
						}
						else if (containsDataType && populatedInput.Attributes["DataType"] == "timezone")
						{
							var timeZoneName = populatedInput.InputValue;
							var hoursToUse = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(tzi => tzi.Id == timeZoneName).GetUtcOffset(DateTime.UtcNow).TotalHours;
							replaceString = string.Format("{0:0.##}", hoursToUse);
						}
						else
						{
							replaceString = populatedInput.InputValue;
						}

						break;
					case RelativityScriptInputDetailsScriptInputType.SavedSearch:
						var searchTableName = GetSearchTableName(searchTablePrepend, Convert.ToInt32(populatedInput.InputValue));
						replaceString = string.Format(
							@"FROM [Document], {0} (NOLOCK)
WHERE {0}.DocId = [Document].ArtifactID",
							searchTableName);
						break;
				}

				inputSql = Regex.Replace(
							inputSql,
							Regex.Escape($"#{populatedInput.Id}#"),
							replaceString,
							RegexOptions.IgnoreCase);
			}

			return inputSql;
		}
	}
}
