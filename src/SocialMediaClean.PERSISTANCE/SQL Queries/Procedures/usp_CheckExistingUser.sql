CREATE OR ALTER PROCEDURE usp_CheckExistingUser
@Email VARCHAR(255),
@PhoneNumber VARCHAR(15)
AS
SELECT u.Email
FROM Users as u
WHERE u.Email = @Email OR u.PhoneNumber = @PhoneNumber
