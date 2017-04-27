namespace Milyli.ScriptRunner.Data.Models
{
    using System;
    using Framework.Repositories.Interfaces;
    using LinqToDB.Mapping;

    [Table("JobHistory")]
    public class JobHistory : IModel<int>
    {
        [PrimaryKey]
        [Identity]
        [Column("JobHistoryId")]
        public int Id { get; set; }

        [Column("JobScheduleId")]
        public int JobScheduleId { get; set; }

        [Column("StartTime")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Column("Runtime")]
        public int? Runtime { get; set; }

        [Column("Errored")]
        public bool Errored { get; set; }

        [Column("ResultText")]
        public string ResultText { get; set; }

        internal void UpdateRuntime()
        {
            this.Runtime = DateTime.Now.Subtract(this.StartTime).Seconds;
        }
    }
}
