CREATE OR ALTER PROCEDURE usp_Post_GetAllRecipePosts
@CurrentUserID INT
AS
	BEGIN
	SELECT *,
	        CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
	FROM RecipePost rp
	LEFT JOIN 
		Post_Likes pl ON
		rp.PostID = pl.PostID AND pl.UserID = @CurrentUserID
	ORDER BY rp.CreatedAt DESC;
	END
END
