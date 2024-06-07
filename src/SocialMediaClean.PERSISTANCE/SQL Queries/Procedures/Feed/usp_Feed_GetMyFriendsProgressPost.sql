ALTER PROCEDURE [dbo].[usp_Post_GetPublicProgressPosts]
    @CurrentUserID INT
AS
BEGIN
    SELECT pp.*,
        CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
    FROM ProgressPost pp
    LEFT JOIN
    Post_Likes pl ON
        pp.PostID = pl.PostID AND pl.UserID = @CurrentUserID
    WHERE (
        (pp.AuthorID = @CurrentUserID AND pp.StatusID IN (1, 2, 3))
        OR 
        (pp.AuthorID IN (
            SELECT SenderID AS FriendID
            FROM Friends
            WHERE ReceiverID = @CurrentUserID
            UNION
            SELECT ReceiverID AS FriendID
            FROM Friends
            WHERE SenderID = @CurrentUserID
        ) AND pp.StatusID IN (1, 2))
    )
    ORDER BY 
        CASE WHEN pp.SharedAt IS NOT NULL THEN pp.SharedAt ELSE pp.CreatedAt END DESC;
END