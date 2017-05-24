namespace Milyli.ScriptRunner.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_Start;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Web.Models;

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
                this.NotFound($"could not find the job schedule with id {jobScheduleId}");
            }

            return this.View("EditSchedule", jobScheduleModel);
        }

        public ActionResult NewSchedule(int workspaceId, int relativityScriptId)
        {
            var workspace = this.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                this.NotFound($"Could not find the workspace {workspaceId}");
            }

            var script = this.RelativityScriptRepository.GetRelativityScript(workspace, relativityScriptId);
            if (script == null)
            {
                this.NotFound($"Could not find the script {relativityScriptId}");
            }

            return this.View("EditSchedule", this.NewJobScheduleModel(script, workspace));
        }

        public ActionResult Save([ModelBinder(typeof(JsonBinder))]JobScheduleModel jobScheduleModel)
        {
            var jobSchedule = jobScheduleModel.JobSchedule;
            jobSchedule.NextExecutionTime = jobSchedule.GetNextExecution(DateTime.Now);
            var id = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobScheduleModel.ToJobScriptInputs());
            return JsonContent(this.GetJobScheduleModel(id));
        }

        public ActionResult Run([ModelBinder(typeof(JsonBinder))]JobSchedule jobSchedule)
        {
            this.JobScheduleRepository.ActivateJob(jobSchedule);
            return JsonContent(this.GetJobScheduleModel(jobSchedule.Id));
        }

        public ActionResult JobHistory(int jobScheduleId, int page = 0, int pageSize = 25)
        {
            var jobSchedule = this.JobScheduleRepository.Read(jobScheduleId);
            if (jobSchedule == null)
            {
                this.NotFound($"Could not find job with id ${jobScheduleId}");
            }

            int resultCount;
            var jobHistory = this.JobScheduleRepository.GetJobHistory(jobSchedule, out resultCount, page, pageSize);
            return JsonContent(new JobHistoryModel(jobHistory, resultCount, page, pageSize));
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
            var currentJobScriptInputs = this.JobScheduleRepository.GetJobInputs(jobSchedule).ToDictionary(i => i.InputId);
            foreach (var input in currentScriptInputs)
            {
                if (currentJobScriptInputs.ContainsKey(input.InputId))
                {
                    input.Id = currentJobScriptInputs[input.InputId].Id;
                    input.InputValue = currentJobScriptInputs[input.InputId].InputValue;
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
            var inputs = this.RelativityScriptRepository.GetScriptInputs(relativityScript, relativityWorkspace);
            return inputs.Select(i => new JobScriptInputModel()
            {
                InputId = i.InputId,
                InputName = i.Name,
                InputType = i.InputType,
                IsRequired = i.IsRequired
            }).ToList();
        }

        private void PopulateJobScheduleModel(JobScheduleModel jobScheduleModel, int workspaceId, int scriptArtifactId)
        {
            jobScheduleModel.RelativityWorkspace = this.GetWorkspace(workspaceId);
            jobScheduleModel.RelativityScript = this.RelativityScriptRepository.GetRelativityScript(jobScheduleModel.RelativityWorkspace, scriptArtifactId);
        }

        private JobScheduleModel GetJobScheduleModel(int jobScheduleId)
        {
            var jobSchedule = this.JobScheduleRepository.Read(jobScheduleId);
            if (jobSchedule == null)
            {
                return null;
            }

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

            var inputs = this.RelativityScriptRepository
                .GetScriptInputs(relativityScript, relativityWorkspace)
                .Select(i => new JobScriptInputModel(i))
                .ToList();

            var jobScheduleModel = new JobScheduleModel()
            {
                JobSchedule = jobSchedule,
                RelativityWorkspace = relativityWorkspace,
                RelativityScript = relativityScript,
                JobScriptInputs = inputs,
                IsNew = true
            };
            return jobScheduleModel;
        }
    }
}
