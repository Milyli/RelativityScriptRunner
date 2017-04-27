namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class Input
    {
#pragma warning disable CA2227
        [XmlElement(ElementName = "constant")]
        public List<Constant> Constants { get; set; } = new List<Constant>();

        [XmlElement(ElementName = "sql")]
        public List<SqlInput> SqlInputs { get; set; } = new List<SqlInput>();

        [XmlElement(ElementName = "field")]
        public List<Field> Field { get; set; } = new List<Field>();

#pragma warning restore CA2227
    }
}
