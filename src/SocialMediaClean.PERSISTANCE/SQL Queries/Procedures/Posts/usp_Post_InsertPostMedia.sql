USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_Post_InsertPostMedia]    Script Date: 3/29/2024 5:13:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_Post_InsertPostMedia]
    @Pictures PicturesTableType READONLY,
    @Videos VideosTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insert pictures
        INSERT INTO Pictures (PostID, PictureURI, CreatedAt)
        SELECT PostID, PictureURI, CreatedAt FROM @Pictures;

        -- Insert videos
        INSERT INTO Videos (PostID, VideoURI, CreatedAt)
        SELECT PostID, VideoURI, CreatedAt FROM @Videos;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END

