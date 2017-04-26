namespace Milyli.ScriptRunner.Data.Repositories
{
    using Milyli.ScriptRunner.Data.DataContexts;
    using Milyli.ScriptRunner.Data.Models;
    using Repository = Milyli.Framework.Repositories;

    public class JobScheduleWorkspaceRepository : Repository.BaseReadWriteRepository<InstanceDataContext, JobScheduleWorkspace, int>
    {
        private IRelativityWorkspaceRepository relativityWorkspaceRepository;

        public JobScheduleWorkspaceRepository(InstanceDataContext dataConext, IRelativityWorkspaceRepository relativityWorkspaceRepository)
            : base(dataConext)
        {
            this.relativityWorkspaceRepository = relativityWorkspaceRepository;
        }
    }
}
