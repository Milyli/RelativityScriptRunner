namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class Constant : AInput
    {
#pragma warning disable CA2227
        [XmlElement(ElementName = "option")]
        public List<Option> Options { get; set; } = new List<Option>();

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
#pragma warning restore CA2227
    }
}
