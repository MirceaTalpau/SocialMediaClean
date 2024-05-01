CREATE OR ALTER PROCEDURE usp_Comment_Get_Comment
	@CommentId INT
	AS
	BEGIN
		SELECT * FROM PostComments WHERE ID = @CommentId
	END
