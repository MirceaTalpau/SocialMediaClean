CREATE OR ALTER PROCEDURE usp_InsertEmailConfirmationToken
@EmailConfirmationToken VARCHAR(MAX),
@ID INT
AS 
UPDATE Users
SET EmailVerifyToken = @EmailConfirmationToken
WHERE ID = @ID