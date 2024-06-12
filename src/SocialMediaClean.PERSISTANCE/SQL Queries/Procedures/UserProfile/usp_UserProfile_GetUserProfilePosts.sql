CREATE OR ALTER PROCEDURE usp_Post_GetUserProfilePosts
@CurrentUserID INT,
@ProfileUserID INT
AS
BEGIN
    SELECT np.*,
            CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked,
            CASE WHEN f.SenderID IS NOT NULL THEN 1 ELSE 0 END AS AreFriends
    FROM NormalPost np
    LEFT JOIN Post_Likes pl ON
        np.PostID = pl.PostID AND pl.UserID = @CurrentUserID
    LEFT JOIN Friends f ON
        (f.SenderID = @ProfileUserID AND f.ReceiverID = @CurrentUserID)
        OR (f.SenderID = @CurrentUserID AND f.ReceiverID = @ProfileUserID)
    WHERE 
        np.SharedByID IS NULL -- Posts that are not shared
        AND np.AuthorID = @ProfileUserID -- Posts authored by the profile user
        AND (
            (np.StatusID = 1) -- Public posts
            OR
            (np.StatusID = 2 AND (f.SenderID IS NOT NULL OR @ProfileUserID = @CurrentUserID)) -- Posts visible to friends or if the current user is the profile user
            OR
            (np.StatusID = 3 AND @ProfileUserID = @CurrentUserID) -- Posts visible only to the profile user
        )
END