USE [edds]
GO

IF OBJECT_ID('JobSchedule', 'u') IS NULL
BEGIN
	CREATE TABLE JobSchedule (
		JobScheduleId int not null IDENTITY(1,1),
		RelativityScriptId UNIQUEIDENTIFIER NOT NULL,
		LastExecutionTime DATETIME NULL,
		NextExecutionTime DATETIME NULL,
		JobStatus INT not null,
		JobEnabled BIT NOT NULL,
		MaximumRuntime int NOT NULL,
		ExecutionTime INT NOT NULL,
		ExecutionSchedule INT NOT NULL,
		CONSTRAINT PK_JobSchedule_JobScheduleId PRIMARY KEY CLUSTERED (JobScheduleId)
	)

	CREATE INDEX IX_JobSchedule_Script ON JobSchedule (RelativityScriptId)
	CREATE INDEX IX_JobSchedule_NextExecution ON JobSchedule (NextExecutionTime, JobStatus)
END

IF OBJECT_ID('JobHistory', 'u') IS NULL
BEGIN
	CREATE TABLE JobHistory
	(
		JobHistoryId int not null IDENTITY(1,1),
		JobScheduleId int not null,
		StartTime DATETIME not null,
		Runtime INT null,
		CONSTRAINT PK_JobHistory_JobHistoryId PRIMARY KEY CLUSTERED (JobHistoryId),
		CONSTRAINT FK_JobHistory_JobSchedule FOREIGN KEY (JobScheduleId) REFERENCES JobSchedule(JobScheduleId)
	)

	CREATE INDEX IX_JobHistory_ByScheduleStart ON JobHistory (JobScheduleId, StartTime DESC)
END