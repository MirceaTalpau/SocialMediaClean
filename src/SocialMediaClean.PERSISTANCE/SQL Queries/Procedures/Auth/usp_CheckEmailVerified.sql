USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_CheckEmailVerified]    Script Date: 12/17/2023 4:52:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_CheckEmailVerified]
@Email VARCHAR(250),
@Verified BIT OUTPUT 
AS

BEGIN

	SELECT @Verified = u.EmailVerified
	FROM Users as u
	WHERE u.Email = @Email

	IF @Verified IS NULL
		BEGIN
			SET @Verified = 0
		END;
END;
