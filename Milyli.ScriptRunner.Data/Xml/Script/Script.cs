namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Models;
    using System.Linq;

    [XmlRoot(Namespace = "", ElementName = "script")]
    public class Script
    {
        [XmlElement(ElementName = "input")]
        public Input Input { get; set; }

        [XmlElement(ElementName = "action")]
        public ScriptAction Action { get; set; }

        internal IEnumerable<OptionScriptInput> GetOptionInputs()
        {
            foreach (var input in this.Input.Constants)
            {
                if (input.Options?.Any() ?? false)
                {
                    yield return new OptionScriptInput(input.Options)
                    {
                        Name = input.Name,
                        Type = input.Type
                    };
                }
                else
                {
                    yield return new OptionScriptInput()
                    {
                        Name = input.Name,
                        Type = input.Type
                    };
                }
            }
        }

        internal IEnumerable<FieldScriptInput> GetFieldInputs()
        {
            return this.Input.Field?.Select(i => new FieldScriptInput()
            {
                Category = i.Filters.Catagory,
                FieldType = i.Filters.Type
            }) ?? Enumerable.Empty<FieldScriptInput>();
        }

        internal IEnumerable<SqlScriptInput> GetSqlScriptInputs()
        {
            return this.Input.SqlInputs?.Select(i => new SqlScriptInput()
            {
                Query = i.SqlText
            }) ?? Enumerable.Empty<SqlScriptInput>();
        }

    }
}
