
using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Models
{
    using System;
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
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [Column("Runtime")]
        public int? Runtime { get; set; }

        [Column("Errored")]
        public bool HasError { get; set; }

        [Column("ResultText")]
        public string ResultText { get; set; }

        internal void UpdateRuntime()
        {
            this.Runtime = (int)DateTime.UtcNow.Subtract(this.StartTime).TotalSeconds;
        }
    }
}
