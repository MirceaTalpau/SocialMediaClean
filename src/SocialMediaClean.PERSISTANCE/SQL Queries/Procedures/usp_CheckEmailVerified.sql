CREATE OR ALTER PROCEDURE usp_CheckEmailVerified
@Email VARCHAR(250)
AS
SELECT u.EmailVerified
FROM Users as u
WHERE u.Email = @Email