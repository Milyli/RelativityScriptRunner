namespace Milyli.ScriptRunner.Core.Services
{
	using System;

	public interface IPermissionsService : IDisposable
	{
		bool CanEdit(int workspaceId, int artifactTypeId);
	}
}
