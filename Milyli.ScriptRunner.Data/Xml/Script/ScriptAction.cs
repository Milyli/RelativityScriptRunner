namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System.Xml.Serialization;

    public class ScriptAction
    {
        [XmlAttribute(AttributeName = "returns")]
        public string Returns { get; set; }

        [XmlAttribute(AttributeName = "allowhtmltagsinoutput")]
        public bool AllowHtmlTagsInOutput { get; set; }

        [XmlAttribute(AttributeName = "timeout")]
        public int Timeout { get; set; }

        [XmlText]
        public string ScriptSource { get; set; }
    }
}
