namespace Milyli.ScriptRunner.Core.Services
{
    using System;
    using global::Relativity.Services.Permission;

	public interface IPermissionsService : IDisposable
	{
        IPermissionManager PermissionManager { get; }

        bool CanEdit(int workspaceId, int artifactTypeId);
	}
}
