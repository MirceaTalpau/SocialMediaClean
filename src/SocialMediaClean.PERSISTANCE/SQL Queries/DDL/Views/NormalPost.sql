
CREATE OR ALTER VIEW [dbo].[NormalPost]
AS
WITH LikeCounts AS (
    SELECT PostID, COUNT(*) AS LikesCount
    FROM Post_Likes
    GROUP BY PostID
),
CommentCounts AS (
    SELECT PostID, COUNT(*) AS CommentsCount
    FROM Comments
    GROUP BY PostID
),
ShareCounts AS (
    SELECT PostID, COUNT(*) AS SharesCount
    FROM Post_Shares
    GROUP BY PostID
)
SELECT 
    p.ID AS PostID,
    p.AuthorID,
    p.StatusID,
    p.Body,
    p.CreatedAt,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    u.ProfilePictureURL AS AuthorProfilePictureURL,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount,
    NULL AS SharedByID,
    NULL AS SharedByName,
    NULL AS SharedAt
FROM 
    Posts p
INNER JOIN 
    Users u ON p.AuthorID = u.ID
INNER JOIN 
    Statuses s ON p.StatusID = s.ID
LEFT JOIN 
    LikeCounts lc ON p.ID = lc.PostID
LEFT JOIN 
    CommentCounts cc ON p.ID = cc.PostID
LEFT JOIN 
    ShareCounts sc ON p.ID = sc.PostID
WHERE 
    NOT EXISTS (SELECT 1 FROM Recipe r WHERE r.PostID = p.ID)
    AND NOT EXISTS (SELECT 1 FROM Progress pr WHERE pr.PostID = p.ID)

UNION ALL

SELECT 
    p.ID AS PostID,
    p.AuthorID,
    p.StatusID,
    p.Body,
    p.CreatedAt,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    u.ProfilePictureURL AS AuthorProfilePictureURL,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount,
    ps.UserID AS SharedByID,
    us.FirstName + ' ' + us.LastName AS SharedByName,
    ps.CreatedAt AS SharedAt
FROM 
    Posts p
INNER JOIN 
    Users u ON p.AuthorID = u.ID
INNER JOIN 
    Statuses s ON p.StatusID = s.ID
LEFT JOIN 
    LikeCounts lc ON p.ID = lc.PostID
LEFT JOIN 
    CommentCounts cc ON p.ID = cc.PostID
LEFT JOIN 
    ShareCounts sc ON p.ID = sc.PostID
INNER JOIN 
    Post_Shares ps ON p.ID = ps.PostID
INNER JOIN 
    Users us ON ps.UserID = us.ID
WHERE 
    NOT EXISTS (SELECT 1 FROM Recipe r WHERE r.PostID = p.ID)
    AND NOT EXISTS (SELECT 1 FROM Progress pr WHERE pr.PostID = p.ID);
GO