CREATE OR ALTER PROCEDURE usp_Friends_GetMyFriendRequests
@UserID INT
AS
BEGIN
    SELECT u.ID, u.FirstName, u.LastName, u.ProfilePictureURL
    FROM Users u
    INNER JOIN FriendRequests fr
    ON u.ID = fr.SenderID
    WHERE fr.ReceiverID = @UserID
    
    UNION
    
    SELECT u.ID, u.FirstName, u.LastName, u.ProfilePictureURL
    FROM Users u
    INNER JOIN FriendRequests fr
    ON u.ID = fr.ReceiverID
    WHERE fr.SenderID = @UserID
END