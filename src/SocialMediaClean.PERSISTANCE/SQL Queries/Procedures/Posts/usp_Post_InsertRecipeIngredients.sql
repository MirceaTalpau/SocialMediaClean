CREATE OR ALTER PROCEDURE usp_Post_InsertRecipeIngredients
@Ingredients RecipeTableType READONLY
AS
BEGIN
	INSERT INTO Ingredients (RecipeID, Name, Quantity, Unit)
	SELECT RecipeID, Name, Quantity, Unit FROM @Ingredients
END