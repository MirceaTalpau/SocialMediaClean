USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_CheckExistingUser]    Script Date: 8/18/2023 7:09:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_CheckExistingUser]
@Email VARCHAR(255),
@PhoneNumber VARCHAR(15) = NULL
AS
SELECT u.ID
FROM Users as u
WHERE u.Email = @Email OR u.PhoneNumber = @PhoneNumber
