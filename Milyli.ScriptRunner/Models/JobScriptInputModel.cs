namespace Milyli.ScriptRunner.Models
{
    using Milyli.ScriptRunner.Core.Models;

    public class JobScriptInputModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobScriptInputModel"/> class.
        /// parameterless constructor for deserialization
        /// </summary>
        public JobScriptInputModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobScriptInputModel"/> class from a <see cref="ScriptInput"/> instance.
        /// </summary>
        /// <param name="relativityScriptInput">the relativity ScriptInput object</param>
        public JobScriptInputModel(ScriptInput relativityScriptInput)
        {
            this.InputId = relativityScriptInput.InputId;
            this.InputName = relativityScriptInput.Name;
            this.InputType = relativityScriptInput.InputType;
            this.IsRequired = relativityScriptInput.IsRequired;
        }

        public int? Id { get; set; }

        public int? JobScheduleId { get; set; }

        public string InputId { get; set; }

        public string InputName { get; set; }

        public string InputValue { get; set; }

        public string InputType { get; set; }

        public bool IsRequired { get; set; }
    }
}