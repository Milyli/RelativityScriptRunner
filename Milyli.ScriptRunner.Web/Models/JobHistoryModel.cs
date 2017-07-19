namespace Milyli.ScriptRunner.Web.Models
{
    using System.Collections.Generic;
    using Core.Models;

    public class JobHistoryModel
    {
        public JobHistoryModel(List<JobHistory> jobHistory, int resultCount, int pageNumber, int pageSize)
        {
            this.JobHistory = jobHistory;
            this.ResultCount = resultCount;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public List<JobHistory> JobHistory { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int ResultCount { get; }
    }
}