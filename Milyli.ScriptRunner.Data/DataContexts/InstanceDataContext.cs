namespace Milyli.ScriptRunner.Data.DataContexts
{
    using System;
    using LinqToDB;
    using Milyli.Framework.Repositories.Interfaces;
    using Milyli.ScriptRunner.Data.Models;
    using IRepositories = Milyli.Framework.Repositories.Interfaces;
    using MRepositories = Milyli.Framework.Repositories;

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
    }
}
