namespace Milyli.ScriptRunner.Web.Models
{
    using System.Collections.Generic;
    using Core.Models;

    public class ScriptListModel
    {
        public List<RelativityWorkspace> RelativityWorkspaces { get; internal set; }

        public RelativityWorkspace RelativityWorkspace { get; set; }

        public List<RelativityScriptModel> RelativityScripts { get; set; }
    }
}