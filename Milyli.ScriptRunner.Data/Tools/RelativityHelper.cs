namespace Milyli.ScriptRunner.Data.Tools
{
    using System;
    using System.Threading;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Data.Models;

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
    }
}
