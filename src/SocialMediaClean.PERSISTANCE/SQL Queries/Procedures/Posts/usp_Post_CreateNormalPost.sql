CREATE OR ALTER PROCEDURE usp_Post_CreateNormalPost
@AuthorID INT,
@Body VARCHAR(MAX),
@StatusID INT = 1,
@ID INT OUTPUT

AS
BEGIN
	INSERT INTO Posts(AuthorID,Body,StatusID)
	VALUES(@AuthorID,@Body,@StatusID)

	SET @ID = SCOPE_IDENTITY();
END;


