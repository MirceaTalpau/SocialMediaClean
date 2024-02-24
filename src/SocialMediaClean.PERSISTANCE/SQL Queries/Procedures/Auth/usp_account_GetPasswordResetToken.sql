USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_account_GetPasswordResetToken]    Script Date: 12/11/2023 8:54:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   procedure [dbo].[usp_account_GetPasswordResetToken]
@ID int
AS 

SELECT PasswordForgotToken
FROM Users
WHERE ID = @ID

