namespace Milyli.ScriptRunner.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ScriptXml = Milyli.ScriptRunner.Data.Xml.Script;

    public class OptionScriptInput : ScriptInput
    {
        public OptionScriptInput()
        {
        }

        public OptionScriptInput(List<ScriptXml.Option> options)
        {
            this.AvailableOptions = options.Select(o => new ScriptOption(o)).ToList();
        }

        public List<ScriptOption> AvailableOptions { get; private set; }
    }
}
