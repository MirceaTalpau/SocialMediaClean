CREATE OR ALTER VIEW RecipePost 
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
    r.ID AS RecipeID,
    p.StatusID,
    p.GroupID,
    p.SharedByID,
    us.FirstName + ' ' + us.LastName as SharedBy,
    p.Body,
    p.CreatedAt,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    u.ProfilePictureURL AS AuthorProfilePictureURL,
    r.Name AS RecipeName,
    r.Description,
    r.Instructions,
    r.CookingTime,
    r.Servings,
    r.Calories,
    r.Protein,
    r.Carbs,
    r.Fat,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount
FROM 
    Posts p
INNER JOIN 
    Recipe r ON p.ID = r.PostID
INNER JOIN 
    Users u ON p.AuthorID = u.ID
LEFT JOIN 
    Users us ON p.SharedByID = us.ID
INNER JOIN 
    Statuses s ON p.StatusID = s.ID
LEFT JOIN 
    LikeCounts lc ON p.ID = lc.PostID
LEFT JOIN 
    CommentCounts cc ON p.ID = cc.PostID
LEFT JOIN 
    ShareCounts sc ON p.ID = sc.PostID
WHERE 
    s.Status = 'Public';


CREATE OR ALTER VIEW NormalPost
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
    p.GroupID,
    p.SharedByID,
    p.Body,
    p.CreatedAt,
    u.FirstName + ' ' + u.LastName AS AuthorName,
    u.ProfilePictureURL AS AuthorProfilePictureURL,
    ISNULL(lc.LikesCount, 0) AS LikesCount,
    ISNULL(cc.CommentsCount, 0) AS CommentsCount,
    ISNULL(sc.SharesCount, 0) AS SharesCount
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
    AND NOT EXISTS (SELECT 1 FROM Progress pr WHERE pr.PostID = p.ID);

CREATE VIEW ProgressPost
AS
SELECT p.ID AS PostID,p.AuthorID,pr.ID AS ProgressID,p.StatusID,p.GroupID,p.SharedByID,p.Body,p.CreatedAt,
pr.BeforeWeight,pr.AfterWeight,pr.BeforePictureURI,pr.AfterPictureURI,pr.BeforeDate,pr.AfterDate,u.ProfilePictureURL,
u.FirstName + ' ' + u.LastName as AuthorName
FROM Posts p
INNER JOIN Progress pr
ON p.ID = pr.PostID
INNER JOIN Users u
ON u.ID = p.AuthorID
INNER JOIN 
    Statuses s ON p.StatusID = s.ID
LEFT JOIN 
    LikeCounts lc ON p.ID = lc.PostID
LEFT JOIN 
    CommentCounts cc ON p.ID = cc.PostID
LEFT JOIN 
    ShareCounts sc ON p.ID = sc.PostID


CREATE OR ALTER VIEW MediaPost
AS
SELECT po.ID AS PostID, pi.CreatedAt AS PictureCreatedAt,pi.PictureURI,v.CreatedAt AS VideoCreatedAt,v.VideoURI
FROM Posts po
LEFT JOIN Pictures pi
ON po.ID  =pi.PostID
LEFT JOIN Videos v
ON v.PostID = po.ID

CREATE OR ALTER VIEW PostComments
AS
SELECT c.ID AS CommentID,c.PostID,c.AuthorID,c.Body,c.CreatedAt,c.ParentID,
u.FirstName + ' ' + u.LastName AS AuthorName,u.ProfilePictureURL
FROM Comments c
INNER JOIN Users u
ON c.AuthorID = u.ID
