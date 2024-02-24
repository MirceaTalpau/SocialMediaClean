USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_ChangePassword]    Script Date: 12/15/2023 3:02:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_ChangePassword]
@ID INT,
@Password VARCHAR(MAX),
@PasswordSalt VARCHAR(MAX)
AS
UPDATE Users
SET Password = @Password,PasswordSalt = @PasswordSalt,PasswordForgotToken = NULL
WHERE Users.ID = @ID
