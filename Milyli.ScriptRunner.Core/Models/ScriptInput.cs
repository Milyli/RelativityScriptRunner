namespace Milyli.ScriptRunner.Core.Models
{
    public class ScriptInput
    {
        public string Name { get; set; }

        public string InputId { get; internal set; }

        public string InputType { get; set; }

        public bool IsRequired { get; set; }
    }
}
