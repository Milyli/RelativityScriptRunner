namespace Milyli.ScriptRunner.Core.Repositories
{
    public interface IPermissionRepository
    {
        bool IsUserAdmin(int userId);
    }
}