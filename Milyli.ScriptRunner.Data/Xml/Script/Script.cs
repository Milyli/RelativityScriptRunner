namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "", ElementName = "script")]
    public class Script
    {
        [XmlElement(ElementName = "input")]
        public Input Input { get; set; }

        [XmlElement(ElementName = "action")]
        public ScriptAction Action { get; set; }
    }
}
