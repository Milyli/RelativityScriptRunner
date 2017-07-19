
using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.DataContexts
{
    using LinqToDB;
    using Models;

    public class InstanceDataContext : Repositories.DataContext
    {
        public InstanceDataContext(IInstanceConnectionFactory factory)
            : base(factory)
        {
        }

        public ITable<JobSchedule> JobSchedule
        {
            get { return this.GetTable<JobSchedule>(); }
        }

        public ITable<JobHistory> JobHistory
        {
            get
            {
                return this.GetTable<JobHistory>();
            }
        }

        public ITable<JobScriptInput> JobScriptInput
        {
            get
            {
                return this.GetTable<JobScriptInput>();
            }
        }
    }
}
