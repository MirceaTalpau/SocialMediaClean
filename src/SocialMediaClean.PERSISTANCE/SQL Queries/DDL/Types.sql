CREATE TYPE PicturesTableType AS TABLE(
        PostID INT,
        PictureURI NVARCHAR(MAX),
        CreatedAt DATETIME
    )
CREATE TYPE VideosTableType AS TABLE(
        PostID INT,
        VideoURI NVARCHAR(MAX),
        CreatedAt DATETIME
    )
