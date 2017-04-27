namespace Milyli.ScriptRunner.Data.Repositories
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Data.Models;

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
        List<RelativityScriptInput> GetScriptInputs(RelativityScript script, RelativityWorkspace workspace);
    }
}
