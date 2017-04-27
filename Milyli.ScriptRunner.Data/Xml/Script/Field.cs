namespace Milyli.ScriptRunner.Data.Xml.Script
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class Field
    {
        [XmlElement(ElementName="filter")]
        public Filters Filters { get; set; }
    }
}
