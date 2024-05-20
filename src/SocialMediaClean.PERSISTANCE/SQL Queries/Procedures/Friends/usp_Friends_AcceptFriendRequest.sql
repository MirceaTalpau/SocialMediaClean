  CREATE OR ALTER PROCEDURE usp_Friends_AcceptFriendRequest
    @ReceiverID INT,
    @SenderID INT
AS
BEGIN
    -- Check if a friendship already exists
    IF NOT EXISTS (SELECT 1 FROM Friends WHERE ReceiverID = @ReceiverID AND SenderID = @SenderID)
    BEGIN
        -- Insert the new friendship
        IF NOT EXISTS (SELECT 1 FROM FriendRequests WHERE ReceiverID = @ReceiverID AND SenderID = @SenderID)
        BEGIN
			RAISERROR('Friend request does not exist', 16, 1);
			RETURN;
		END
        INSERT INTO Friends (ReceiverID, SenderID) VALUES (@ReceiverID, @SenderID);

        -- Remove the friend request
        DELETE FROM FriendRequests WHERE ReceiverID = @ReceiverID AND SenderID = @SenderID;
    END

END;
