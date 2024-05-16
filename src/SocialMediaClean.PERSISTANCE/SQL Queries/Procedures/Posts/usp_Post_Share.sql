CREATE OR ALTER PROCEDURE usp_Post_Share
	@PostID INT,
	@UserID INT
	AS
	BEGIN
	IF NOT EXISTS (SELECT * FROM Post_Shares WHERE PostID = @PostID AND UserID = @UserID)
		INSERT INTO Post_Shares (PostID, UserID)
			VALUES (@PostID, @UserID)
	END
	