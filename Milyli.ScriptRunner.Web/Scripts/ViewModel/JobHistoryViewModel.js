(function () {


    // Job Schedule History //
    JobHistoryLoader = function(url, jobScheduleId)
    {
        this.LoadJobHistory = function (callback, page, pageSize)
        {
            var query = {};
            query.jobScheduleId = jobScheduleId;
            if(page)
            {
                query.page = page;
            }

            if(pageSize)
            {
                query.pageSize = pageSize;
            }

            $.ajax({
                type :'GET',
                url: url,
                dataType: 'json',
                contentType: "application/json",
                data: query
            })
        .done(callback)
        .fail(error);            
        };
    }
    
    JobHistoryViewModel = function(jobHistoryLoader, loadedCallback)
    {
        var self = this;
        var jobHistoryViewModel = {};
        jobHistoryLoader.LoadJobHistory(function(jobHistoryModel)
        {
            var jobHistoryOptions = {
                extend : {
                    "{root}.JobHistory[i]" : function (JobHistory)
                    {
                        JobHistory.StartTimeString = ko.computed(function()
                        {
                            return toTimeString(JobHistory.StartTime());
                        });
                    },
                    "{root}" : function(JobHistoryModel)
                    {

                        JobHistoryModel.CurrentStart = ko.computed(function()
                        {
                            return JobHistoryModel.PageNumber() * JobHistoryModel.PageSize();
                        });

                        JobHistoryModel.HasNextPage = ko.computed(function()
                        {
                            var startResultNum = JobHistoryModel.CurrentStart();
                            return (startResultNum + parseInt(JobHistoryModel.PageSize())) < JobHistoryModel.ResultCount();
                        });

                        JobHistoryModel.HasPreviousPage = ko.computed(function()
                        {
                            return JobHistoryModel.PageNumber() > 0;
                        });

                        JobHistoryModel.LastPage = ko.computed(function()
                        {
                            return Math.floor(JobHistoryModel.ResultCount() / JobHistoryModel.PageSize());
                        });

                        JobHistoryModel.FirstPage = ko.computed(function()
                        {
                            return 0;
                        });

                        JobHistoryModel.PageNumberText = ko.computed(function()
                        {
                            var lastPage = JobHistoryModel.LastPage() + 1;
                            var currentPage = JobHistoryModel.PageNumber() + 1;

                            if(lastPage == 1)
                            {
                                return "Page 1 of 1";
                            }
                            else
                            {
                                return "Page " + currentPage + " of " + lastPage;
                            }
                        });
                    }
                }
            }

            jobHistoryViewModel = ko.viewmodel.fromModel(jobHistoryModel, jobHistoryOptions);


            jobHistoryViewModel.Go = function(page, precondCallback)
            {
                var precond = typeof(precondCallback) === "function" ? precondCallback() : true;
                if(precond)
                {
                    jobHistoryLoader.LoadJobHistory(function(jobHistoryModel)
                    {
                        jobHistoryViewModel.updating = true;
                        ko.viewmodel.updateFromModel(jobHistoryViewModel, jobHistoryModel);
                        jobHistoryViewModel.updating = false;
                    }, page, jobHistoryViewModel.PageSize());
                }                
            };

            jobHistoryViewModel.SelectPageSize = function()
            {
                if(!jobHistoryViewModel.updating)
                {
                    var newValue = jobHistoryViewModel.PageSize();
                    var newPage = Math.floor(jobHistoryViewModel.CurrentStart() / newValue);
                    jobHistoryViewModel.Go(newPage);
                }
            };

            jobHistoryViewModel.PageSize.subscribe(jobHistoryViewModel.SelectPageSize);

            jobHistoryViewModel.GoNextPage = function()
            {
                jobHistoryViewModel.Go(jobHistoryViewModel.PageNumber() + 1, jobHistoryViewModel.HasNextPage);
            };

            jobHistoryViewModel.GoPreviousPage = function()
            {
                jobHistoryViewModel.Go(jobHistoryViewModel.PageNumber() - 1, jobHistoryViewModel.HasPreviousPage);
            };

            jobHistoryViewModel.GoFirstPage = function()
            {
                jobHistoryViewModel.Go(0);
            };

            jobHistoryViewModel.GoLastPage = function()
            {
                jobHistoryViewModel.Go(jobHistoryViewModel.LastPage());
            };
                        
            loadedCallback(jobHistoryViewModel)
            jobHistoryViewModel.updating = false;
        });   
    }

        
})();