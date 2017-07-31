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
	    /// Executes an individual script with a given list of inputs
	    /// </summary>
	    /// <param name="script">The <see cref="RelativityScript"/> to run</param>
	    /// <param name="inputs">The <see cref="List{RelativityScriptInput}"/> inputs to the script</param>
	    /// <returns>a <see cref="RelativityScriptResult"/></returns>
	    RelativityScriptResult ExecuteRelativityScript(
	    	kCura.Relativity.Client.DTOs.RelativityScript script,
		    List<RelativityScriptInput> inputs);
    }
}
