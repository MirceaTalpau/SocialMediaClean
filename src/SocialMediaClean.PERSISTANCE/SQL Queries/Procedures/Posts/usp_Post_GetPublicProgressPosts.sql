CREATE OR ALTER PROCEDURE usp_Post_GetPublicProgressPosts
@CurrentUserID INT
AS

BEGIN
	SELECT *,
	        CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
			FROM ProgressPost pp
			LEFT JOIN
			Post_Likes pl ON
				pp.PostID = pl.PostID
				ORDER BY pp.CreatedAt DESC;


END
