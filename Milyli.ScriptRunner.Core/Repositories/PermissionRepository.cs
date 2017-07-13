using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private DataContext dataContext;

        public PermissionRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool IsUserAdmin(int activeUserId)
        {
            using (var com = this.dataContext.CreateCommand())
            {
                com.CommandText = $@"select top 1 GroupArtifactID from [edds].[eddsdbo].[GroupUser] where GroupArtifactID = 20 and UserArtifactID = {activeUserId}";
                var isAdmin = com.ExecuteScalar();
                if (isAdmin == null)
                {
                    return false;
                }

                return !string.IsNullOrEmpty(isAdmin.ToString());
            }
        }
    }
}
