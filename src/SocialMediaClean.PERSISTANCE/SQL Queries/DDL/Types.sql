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
CREATE TYPE IngredientsTableType AS TABLE
(
    RecipeID INT,
	Name VARCHAR(50),
	Quantity INT,
	Unit VARCHAR(50)
)