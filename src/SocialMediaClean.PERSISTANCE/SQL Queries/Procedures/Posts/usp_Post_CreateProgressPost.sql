CREATE OR ALTER PROCEDURE usp_Post_CreateProgressPost
@PostID INT,
@BeforeWeight INT,
@AfterWeight INT,
@BeforePictureUri NVARCHAR(255),
@AfterPictureUri NVARCHAR(255),
@BeforeDate DATE,
@AfterDate DATE,
@ID INT OUTPUT

AS

BEGIN
	INSERT INTO Progress (BeforeWeight, AfterWeight, BeforePictureUri, AfterPictureUri, BeforeDate, AfterDate)
	VALUES (@BeforeWeight, @AfterWeight, @BeforePictureUri, @AfterPictureUri, @BeforeDate, @AfterDate)
	SET @ID = SCOPE_IDENTITY();
END

