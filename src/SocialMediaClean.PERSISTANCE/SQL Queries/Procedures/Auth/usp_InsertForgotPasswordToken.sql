CREATE OR ALTER PROCEDURE usp_InsertForgotPasswordToken
@PasswordForgotToken VARCHAR(max),
@ID INT
AS
UPDATE Users
SET PasswordForgotToken = @PasswordForgotToken
WHERE ID = @ID