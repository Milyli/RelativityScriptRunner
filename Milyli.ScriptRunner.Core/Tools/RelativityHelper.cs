namespace Milyli.ScriptRunner.Core.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using kCura.Relativity.Client;
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
    }
}
