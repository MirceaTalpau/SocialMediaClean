USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_RegisterUser]    Script Date: 8/21/2023 6:45:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   PROCEDURE [dbo].[usp_RegisterUser]
@FirstName VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(255),
@PhoneNumber VARCHAR(15),
@Password VARCHAR(MAX),
@PasswordSalt VARCHAR(MAX),
@BirthDay DATETIME,
@Gender VARCHAR(2),
@EmailVerifyToken VARCHAR(MAX)

AS

INSERT INTO Users(FirstName,LastName,Email,PhoneNumber,Password,PasswordSalt,BirthDay,GENDER,EmailVerifyToken)
VALUES (@FirstName,@LastName,@Email,@PhoneNumber,@Password,@PasswordSalt,@BirthDay,@Gender,@EmailVerifyToken)
