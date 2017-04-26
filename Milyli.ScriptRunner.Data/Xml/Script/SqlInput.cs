namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System.Xml.Serialization;

    public class SqlInput : AInput
    {
        [XmlText]
        public string SqlText { get; set; }
    }
}
