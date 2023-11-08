CREATE OR ALTER PROCEDURE usp_GetEmailConfirmationToken
@Email VARCHAR(250)
AS 
SELECT u.EmailVerifyToken
FROM Users as u
WHERE u.Email = @Email AND u.EmailVerified = 0