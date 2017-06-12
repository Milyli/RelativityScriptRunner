namespace Milyli.ScriptRunner.Core.Services
{
	public interface IPermissionsService
	{
		bool CanEdit(int workspaceId, int artifactTypeId);
	}
}
