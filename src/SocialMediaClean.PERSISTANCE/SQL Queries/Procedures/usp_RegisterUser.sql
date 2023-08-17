
CREATE OR ALTER PROCEDURE usp_RegisterUser
@FirstName VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(255),
@PhoneNumber VARCHAR(15),
@Password VARCHAR(MAX),
@PasswordSalt VARCHAR(MAX),
@BirthDay DATETIME,
@Gender VARCHAR(2)

AS

INSERT INTO Users(FirstName,LastName,Email,PhoneNumber,Password,PasswordSalt,BirthDay,GENDER)
VALUES (@FirstName,@LastName,@Email,@PhoneNumber,@Password,@PasswordSalt,@BirthDay,@Gender)
