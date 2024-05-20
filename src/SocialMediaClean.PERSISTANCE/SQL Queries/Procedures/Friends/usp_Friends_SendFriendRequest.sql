CREATE OR ALTER PROCEDURE usp_Friends_SendFriendRequest
    @SenderID INT,
    @ReceiverID INT
AS
BEGIN
    -- Check if a friend request already exists between the Sender and Receiver in either direction
    IF EXISTS(SELECT 1 FROM FriendRequests WHERE SenderID = @SenderID AND ReceiverID = @ReceiverID)
    BEGIN
        -- Friend request already exists from Sender to Receiver, so do nothing
        RETURN;
    END
    ELSE IF EXISTS(SELECT 1 FROM FriendRequests WHERE SenderID = @ReceiverID AND ReceiverID = @SenderID)
    BEGIN
        -- Friend request already exists from Receiver to Sender, so do nothing
        RETURN;
    END
    ELSE
    BEGIN
        -- No existing friend request between Sender and Receiver, so insert a new one
        INSERT INTO FriendRequests (SenderID, ReceiverID)
        VALUES (@SenderID, @ReceiverID);
    END
END;
