CREATE OR ALTER PROCEDURE usp_Post_CreateRecipePost
@PostID INT,
@Name VARCHAR(50),
@Description VARCHAR(255),
@Instructions VARCHAR(511),
@CookingTime INT,
@Servings INT,
@Calories INT,
@Protein INT,
@Carbs INT,
@Fat INT,
@ID INT OUTPUT
AS
BEGIN
	INSERT INTO Recipe (PostID, Name, Description, Instructions, CookingTime, Servings, Calories, Protein, Carbs, Fat)
	VALUES (@PostID, @Name, @Description, @Instructions, @CookingTime, @Servings, @Calories, @Protein, @Carbs, @Fat)
	SET @ID = SCOPE_IDENTITY();
END


