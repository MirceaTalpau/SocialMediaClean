CREATE OR ALTER PROCEDURE usp_account_GetUserData
@ID INT
AS
SELECT ID,FirstName + ' ' + LastName AS username,email,ProfilePictureURL
FROM Users
WHERE ID = @ID