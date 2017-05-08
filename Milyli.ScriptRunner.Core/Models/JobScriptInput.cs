namespace Milyli.ScriptRunner.Core.Models
{
    using LinqToDB.Mapping;

    [Table(Name = "JobScriptInput")]
    public class JobScriptInput
    {
        [Column("JobScriptInputId")]
        public int Id { get; set; }

        [Column("JobScheduleId")]
        public int JobScheduleId { get; set; }

        [Column("InputId")]
        public string InputId { get; set; }

        [Column(Name = "InputName")]
        public string InputName { get; set; }

        [Column(Name = "InputValue")]
        public string InputValue { get; set; }
    }
}
