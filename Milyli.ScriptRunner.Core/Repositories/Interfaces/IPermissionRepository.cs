namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        bool IsUserAdmin(int userId);
    }
}