CREATE OR ALTER PROCEDURE usp_Friends_CheckIfTheyAreFriends
@UserID INT,
@FriendID INT
AS
BEGIN
  IF EXISTS (
    SELECT * FROM Friends
    WHERE (SenderID = @UserID AND ReceiverID = @FriendID)
    OR (SenderID = @FriendID AND ReceiverID = @UserID)
  )
  BEGIN
    SELECT 1
  END
  ELSE
  BEGIN
    SELECT 0
  END
END