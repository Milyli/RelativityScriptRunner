namespace Milyli.ScriptRunner.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class RelativityScript
    {
        public int RelativityScriptId { get; set; }

        public int WorkspaceId { get; set; }

        public string Name { get; set; }

        public string Descirption { get; set; }

        public int ScriptTimeout { get; set; }
    }
}
