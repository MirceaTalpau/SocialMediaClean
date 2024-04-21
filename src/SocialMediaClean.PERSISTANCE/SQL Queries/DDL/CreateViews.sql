CREATE OR ALTER VIEW RecipePost 
AS
SELECT p.ID AS PostID,p.AuthorID,r.ID AS RecipeID,p.StatusID,p.GroupID,
p.SharedByID,us.FirstName + ' ' + us.LastName as SharedBy,p.Body,p.CreatedAt,
u.FirstName + ' ' + u.LastName AS AuthorName,u.ProfilePictureURL AS AuthorProfilePictureURL,
r.Name AS RecipeName,r.Description,r.Instructions,r.CookingTime,
r.Servings,r.Calories,r.Protein,r.Carbs,r.Fat
FROM Posts p
INNER JOIN Recipe r
ON p.ID = r.PostID
INNER JOIN Users u
ON p.AuthorID = u.ID
INNER JOIN Users us
ON p.AuthorID = us.ID
INNER JOIN Statuses s 
ON p.StatusID = s.ID
WHERE s.Status = 'Public'

CREATE VIEW NormalPost
AS
SELECT Posts.*
FROM Posts
LEFT JOIN PROGRESS ON Posts.ID = PROGRESS.PostID
LEFT JOIN Recipe ON Posts.ID = Recipe.PostID
WHERE PROGRESS.PostID IS NULL
AND Recipe.PostID IS NULL
ORDER BY Posts.CreatedAt DESC;

CREATE OR ALTER VIEW NormalPost
AS
SELECT p.ID AS PostID,p.AuthorID,p.StatusID,p.GroupID,p.SharedByID,p.Body,p.CreatedAt,
u.FirstName + ' ' + u.LastName AS AuthorName, u.ProfilePictureURL
FROM Posts p
INNER JOIN Users u
ON p.AuthorID = u.ID
INNER JOIN Statuses s
ON p.StatusID = s.ID
WHERE s.Status = 'Public'

CREATE VIEW ProgressPost
AS
SELECT p.ID AS PostID,p.AuthorID,pr.ID AS ProgressID,p.StatusID,p.GroupID,p.SharedByID,p.Body,p.CreatedAt,
pr.BeforeWeight,pr.AfterWeight,pr.BeforePictureURI,pr.AfterPictureURI,pr.BeforeDate,pr.AfterDate
FROM Posts p
INNER JOIN Progress pr
ON p.ID = pr.PostID


CREATE OR ALTER VIEW MediaPost
AS
SELECT po.ID AS PostID, pi.CreatedAt AS PictureCreatedAt,pi.PictureURI,v.CreatedAt AS VideoCreatedAt,v.VideoURI
FROM Posts po
LEFT JOIN Pictures pi
ON po.ID  =pi.PostID
LEFT JOIN Videos v
ON v.PostID = po.ID
