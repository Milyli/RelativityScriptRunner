namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System.Xml.Serialization;

    public abstract class AInput
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
