namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Core.Models;

    public class ScriptListModel
    {
        public RelativityWorkspace RelativityWorkspace { get; set; }

        public List<RelativityScriptModel> RelativityScripts { get; set; }
    }
}