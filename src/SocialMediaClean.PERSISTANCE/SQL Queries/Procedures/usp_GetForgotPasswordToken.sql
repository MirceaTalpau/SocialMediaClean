CREATE OR ALTER PROCEDURE usp_GetForgotPasswordToken
@Email VARCHAR(250)
AS 
SELECT u.PasswordForgotToken
FROM Users as u
WHERE u.Email = @Email AND u.EmailVerified = 1