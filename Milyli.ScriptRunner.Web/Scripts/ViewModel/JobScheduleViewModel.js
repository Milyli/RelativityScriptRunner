(function () {
    JobScheduleViewModel = function (model, actions) {
        var self = this;
        var scriptOptions = ko.observable();
        var options = {
            extend: {
                "{root}.JobScriptInputs[i]": function (JobScriptInput) {
                    JobScriptInput.Options = ko.computed(function () {
                        var options = scriptOptions() ? scriptOptions()[JobScriptInput.InputId()] : null;
                        return options;
                    });
                    JobScriptInput.SelectedValue = ko.observable();

                    JobScriptInput.HasOptions = ko.computed(function () {
                        return JobScriptInput.Options() && JobScriptInput.Options().length > 0;
                    });

                    JobScriptInput.IsInvalid = ko.computed(function () {
                        var currentValue = JobScriptInput.InputValue();
                        return JobScriptInput.IsRequired() && !(currentValue && currentValue.trim && currentValue.trim());
                    });
                },

                "{root}.JobSchedule": function (JobSchedule) {
                    JobSchedule.NextExecutionTimeString = ko.computed(function () {
                        return toTimeString(JobSchedule.NextExecutionTime());
                    });

                    JobSchedule.LastExecutionTimeString = ko.computed(function () {
                        return toTimeString(JobSchedule.LastExecutionTime());
                    });

                    (function () {
                        var time = JobSchedule.ExecutionTime();
                        var minutes = Math.floor(time / 60);
                        var hours = Math.floor(minutes / 60);
                        var meridian = hours >= 12 ? "PM" : "AM";
                        hours = hours == 0 ? 12 : hours > 12 ? hours - 12 : hours;
                        var minuteValue = minutes % 60;

                        JobSchedule.ExecutionTimeHours = ko.observable(hours);
                        JobSchedule.ExecutionTimeMinutes = ko.observable(minuteValue < 10 ? "0" + minuteValue : minuteValue);
                        JobSchedule.ExecutionTimeMeridian = ko.observable(meridian);
                    })();

                    var setTime = function (inputHours, inputMinutes, meridian) {
                        var hours = parseInt(inputHours);
                        if (meridian === "PM" && hours !== 12) {
                            hours += 12;
                        }
                        if (meridian === "AM" && hours === 12) {
                            hours = 0;
                        }
                        var minutes = parseInt(inputMinutes);
                        var timeSeconds = ((hours * 60) + minutes) * 60;
                        JobSchedule.ExecutionTime(timeSeconds);
                    }

                    JobSchedule.ExecutionTimeHours.subscribe(function (newValue) {
                        setTime(newValue, JobSchedule.ExecutionTimeMinutes(), JobSchedule.ExecutionTimeMeridian());
                    });

                    JobSchedule.ExecutionTimeMinutes.subscribe(function (newValue) {
                        setTime(JobSchedule.ExecutionTimeHours(), newValue, JobSchedule.ExecutionTimeMeridian());
                    });
                    JobSchedule.ExecutionTimeMeridian.subscribe(function (newValue) {
                        setTime(JobSchedule.ExecutionTimeHours(), JobSchedule.ExecutionTimeMinutes(), newValue);
                    });
                },

                "{root}": function (JobScheduleModel) {
                    JobScheduleModel.AllowRun = ko.computed(function () {
                        return JobScheduleModel.JobSchedule.JobEnabled() && !JobScheduleModel.IsNew()
                    });

                    JobScheduleModel.EnableDisableText = ko.computed(function () {
                        if (JobScheduleModel.JobSchedule.JobEnabled()) {
                            return "Disable";
                        }
                        return "Enabled";
                    })

                    JobScheduleModel.JobStatusName = ko.computed(function () {
                        if (JobScheduleModel.IsNew()) {
                            return "New";
                        }

                        if (!JobScheduleModel.JobSchedule.JobEnabled()) {
                            return "Disabled";
                        }

                        var status = JobScheduleModel.JobSchedule.JobStatus();
                        if (JobStatus[status]) {
                            return JobStatus[status];
                        }
                        return "Unknown";
                    });

                    JobScheduleModel.InvalidInputs = ko.computed(function () {
                        var invalidInputs = JobScheduleModel.JobScriptInputs()
                        .filter(function (jobScriptInput) {
                            return jobScriptInput.IsInvalid();
                        })
                        .map(function (jobScriptInput) {
                            return jobScriptInput.InputName();
                        });
                        return invalidInputs;
                    });

										JobScheduleModel.TimeoutInvalid = ko.computed(function () {
											return JobScheduleModel.JobSchedule.DirectSql() ? JobScheduleModel.JobSchedule.MaximumRuntime() < 30 : false;
										});

										JobScheduleModel.InputsInvalid = ko.computed(function () {
											return JobScheduleModel.InvalidInputs().length > 0;
										});

                    JobScheduleModel.IsInvalid = ko.computed(function () {
											return JobScheduleModel.InputsInvalid() || JobScheduleModel.TimeoutInvalid();
                    });

                    JobScheduleModel.InvalidText = ko.computed(function () {
                        if (JobScheduleModel.InputsInvalid()) {
                            return "Inputs are required by invalid: " + JobScheduleModel.InvalidInputs().join(", ");
												}
												else if (JobScheduleModel.TimeoutInvalid()) {
													return "Minimum timeout is 30 seconds";
												}
                        else {
                            return "";
                        }
                    });

                    JobScheduleModel.toggle = function (day) {
                        var currentValue = JobScheduleModel[day]();
                        JobScheduleModel[day](!currentValue);
                    };

                    //Initialization
                    (function () {
                        var schedule = JobScheduleModel.JobSchedule.ExecutionSchedule();
                        $.each(days, function (idx, day) {
                          var dayEnabled = (schedule & (1 << idx)) > 0;
                            JobScheduleModel[day] = ko.observable(dayEnabled);
                            JobScheduleModel[day].subscribe(function (newValue) {
                                var oldSchedule = JobScheduleModel.JobSchedule.ExecutionSchedule();
                                var mask = newValue ? 1 << idx : ~(1 << idx);
                                var newSchedule = newValue ? oldSchedule | mask : oldSchedule & mask;
                                JobScheduleModel.JobSchedule.ExecutionSchedule(newSchedule);
                            });
                        });
                    })();
                }
            }
        };
        var viewmodel = ko.viewmodel.fromModel(model, options);
        
        function internalSaveJobSchedule(postSaveAction) {
          mask();
          $.ajax({
                type: 'POST',
                url: actions.Save,
                processData: false,
                dataType: 'json',
                contentType: "application/json",
                data: ko.toJSON(ko.viewmodel.toModel(viewmodel))
            })
                        .done(function (data) {
                            if (viewmodel.IsNew()) {
                                window.location.href = actions.Index + "?jobScheduleId=" + data.JobSchedule.Id;
                            }
                            else {
                                ko.viewmodel.updateFromModel(viewmodel, data);
                                if (!!postSaveAction) {
                                    postSaveAction(viewmodel);
                                }
                            }
                        })
                        .fail(error)
                        .always(function (data) {
                            unmask();
                        });
        };

        viewmodel.SaveJobSchedule = function () {
            internalSaveJobSchedule();
        };

        viewmodel.EnableDisableJob = function () {
            viewmodel.JobSchedule.JobEnabled(!ko.unwrap(viewmodel.JobSchedule.JobEnabled));
            self.SaveJobSchedule();
        };

        viewmodel.RunJob = function () {
            internalSaveJobSchedule(function (jobScheduleModel) {
                $.ajax({
                    type: 'POST',
                    url: actions.Run,
                    processData: false,
                    dataType: 'json',
                    contentType: "application/json",
                    data: ko.toJSON(jobScheduleModel.JobSchedule)
                })
                    .done(function (data) {
                        ko.viewmodel.updateFromModel(viewmodel, data)
                    })
                    .fail(error);
            });
        };

        self.UpdateOptions = function (selectableInputValues) {
            scriptOptions(selectableInputValues);
        };

        self.IsNew = function () {
            return viewmodel.IsNew();
        };

        self.GetViewModel = function()
        {
            viewmodel.HasName = ko.computed(function () {
                return viewmodel.JobSchedule.Name().length > 0;
            })

            return viewmodel;
        };
    }
})();