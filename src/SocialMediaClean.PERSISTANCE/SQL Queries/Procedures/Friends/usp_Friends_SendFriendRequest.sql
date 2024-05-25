CREATE OR ALTER PROCEDURE usp_Friends_SendFriendRequest
@SenderID INT,
@ReceiverID INT
AS
BEGIN
    -- Check if the Sender and Receiver are already friends
    IF EXISTS (
        SELECT 1
        FROM Friends
        WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID)
        OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)
    )
    BEGIN
        -- The Sender and Receiver are already friends, so do nothing
        RETURN;
    END
    ELSE
    BEGIN
        -- Check if a friend request already exists between the Sender and Receiver in either direction
        IF EXISTS (
            SELECT 1
            FROM FriendRequests
            WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID)
            OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)
        )
        BEGIN
            -- Friend request already exists in either direction, so do nothing
            RETURN;
        END
        ELSE
        BEGIN
            -- No existing friend request or friendship between Sender and Receiver, so insert a new friend request
            INSERT INTO FriendRequests (SenderID, ReceiverID)
            VALUES (@SenderID, @ReceiverID);
        END
    END
END;