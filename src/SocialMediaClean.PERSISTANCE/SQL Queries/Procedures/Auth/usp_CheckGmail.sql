CREATE OR ALTER PROCEDURE usp_CheckGmail
@Email VARCHAR(250),
@Exists BIT OUTPUT,
@ID INT OUTPUT

AS

SET NOCOUNT ON;

BEGIN
	IF EXISTS (	SELECT 1 FROM Users WHERE Email = @Email AND Password IS NULL AND PasswordSalt IS NULL)
		BEGIN
			SELECT @ID = ID FROM Users WHERE Email = @Email AND Password IS NULL AND PasswordSalt IS NULL
			SET @Exists = 1
		END
	ELSE
		SET @Exists = 0
		SET @ID = -1
END;

