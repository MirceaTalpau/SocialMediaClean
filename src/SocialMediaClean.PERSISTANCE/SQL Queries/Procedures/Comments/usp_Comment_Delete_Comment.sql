CREATE OR ALTER PROCEDURE usp_Comment_Delete_Comment
@CommentID INT
AS
BEGIN
	DELETE FROM Comments WHERE ID = @CommentID OR ParentID = @CommentID
END