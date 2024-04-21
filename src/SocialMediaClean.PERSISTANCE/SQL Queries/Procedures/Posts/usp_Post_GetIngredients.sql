CREATE OR ALTER PROCEDURE usp_Post_GetIngredients
	@RecipeID INT
	AS
	BEGIN
		SELECT *
		FROM
			Ingredients
		WHERE
			RecipeID = @RecipeID
	END