CREATE OR ALTER PROCEDURE usp_Chat_GetChatIdByUserID
	@User1ID INT,
	@User2ID INT
	AS
	BEGIN
	SELECT * FROM CHAT WHERE (User1ID = @User1ID AND User2ID = @User2ID) OR (User1ID = @User2ID AND User2ID = @User1ID)
	END