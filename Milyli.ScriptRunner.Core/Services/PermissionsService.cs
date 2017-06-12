namespace Milyli.ScriptRunner.Core.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Relativity.API;
	using global::Relativity.Services;
	using global::Relativity.Services.Permission;
	using Milyli.ScriptRunner.Core.DependencyResolution;

	public class PermissionsService : IDisposable
	{
		private IPermissionManager permissionManager;
		private bool disposedValue = false;

		public PermissionsService(IServiceManagerFactory serviceManagerFactory)
		{
			this.permissionManager = serviceManagerFactory.GetServiceProxy<IPermissionManager>(ExecutionIdentity.CurrentUser);
		}

		public bool CanEdit(int workspaceId, int artifactTypeId)
		{
			var query = new Query();
			var objectTypeCondition = new ObjectCondition(PermissionFieldNames.ArtifactType, ObjectConditionEnum.EqualTo, artifactTypeId);
			var editPermissionTypeCondition = new TextCondition(PermissionFieldNames.PermissionType, TextConditionEnum.EqualTo, PermissionType.Edit.Name);
			query.Condition = new CompositeCondition(objectTypeCondition, CompositeConditionEnum.And, editPermissionTypeCondition).ToQueryString();
			var permissions = this.permissionManager.QueryAsync(workspaceId, query, 1).Result;
			if (permissions.Success)
			{
				var permission = permissions.Results.FirstOrDefault().Artifact;
				var result = this.permissionManager.GetPermissionSelectedAsync(workspaceId, new List<PermissionRef>() { permission }).Result;
				return result.FirstOrDefault()?.Selected ?? false;
			}
			else
			{
				//TODO log errors
			}
			return false;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.permissionManager.Dispose();
				}

				this.disposedValue = true;
			}
		}
	}
}
