CREATE OR ALTER PROCEDURE usp_Post_DeletePost
@PostID INT
AS
BEGIN
	-- Check if the post is a recipe post
	IF EXISTS (SELECT 1 FROM Recipe WHERE PostID = @PostID)
	BEGIN
		-- Get RecipeID
		DECLARE @RecipeID INT;
		SELECT @RecipeID = ID FROM Recipe WHERE PostID = @PostID;

		-- Delete ingredients associated with the recipe
		DELETE FROM Ingredients WHERE RecipeID = @RecipeID;
	END

	-- Delete other associated data
	DELETE FROM Pictures WHERE PostID = @PostID;
	DELETE FROM Videos WHERE PostID = @PostID;
	DELETE FROM Recipe WHERE PostID = @PostID;
	DELETE FROM Progress WHERE PostID = @PostID;
	DELETE FROM Posts WHERE ID = @PostID;
END
