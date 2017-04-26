namespace Milyli.ScriptRunner.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class RelativityScript
    {
        public Guid RelativityScriptId { get; set; }

        public int ScriptTimeout { get; set; }

#pragma warning disable CA2227
        public List<ScriptInput> Inputs { get; set; }
#pragma warning restore CA2227
    }
}
