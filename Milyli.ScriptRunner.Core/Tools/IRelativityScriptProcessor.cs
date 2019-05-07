namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Models;

	/// <summary>
	/// Interface for processing Relativity Scripts and inputs.
	/// </summary>
	public interface IRelativityScriptProcessor
	{
		/// <summary>
		/// Substitutes global script variables used in Relativity script SQL.
		/// </summary>
		/// <param name="workspaceId">ArtifactId of the workspace the script will be executed in.</param>
		/// <param name="inputSql">Sql to substitute global variables for.</param>
		/// <returns>Transformed SQL text with Relativity Script global script variables substituted.</returns>
		/// <remarks>Currently does not handle #LoggedInUserID#.</remarks>
		string SubstituteGlobalVariables(int workspaceId, string inputSql);

		/// <summary>
		/// Substitutes run-time script inputs used in Relativity script SQL other than saved searches.
		/// </summary>
		/// <param name="populatedInputs">List of populated inputs stored by ScriptRunner to use for the script execution.</param>
		/// <param name="relativityScriptInputDetails">List of script input descriptions from Relativity.</param>
		/// <param name="inputSql">Sql to substitute global variables for.</param>
		/// <param name="searchTablePrepend">String prepended to any tables created to store saved search results. This should be unique for a given script execution.</param>
		/// <param name="scriptRunnerJobId">Id of the associated script runner job.</param>
		/// <returns>Transformed SQL text with Relativity Script with script inputs substituted with appropriate values.</returns>
		string SubstituteScriptInputs(
			IEnumerable<JobScriptInput> populatedInputs,
			IEnumerable<RelativityScriptInputDetails> relativityScriptInputDetails,
			string inputSql,
			string searchTablePrepend,
			int scriptRunnerJobId);

		/// <summary>
		/// Gets a list of saved search Ids used in execution of a relativity script.
		/// </summary>
		/// <param name="populatedInputs">List of populated inputs stored by ScriptRunner to use for the script execution.</param>
		/// <param name="relativityScriptInputDetails">List of script input descriptions from Relativity.</param>
		/// <returns>List of saved search Ids to be used during script execution.</returns>
		IList<int> GetSavedSearchIds(
			IEnumerable<JobScriptInput> populatedInputs,
			IEnumerable<RelativityScriptInputDetails> relativityScriptInputDetails);
	}
}
