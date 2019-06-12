IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[eddsdbo].[JobSchedule]') 
         AND name = 'DirectSql'
)
BEGIN
    ALTER TABLE [eddsdbo].[JobSchedule]
    ADD [DirectSql] bit
END