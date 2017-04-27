namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;
    using System.Text;

    [Serializable]
    public class RDOMappingException : Exception
    {
        public RDOMappingException(Type type)
        {
            this.MappedType = type;
        }

        public RDOMappingException(Type type, string message)
            : this(type)
        {
            this.message = message;
        }

#pragma warning disable SA1401 // Fields must be private
#pragma warning disable SA1201 // Elements must appear in the correct order
        protected string message;

#pragma warning restore SA1201 // Elements must appear in the correct order
#pragma warning restore SA1401 // Fields must be private
        public Type MappedType { get; protected set; }

        public override string Message
        {
            get
            {
                return this.message ?? base.Message;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("Mapping Exception in {0} : {1}", this.MappedType.Name, this.Message));
            stringBuilder.Append(base.ToString());
            return stringBuilder.ToString();
        }
    }
}
