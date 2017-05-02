using Milyli.ScriptRunner.Core.Models;

namespace Milyli.ScriptRunner.Models
{
    public class JobScriptInputModel
    {
        public int? Id { get; set; }

        public int? JobScheduleId { get; set; }

        public string InputName { get; set; }

        public string InputValue { get; set; }

        public string InputType { get; set; }

        public bool IsRequired { get; set; }
    }
}