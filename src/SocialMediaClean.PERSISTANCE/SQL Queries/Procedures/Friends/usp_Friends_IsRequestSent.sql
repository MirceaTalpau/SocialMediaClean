CREATE OR ALTER PROCEDURE usp_Friends_IsRequestSent
@SenderID INT,
@ReceiverID INT
AS
BEGIN
	IF EXISTS(
	SELECT * FROM FriendRequests
	WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID) OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)
	)
	BEGIN
	SELECT 1
	END
		ELSE
		BEGIN
		SELECT 0
		END
END