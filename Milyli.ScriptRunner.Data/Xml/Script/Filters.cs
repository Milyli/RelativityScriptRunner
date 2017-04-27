namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class Filters
    {
        [XmlElement(ElementName = "category")]
        public int Catagory { get; set; }

        [XmlElement(ElementName = "type")]
        public int Type { get; set; }
    }
}
