IF OBJECT_ID('JobHistory', 'u') IS NULL
BEGIN
	CREATE TABLE JobHistory
	(
		JobHistoryId int not null IDENTITY(1,1),
		JobScheduleId int not null,
		StartTime DATETIME not null,
		Runtime INT null,
		Errored BIT NOT NULL,
		ResultText NVARCHAR(max),
		CONSTRAINT PK_JobHistory_JobHistoryId PRIMARY KEY CLUSTERED (JobHistoryId),
		CONSTRAINT FK_JobHistory_JobSchedule FOREIGN KEY (JobScheduleId) REFERENCES JobSchedule(JobScheduleId)
	)

	CREATE INDEX IX_JobHistory_ByScheduleStart ON JobHistory (JobScheduleId, StartTime DESC)
END