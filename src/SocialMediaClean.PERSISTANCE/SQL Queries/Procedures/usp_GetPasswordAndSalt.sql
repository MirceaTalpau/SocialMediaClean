USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPasswordAndSalt]    Script Date: 8/15/2023 11:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_GetPasswordAndSalt]
@Email varchar(255)
AS
SELECT u.ID,u.Password,u.PasswordSalt
FROM Users as u
WHERE u.Email = @Email; 