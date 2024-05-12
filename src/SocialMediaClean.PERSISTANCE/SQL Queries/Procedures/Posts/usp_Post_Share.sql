CREATE OR ALTER PROCEDURE usp_Post_Share
	@PostID INT,
	@UserID INT
	AS
	BEGIN
	INSERT INTO Post_Shares (PostID, UserID)
		VALUES (@PostID, @UserID)
	END
	