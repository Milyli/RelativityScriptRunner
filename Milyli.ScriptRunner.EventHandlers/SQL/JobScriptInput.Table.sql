IF OBJECT_ID('JobScriptInput', 'u') IS NULL
BEGIN
	CREATE TABLE JobScriptInput
	(
		JobScriptInputId int not null IDENTITY(1,1),
		JobScheduleId int not null,
		InputId nvarchar(255) not null,
		InputName nvarchar(255) not null,
		InputValue nvarchar(max),
		CONSTRAINT PK_JobScriptInput_JobScriptInputId PRIMARY KEY CLUSTERED (JobScriptInputId),
		CONSTRAINT FK_JobScriptInput_JobSchedule FOREIGN KEY (JobScheduleId) REFERENCES JobSchedule(JobScheduleId)
	)

	CREATE INDEX IX_JobScriptInput_JobScheduleId ON JobScriptInput(JobScheduleId)
END