namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;

    public class DuplicateFieldException : RDOMappingException
    {
        public DuplicateFieldException(Type type, string fieldName, Guid fieldGuid)
            : base(type)
        {
            this.message = string.Format("Field {0} ({1}) already exists", fieldName, fieldGuid);
            this.FieldName = fieldName;
            this.FieldGuid = fieldGuid;
        }

        public string FieldName { get; private set; }

        public Guid FieldGuid { get; private set; }
    }
}
