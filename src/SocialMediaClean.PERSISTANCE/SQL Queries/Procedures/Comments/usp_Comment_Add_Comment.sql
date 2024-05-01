USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_Comment_Add_Comment]    Script Date: 4/30/2024 1:51:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_Comment_Add_Comment]
@PostID INT,
@AuthorID INT,
@ParentID INT = null,
@Body NVARCHAR(MAX),
@InsertedID INT OUTPUT
AS
BEGIN
	INSERT INTO Comments(PostID,AuthorID,ParentID,Body)
	VALUES(@PostID,@AuthorID,@ParentID,@Body)
	SET @InsertedID = SCOPE_IDENTITY();

END
