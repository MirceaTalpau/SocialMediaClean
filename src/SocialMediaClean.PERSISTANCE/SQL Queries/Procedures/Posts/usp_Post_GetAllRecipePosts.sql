CREATE OR ALTER PROCEDURE usp_Post_GetAllRecipePosts
AS
SELECT * FROM RecipePost
ORDER BY CreatedAt DESC;