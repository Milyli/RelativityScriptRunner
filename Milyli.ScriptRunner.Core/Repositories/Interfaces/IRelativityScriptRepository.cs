namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
	using System.Collections.Generic;
	using kCura.Relativity.Client;
	using Models;

	public interface IRelativityScriptRepository
	{
		/// <summary>
		/// Returns the list of scripts for a given workspace
		/// </summary>
		/// <param name="workspace">the application-specific workspace</param>
		/// <returns>a list of relativity script models</returns>
		List<RelativityScript> GetRelativityScripts(RelativityWorkspace workspace);

		/// <summary>
		/// Returns the list of given inputs for a script
		/// </summary>
		/// <param name="script">the application model representing a relativity script</param>
		/// <param name="workspace">the workspace we exect to execute the script in</param>
		/// <returns>A list of inputs</returns>
		List<ScriptInput> GetScriptInputs(RelativityScript script, RelativityWorkspace workspace);

		/// <summary>
		/// Returns an individual script resource in a workspace for a given script id
		/// </summary>
		/// <param name="workspace">the relativity workspace</param>
		/// <param name="scriptArtifactId">the artifact id for the script in the given workspace</param>
		/// <returns>a relativity script</returns>
		RelativityScript GetRelativityScript(RelativityWorkspace workspace, int scriptArtifactId);

		/// <summary>
		/// Executes an individual script with a given list of inputs using RSAPI.
		/// </summary>
		/// <param name="scriptArtifactId">ArtifactId of the <see cref="RelativityScript"/> to run</param>
		/// <param name="inputs">The <see cref="List{JobScriptInput}"/> inputs to the script</param>
		/// <param name="workspace">the relativity workspace</param>
		/// <returns>a <see cref="ExecuteResult"/></returns>
		ExecuteResult ExecuteRelativityScript(
				int scriptArtifactId,
				List<JobScriptInput> inputs,
				RelativityWorkspace workspace);

		/// <summary>
		/// Executes an individual script with a given list of inputs through a direct SQL connection.
		/// </summary>
		/// <param name="scriptArtifactId">ArtifactId of the Relativity Script to run.</param>
		/// <param name="inputs">The <see cref="List{JobScriptInput}"/> inputs to the script</param>
		/// <param name="workspace">the relativity workspace</param>
		/// <param name="timeOutSeconds">Number of seconds to allow before the script execution times out.</param>
		/// <returns><see cref="ExecuteResult"/> indicating whether the script executed successfully.</returns>
		ExecuteResult ExecuteScriptDirectSql(
				int scriptArtifactId,
				List<JobScriptInput> inputs,
				RelativityWorkspace workspace,
				int timeOutSeconds = 600);
	}
}
