USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_CheckExistingUser]    Script Date: 12/15/2023 2:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER     PROCEDURE [dbo].[usp_CheckExistingUser]
@Email VARCHAR(255) = NULL,
@PhoneNumber VARCHAR(15) = NULL
AS

BEGIN
	IF @Email is NULL AND @PhoneNumber IS NOT NULL
		BEGIN
			SELECT u.ID
			FROM Users as u
			WHERE u.PhoneNumber = @PhoneNumber
		END;
	ELSE IF @Email IS NOT NULL AND @PhoneNumber IS NULL
		BEGIN
			SELECT u.ID
			FROM Users as u
			WHERE u.Email = @Email
		END;
END;
