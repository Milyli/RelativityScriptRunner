namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;

    public class RelativityDynamicObjectAttribute : Attribute
    {
        public RelativityDynamicObjectAttribute(string guid)
        {
            this.Guid = new Guid(guid);
        }

        public Guid Guid { get; private set; }

        public string Name { get; set; }
    }
}
