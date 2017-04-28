namespace Milyli.ScriptRunner.Controllers
{
    using System.Web.Mvc;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Models;
    using System.Linq;
    using System.Collections.Generic;

    public class JobScheduleController : ScriptRunnerController
    {
        public JobScheduleController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository) : base(jobScheduleRepository, scriptRepository, workspaceRepository)
        {
        }

        //
        // GET: /JobSchedule/

        public ActionResult Index(int jobScheduleId)
        {
            var schedule = this.jobScheduleRepository.Read(jobScheduleId);
            var scriptInputs = this.jobScheduleRepository.GetJobInputs(schedule);
            var jobScheduleModel = new JobScheduleModel()
            {
                JobSchedule = schedule
            };

            if (schedule != null)
            {
                this.PopulateJobScheduleModel(jobScheduleModel, schedule.WorkspaceId, schedule.RelativityScriptId);
                this.MergeScriptInputs(jobScheduleModel);
            }

            return this.View(jobScheduleModel);
        }

        public ActionResult NewSchedule(int workspaceId, int scriptId)
        {
            var jobScheduleModel = new JobScheduleModel();
            this.PopulateJobScheduleModel(jobScheduleModel, workspaceId, scriptId);
            if (jobScheduleModel.RelativityScript != null)
            {
                var inputs = this.scriptRepository.GetScriptInputs(jobScheduleModel.RelativityScript, jobScheduleModel.RelativityWorkspace);
                jobScheduleModel.JobScriptInputs = inputs.Select(i => new JobScriptInputModel()
                {
                    InputName = i.Name,
                    InputType = i.InputType,
                    IsRequired = i.IsRequired
                }).ToList();
            }

            jobScheduleModel.JobSchedule = new JobSchedule()
            {
                Name = $"{jobScheduleModel.RelativityWorkspace.WorkspaceName} - {jobScheduleModel.RelativityScript.Name}"
            };
            return this.View(jobScheduleModel);
        }

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

        private List<JobScriptInputModel> GetScriptInputs(RelativityScript relativityScript, RelativityWorkspace relativityWorkspace)
        {
            var inputs = this.scriptRepository.GetScriptInputs(relativityScript, relativityWorkspace);
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
            jobScheduleModel.RelativityScript = this.scriptRepository.GetRelativityScript(jobScheduleModel.RelativityWorkspace, scriptArtifactId);
        }
    }
}
