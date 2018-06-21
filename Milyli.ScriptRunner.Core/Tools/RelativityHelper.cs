namespace Milyli.ScriptRunner.Core.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
	using System.Text;
	using kCura.Relativity.Client;
	using kCura.Relativity.Client.DTOs;
	using Milyli.ScriptRunner.Core.Models;
    using DTOs = kCura.Relativity.Client.DTOs;

    public static class RelativityHelper
    {
        public static T InWorkspace<T>(Func<IRSAPIClient, RelativityWorkspace, T> workspaceAction, RelativityWorkspace workspace, IRSAPIClient relativityClient)
        {
            int currentWorkspaceId = relativityClient.APIOptions.WorkspaceID;
            try
            {
                relativityClient.APIOptions.WorkspaceID = workspace.WorkspaceId;
                var result = workspaceAction(relativityClient, workspace);
                return result;
            }
            finally
            {
                relativityClient.APIOptions.WorkspaceID = currentWorkspaceId;
            }
        }

        public static void InWorkspace(Action<IRSAPIClient, RelativityWorkspace> workspaceAction, RelativityWorkspace workspace, IRSAPIClient relativityClient)
        {
            int currentWorkspaceId = relativityClient.APIOptions.WorkspaceID;
            try
            {
                relativityClient.APIOptions.WorkspaceID = workspace.WorkspaceId;
                workspaceAction(relativityClient, workspace);
            }
            finally
            {
                relativityClient.APIOptions.WorkspaceID = currentWorkspaceId;
            }
        }

        public static List<DTOs.FieldValue> FieldList(params string[] fieldNames)
        {
            return fieldNames.Select(name => new DTOs.FieldValue()
            {
                Name = name
            }).ToList();
        }

		public static List<T> GetResults<T>(this ResultSet<T> resultSet)
			where T : DTOs.Artifact
		{
			var failureItems = resultSet.Results.Where(x => !x.Success);
			if (resultSet.Success && !failureItems.Any())
			{
				return resultSet.Results.Select(x => x.Artifact).ToList();
			}

			var stringBuilder = new StringBuilder(resultSet.Message ?? "One or more results failed.");
			foreach (var message in failureItems.Select(x => $"- {x.Message}"))
			{
				stringBuilder.AppendLine().Append(message);
			}

			throw new InvalidOperationException(stringBuilder.ToString());
		}
    }
}
