CREATE OR ALTER PROCEDURE usp_Post_GetPublicNormalPosts
@CurrentUserID INT
AS
BEGIN
	SELECT *,
	        CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
	FROM NormalPost np
	LEFT JOIN 
        Post_Likes pl ON
		np.PostID = pl.PostID AND pl.UserID = @CurrentUserID
END

