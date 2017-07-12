
namespace Milyli.ScriptRunner.Core.DataContexts
{
    using LinqToDB;
    using Models;
    using MilyliDependencies.Framework.Repositories.Interfaces;
    using MRepositories = MilyliDependencies.Framework.Repositories;

    public class InstanceDataContext : MRepositories.DataContext
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
