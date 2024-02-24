USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_RegisterUser]    Script Date: 11/30/2023 3:58:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER     PROCEDURE [dbo].[usp_RegisterUser]
@FirstName VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(255),
@PhoneNumber VARCHAR(15) = NULL,
@Password VARCHAR(MAX) = NULL,
@PasswordSalt VARCHAR(MAX) = NULL,
@BirthDay DATETIME = NULL,
@Gender VARCHAR(2) = NULL,
@EmailVerifyToken VARCHAR(MAX) = NULL,
@ProfileAvatar VARCHAR(MAX) = NULL

AS

BEGIN

	IF @PhoneNumber IS NULL AND @Password IS NULL AND @PasswordSalt IS NULL AND @BirthDay IS NULL AND @Gender IS NULL AND @EmailVerifyToken IS NULL
		BEGIN
			INSERT INTO Users(FirstName,LastName,Email,ProfilePictureURL,EmailVerified)
			VALUES(@FirstName,@LastName,@Email,@ProfileAvatar,1)
		END;
	ELSE
		BEGIN
			INSERT INTO Users(FirstName,LastName,Email,PhoneNumber,Password,PasswordSalt,BirthDay,GENDER,EmailVerifyToken)
			VALUES (@FirstName,@LastName,@Email,@PhoneNumber,@Password,@PasswordSalt,@BirthDay,@Gender,@EmailVerifyToken)
		END;

END;