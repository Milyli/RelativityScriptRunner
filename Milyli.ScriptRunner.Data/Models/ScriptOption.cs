namespace Milyli.ScriptRunner.Data.Models
{
    public class ScriptOption
    {
        public ScriptOption()
        {
        }

        public ScriptOption(Xml.Script.Option option)
        {
            this.Name = option.Value;
            this.Value = option.OptionValue;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
