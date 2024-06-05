CREATE OR ALTER PROCEDURE usp_Chat_InsertChatMessage
	@ChatID INT,
	@Message NVARCHAR(4000),
	@AuthorID INT,
	@CreatedAt DATETIME
	AS
	BEGIN
	INSERT INTO ChatMessages (ChatID, Body, AuthorID, CreatedAt)
		VALUES (@ChatID, @Message, @AuthorID, @CreatedAt)
	END