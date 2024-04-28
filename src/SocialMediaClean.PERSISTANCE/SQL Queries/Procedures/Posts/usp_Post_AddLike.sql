CREATE OR ALTER PROCEDURE usp_Post_AddLike
@PostID INT,
@UserID INT
AS
	BEGIN
		IF EXISTS(
		SELECT 1 FROM Post_Likes WHERE PostID = @PostID AND UserID = @UserID)
			BEGIN
				DELETE FROM Post_Likes
				WHERE PostID = @PostID AND UserID = @UserID
			END;
		ELSE
			BEGIN
				INSERT INTO Post_Likes(PostID,UserID)
				VALUES(@PostID,@UserID)
			END;
	END;


