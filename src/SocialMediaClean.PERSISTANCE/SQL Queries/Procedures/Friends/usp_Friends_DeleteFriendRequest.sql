CREATE OR ALTER PROCEDURE usp_Friends_DeleteFriendRequest
	@SenderID INT,
	@ReceiverID INT
	AS
	BEGIN
		DELETE FROM FriendRequests
		WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID)
		OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)
	END