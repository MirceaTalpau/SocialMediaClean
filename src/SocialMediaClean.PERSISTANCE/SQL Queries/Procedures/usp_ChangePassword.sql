CREATE OR ALTER PROCEDURE usp_ChangePassword
@ID INT,
@Password VARCHAR(MAX),
@PasswordSalt VARCHAR(MAX)
AS
UPDATE Users
SET Password = @Password,PasswordSalt = @PasswordSalt
WHERE Users.ID = @ID
