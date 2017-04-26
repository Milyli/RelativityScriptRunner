namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RDOCollectionAttribute : RelativityDynamicObjectAttribute
    {
        public RDOCollectionAttribute(string guid)
            : base(guid)
        {
        }
    }
}
