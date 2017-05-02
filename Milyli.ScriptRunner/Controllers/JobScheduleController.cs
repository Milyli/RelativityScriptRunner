namespace Milyli.ScriptRunner.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_Start;
    using kCura.Relativity.Client;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Models;
    using Relativity.API;
    using Relativity.CustomPages;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class JobScheduleController : ScriptRunnerController
    {
        public JobScheduleController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository) 
            : base(jobScheduleRepository, scriptRepository, workspaceRepository)
        {
        }

        // GET: /JobSchedule/
        public ActionResult Index(int jobScheduleId)
        {
            var jobScheduleModel = this.GetJobScheduleModel(jobScheduleId);
            if (jobScheduleModel == null)
            {
                new HttpNotFoundResult($"could not find the job schedule with id {jobScheduleId}");
            }

            return this.View("EditSchedule", jobScheduleModel);
        }

        public ActionResult NewSchedule(int workspaceId, int relativityScriptId)
        {
            var workspace = this.workspaceRepository.Read(workspaceId);
            if (workspace != null)
            {
                var script = this.relativityScriptRepository.GetRelativityScript(workspace, relativityScriptId);
                if (script != null)
                {
                    return this.View("EditSchedule", this.NewJobScheduleModel(script, workspace));
                }
                else
                {
                    return this.HttpNotFound($"Could not find the script {relativityScriptId}");
                }
            }
            else
            {
                return this.HttpNotFound($"Could not find the workspace {workspaceId}");
            }
        }

        public JsonResult Save([ModelBinder(typeof(JsonBinder))]JobScheduleModel jobScheduleModel)
        {
            var jobSchedule = jobScheduleModel.JobSchedule;
            var scriptInputs = jobScheduleModel.JobScriptInputs;
            var id = this.jobScheduleRepository.SaveJobSchedule(jobSchedule, jobScheduleModel.ToJobScriptInputs());

            return this.Json(this.GetJobScheduleModel(id));
        }

        public ActionResult List(int workspaceId)
        {
            using (var client = ConnectionHelper.Helper().GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
            {
                var workspaces = client.Repositories.Workspace.Query(new DTOs.Query<DTOs.Workspace>()
                {
                    Fields = new List<DTOs.FieldValue>() { new DTOs.FieldValue("Name") }
                });
            }

            var relativityWorkspace = this.workspaceRepository.Read(workspaceId);
            if (relativityWorkspace == null)
            {
                return new HttpNotFoundResult($"Cannot find a workspace with id {workspaceId}");
            }

            var scriptListModel = new ScriptListModel()
            {
                RelativityWorkspace = relativityWorkspace,
                RelativityScripts = this.GetScriptList(relativityWorkspace).ToList()
            };

            return this.View(scriptListModel);
        }

        private IEnumerable<RelativityScriptModel> GetScriptList(RelativityWorkspace relativityWorkspace)
        {
            var jobSchedules = this.jobScheduleRepository.GetJobSchedules(relativityWorkspace).GroupBy(s => s.RelativityScriptId).ToDictionary(s => s.Key, s => s);
            var scripts = this.relativityScriptRepository.GetRelativityScripts(relativityWorkspace);
            foreach (var script in scripts)
            {
                if (jobSchedules.ContainsKey(script.RelativityScriptId))
                {
                    yield return new RelativityScriptModel(script, jobSchedules[script.RelativityScriptId]);
                }
                else
                {
                    yield return new RelativityScriptModel(script);
                }
            }
        }

        /// <summary>
        /// Takes the current job schedule model, merges the list of inputs used for deferred invocation with the list of inputs
        /// available in the Relativity script
        /// </summary>
        /// <param name="jobScheduleModel">The model of the current jobschedule operated on</param>
        private void MergeScriptInputs(JobScheduleModel jobScheduleModel)
        {
            var jobSchedule = jobScheduleModel.JobSchedule;
            var relativityScript = jobScheduleModel.RelativityScript;
            var relativityWorkspace = jobScheduleModel.RelativityWorkspace;
            var currentScriptInputs = this.GetScriptInputs(relativityScript, relativityWorkspace);
            var currentJobScriptInputs = this.jobScheduleRepository.GetJobInputs(jobSchedule).ToDictionary(i => i.InputName);
            foreach (var input in currentScriptInputs)
            {
                if (currentJobScriptInputs.ContainsKey(input.InputName))
                {
                    input.Id = currentJobScriptInputs[input.InputName].Id;
                }

                input.JobScheduleId = jobSchedule.Id;
            }

            jobScheduleModel.JobScriptInputs = currentScriptInputs;
        }

        /// <summary>
        /// Returns a list of script input models
        /// </summary>
        /// <param name="relativityScript">the reference to the relativity script</param>
        /// <param name="relativityWorkspace">the reference to the expected execution workspace</param>
        /// <returns>a list of script input models</returns>
        private List<JobScriptInputModel> GetScriptInputs(RelativityScript relativityScript, RelativityWorkspace relativityWorkspace)
        {
            var inputs = this.relativityScriptRepository.GetScriptInputs(relativityScript, relativityWorkspace);
            return inputs.Select(i => new JobScriptInputModel()
            {
                InputName = i.Name,
                InputType = i.InputType,
                IsRequired = i.IsRequired
            }).ToList();
        }

        private void PopulateJobScheduleModel(JobScheduleModel jobScheduleModel, int workspaceId, int scriptArtifactId)
        {
            jobScheduleModel.RelativityWorkspace = this.workspaceRepository.Read(workspaceId);
            jobScheduleModel.RelativityScript = this.relativityScriptRepository.GetRelativityScript(jobScheduleModel.RelativityWorkspace, scriptArtifactId);
        }

        private JobScheduleModel GetJobScheduleModel(int jobScheduleId)
        {
            var jobSchedule = this.jobScheduleRepository.Read(jobScheduleId);
            if (jobSchedule == null)
            {
                return null;
            }

            var scriptInputs = this.jobScheduleRepository.GetJobInputs(jobSchedule);
            var jobScheduleModel = new JobScheduleModel()
            {
                JobSchedule = jobSchedule
            };

            if (jobSchedule != null)
            {
                this.PopulateJobScheduleModel(jobScheduleModel, jobSchedule.WorkspaceId, jobSchedule.RelativityScriptId);
                this.MergeScriptInputs(jobScheduleModel);
            }

            return jobScheduleModel;
        }

        private JobScheduleModel NewJobScheduleModel(RelativityScript relativityScript, RelativityWorkspace relativityWorkspace)
        {
            var jobSchedule = new JobSchedule()
            {
                Name = $"{relativityWorkspace.Name} - {relativityScript.Name}",
                RelativityScriptId = relativityScript.RelativityScriptId,
                WorkspaceId = relativityWorkspace.WorkspaceId
            };

            var inputs = this.relativityScriptRepository
                .GetScriptInputs(relativityScript, relativityWorkspace)
                .Select(i => new JobScriptInputModel(i))
                .ToList();

            var jobScheduleModel = new JobScheduleModel()
            {
                JobSchedule = jobSchedule,
                RelativityWorkspace = relativityWorkspace,
                RelativityScript = relativityScript,
                JobScriptInputs = inputs
            };
            return jobScheduleModel;
        }
    }
}
