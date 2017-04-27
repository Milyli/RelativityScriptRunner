namespace Milyli.ScriptRunner.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using kCura.Relativity.Client;
    using Models;
    using Tools;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityScriptRepository : IRelativityScriptRepository
    {
        private IRSAPIClient relativityClient;

        public RelativityScriptRepository(IRSAPIClient relativityClient)
        {
            this.relativityClient = relativityClient;
        }

        public List<RelativityScript> GetRelativityScripts(RelativityWorkspace workspace)
        {
            var workspaceScripts = this.InWorkspace<List<RelativityScript>>(this.GetAllScripts, workspace);
            return workspaceScripts;
        }

        public List<Models.ScriptInput> GetScriptInputs(RelativityScript script, RelativityWorkspace workspace)
        {
            return this.InWorkspace(
                (client, ws) =>
                {
                    var scriptArtifact = client.Repositories.RelativityScript.ReadSingle(script.RelativityScriptId);
                    var fields = client.Repositories.RelativityScript.GetRelativityScriptInputs(scriptArtifact);
                    return fields.Select(f => new Models.ScriptInput()
                    {
                        Name = f.Name,
                        InputType = f.InputType.ToString(),
                        IsRequired = f.IsRequired
                    }).ToList();
                }, workspace);
        }

        private T InWorkspace<T>(Func<IRSAPIClient, RelativityWorkspace, T> action, RelativityWorkspace workspace)
        {
            return RelativityHelper.InWorkspace(action, workspace, this.relativityClient);
        }

        private List<DTOs.FieldValue> FieldList(params string[] fieldNames)
        {
            return fieldNames.Select(name => new DTOs.FieldValue()
            {
                Name = name
            }).ToList();
        }

        private List<RelativityScript> GetAllScripts(IRSAPIClient client, RelativityWorkspace workspace)
        {
            var scriptArtifactResults = this.relativityClient.Repositories.RelativityScript.Query(new DTOs.Query<DTOs.RelativityScript>()
            {
                Fields = this.FieldList("Name", "Description")
            });
            if (scriptArtifactResults.Success)
            {
                return scriptArtifactResults.Results.Select(r => new RelativityScript()
                {
                    Name = r.Artifact["Name"]?.Value.ToString(),
                    Descirption = r.Artifact["Description"]?.Value.ToString(),
                    WorkspaceId = workspace.WorkspaceId,
                    RelativityScriptId = r.Artifact.ArtifactID
                }).ToList();
            }

            return new List<RelativityScript>();
        }
    }
}
