USE [SocialMediaClean]
GO
/****** Object:  View [dbo].[ProgressPost]    Script Date: 6/7/2024 6:13:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ProgressPost]
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
    pr.ID AS ProgressID,
    p.StatusID,
    p.GroupID,
    p.Body,
    p.CreatedAt,
    pr.BeforeWeight,
    pr.AfterWeight,
    pr.BeforePictureURI,
    pr.AfterPictureURI,
    pr.BeforeDate,
    pr.AfterDate,
    u.ProfilePictureURL,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount,
    NULL AS SharedByID,
    NULL AS SharedByName,
    NULL AS SharedAt
FROM
    Posts p
INNER JOIN
    Progress pr ON p.ID = pr.PostID
INNER JOIN
    Users u ON u.ID = p.AuthorID
INNER JOIN
    Statuses s ON p.StatusID = s.ID
LEFT JOIN
    LikeCounts lc ON p.ID = lc.PostID
LEFT JOIN
    CommentCounts cc ON p.ID = cc.PostID
LEFT JOIN
    ShareCounts sc ON p.ID = sc.PostID

UNION ALL

SELECT
    p.ID AS PostID,
    p.AuthorID,
    pr.ID AS ProgressID,
    p.StatusID,
    p.GroupID,
    p.Body,
    p.CreatedAt,
    pr.BeforeWeight,
    pr.AfterWeight,
    pr.BeforePictureURI,
    pr.AfterPictureURI,
    pr.BeforeDate,
    pr.AfterDate,
    u.ProfilePictureURL,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount,
    ps.UserID AS SharedByID,
    us.FirstName + ' ' + us.LastName AS SharedByName,
    ps.CreatedAt AS SharedAt
FROM
    Posts p
INNER JOIN
    Progress pr ON p.ID = pr.PostID
INNER JOIN
    Users u ON u.ID = p.AuthorID
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
    Users us ON ps.UserID = us.ID;
GO