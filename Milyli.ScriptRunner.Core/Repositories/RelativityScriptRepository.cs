namespace Milyli.ScriptRunner.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Relativity.API;
    using kCura.Relativity.Client;
    using Models;
    using Relativity.Client;
    using Tools;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityScriptRepository : RelativityClientRepository, IRelativityScriptRepository
    {
        public RelativityScriptRepository(IRelativityClientFactory relativityClientFactory)
            : base(relativityClientFactory)
        {
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
                        InputId = f.Id,
                        InputType = f.InputType.ToString(),
                        IsRequired = f.IsRequired
                    }).ToList();
                }, workspace);
        }

        public RelativityScript GetRelativityScript(RelativityWorkspace workspace, int scriptArtifactId)
        {
            return this.InWorkspace(
                (client, ws) =>
                this.GetScript(client, scriptArtifactId, ws), workspace);
        }

        private T InWorkspace<T>(Func<IRSAPIClient, RelativityWorkspace, T> action, RelativityWorkspace workspace)
        {
            return RelativityHelper.InWorkspace(action, workspace, this.RelativityClient);
        }

        private List<RelativityScript> GetAllScripts(IRSAPIClient client, RelativityWorkspace workspace)
        {
            var scriptArtifactResults = this.RelativityClient.Repositories.RelativityScript.Query(new DTOs.Query<DTOs.RelativityScript>()
            {
                Fields = RelativityHelper.FieldList("Name", "Description")
            });
            if (scriptArtifactResults.Success)
            {
                return scriptArtifactResults.Results.Select(r => new RelativityScript()
                {
                    Name = r.Artifact.Name,
                    Descirption = r.Artifact.Description,
                    WorkspaceId = workspace.WorkspaceId,
                    RelativityScriptId = r.Artifact.ArtifactID
                }).ToList();
            }

            return new List<RelativityScript>();
        }

        private RelativityScript GetScript(IRSAPIClient client, int scriptArtifactId, RelativityWorkspace workspace)
        {
            var scriptArtifact = this.RelativityClient.Repositories.RelativityScript.ReadSingle(scriptArtifactId);
            return new RelativityScript()
            {
                Name = scriptArtifact.Name,
                Descirption = scriptArtifact.Description,
                RelativityScriptId = scriptArtifact.ArtifactID,
                WorkspaceId = workspace.WorkspaceId
            };
        }
    }
}
