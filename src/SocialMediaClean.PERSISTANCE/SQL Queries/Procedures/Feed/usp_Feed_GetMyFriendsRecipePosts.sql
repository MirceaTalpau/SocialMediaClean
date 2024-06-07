CREATE OR ALTER PROCEDURE usp_Feed_GetMyFriendsRecipePosts
    @CurrentUserID INT
AS
BEGIN
    SELECT rp.*,
            CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
    FROM RecipePost rp
    LEFT JOIN 
        Post_Likes pl ON
        rp.PostID = pl.PostID AND pl.UserID = @CurrentUserID
    WHERE (
        (rp.AuthorID = @CurrentUserID AND rp.StatusID IN (1, 2, 3))
        OR 
        (rp.AuthorID IN (
            SELECT SenderID AS FriendID
            FROM Friends
            WHERE ReceiverID = @CurrentUserID
            UNION
            SELECT ReceiverID AS FriendID
            FROM Friends
            WHERE SenderID = @CurrentUserID
        ) AND rp.StatusID IN (1, 2))
    )
    ORDER BY rp.CreatedAt DESC;
END