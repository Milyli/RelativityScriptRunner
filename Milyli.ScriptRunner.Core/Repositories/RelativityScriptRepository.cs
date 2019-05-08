namespace Milyli.ScriptRunner.Core.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Relativity.API;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Repositories.Interfaces;
	using Models;
	using Relativity.Client;
	using Tools;
	using DTOs = kCura.Relativity.Client.DTOs;

    public class RelativityScriptRepository : RelativityClientRepository, IRelativityScriptRepository
    {
			private readonly IHelper relativityHelper;

				public RelativityScriptRepository(IRelativityClientFactory relativityClientFactory, IHelper relativityHelper)
            : base(relativityClientFactory)
        {
					this.relativityHelper = relativityHelper ?? throw new ArgumentNullException(nameof(relativityHelper));
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
                    var scriptArtifact = new DTOs.RelativityScript(script.RelativityScriptId);
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
                this.GetScript(scriptArtifactId, ws), workspace);
        }

		public ExecuteResult ExecuteRelativityScript(int scriptArtifactId, List<JobScriptInput> inputs, RelativityWorkspace workspace)
		{
			var relativityInputs = inputs.Select(i => new RelativityScriptInput(i.InputName, i.InputValue)).ToList();
			return this.InWorkspace(
				(client, ws) =>
				{
					var rsapiResult = client.Repositories.RelativityScript.ExecuteRelativityScript(new DTOs.RelativityScript(scriptArtifactId), relativityInputs);
					return new ExecuteResult { Success = rsapiResult.Success, Message = rsapiResult.Message };
				}, workspace);
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
                    Description = r.Artifact.Description,
                    WorkspaceId = workspace.WorkspaceId,
                    RelativityScriptId = r.Artifact.ArtifactID
                }).ToList();
            }

            return new List<RelativityScript>();
        }

        private RelativityScript GetScript(int scriptArtifactId, RelativityWorkspace workspace)
        {
			DTOs.RelativityScript scriptArtifact;

			// try/catch block needed for R96; see https://devhelp.relativity.com/t/4525/17
			try
			{
				scriptArtifact = this.RelativityClient.Repositories.RelativityScript.ReadSingle(scriptArtifactId);
			}
			catch (APIException ex) when (ex.Message == "Unable to cast object of type 'System.Int32' to type 'System.String'.")
			{
				scriptArtifact = this.RelativityClient.Repositories.RelativityScript.Query(
					new DTOs.Query<DTOs.RelativityScript>
					{
						Condition = new TextCondition(
							"Artifact ID",
							TextConditionEnum.In,
							new[] { scriptArtifactId.ToString() }),
						Fields = DTOs.FieldValue.AllFields
					}).GetResults().Single();
			}

            return new RelativityScript()
            {
				RelativityScriptId = scriptArtifact.ArtifactID,
                Name = scriptArtifact.Name,
                Description = scriptArtifact.Description,
                WorkspaceId = workspace.WorkspaceId
            };
        }
	}
}
