namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RDOFieldAttribute : RelativityDynamicObjectAttribute
    {
        public RDOFieldAttribute(string guid)
            : base(guid)
        {
        }
    }
}
