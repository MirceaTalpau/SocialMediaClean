CREATE OR ALTER PROCEDURE usp_ValidateEmail
@Email VARCHAR(250)
AS 
UPDATE Users
SET EmailVerified = 1
WHERE Email = @Email