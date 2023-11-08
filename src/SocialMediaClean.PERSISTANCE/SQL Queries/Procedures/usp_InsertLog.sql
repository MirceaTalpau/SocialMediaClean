CREATE OR ALTER PROCEDURE usp_InsertLog
@Message VARCHAR(MAX),
@LogLevel VARCHAR(50)
AS
INSERT INTO dbo.LogEntries (Message, LogLevel)
VALUES (@Message, @LogLevel)