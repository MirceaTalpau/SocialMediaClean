CREATE OR ALTER PROCEDURE usp_Friends_GetMyFriends
	@UserID INT
	AS
	BEGIN
		SELECT u.ID, u.FirstName, u.LastName, u.ProfilePictureURL
		FROM Users u
		INNER JOIN Friends f
		ON u.ID = f.ReceiverID
		WHERE f.SenderID = @UserID
		
		UNION

		SELECT u.ID, u.FirstName, u.LastName, u.ProfilePictureURL
		FROM Users u
		INNER JOIN Friends f
		ON u.ID = f.SenderID
		WHERE f.ReceiverID = @UserID
	END
