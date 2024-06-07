CREATE OR ALTER PROCEDURE usp_Feed_GetMyFriendsNormalPosts
    @UserID INT
AS
SELECT np.*,
        CASE WHEN pl.UserID IS NOT NULL THEN 1 ELSE 0 END AS CurrentUserLiked
FROM NormalPost np
LEFT JOIN
Post_Likes pl ON
np.PostID = pl.PostID AND pl.UserID = @UserID
WHERE (
    (np.AuthorID = @UserID AND np.StatusID IN (1, 2, 3))
    OR 
    (np.AuthorID IN (
        SELECT SenderID AS FriendID
        FROM Friends
        WHERE ReceiverID = @UserID
        UNION
        SELECT ReceiverID AS FriendID
        FROM Friends
        WHERE SenderID = @UserID
    ) AND np.StatusID IN (1, 2))
)