IF OBJECT_ID('JobSchedule', 'u') IS NULL
BEGIN
	CREATE TABLE JobSchedule (
		JobScheduleId int not null IDENTITY(1,1),
		RelativityScriptId INT NOT NULL,
		WorkspaceId INT NOT NULL,
		JobName nvarchar(255) NULL,
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