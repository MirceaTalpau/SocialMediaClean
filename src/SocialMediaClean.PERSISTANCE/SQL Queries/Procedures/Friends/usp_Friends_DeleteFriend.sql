CREATE OR ALTER PROCEDURE usp_Friends_DeleteFriend
	@SenderID INT,
	@ReceiverID INT
	AS
	BEGIN
		DELETE FROM Friends
		WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID)
		OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)
	END