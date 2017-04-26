namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System.Xml.Serialization;

    public class Option
    {
        // Best I can tell, the xml option list is inverse of Html's Select OPTION tag.
        // In this class, OptionValue is the machine-readable value, whereas the Value (read from the
        // xml attribute) is the human-readable value
        [XmlText]
        public string OptionValue { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
