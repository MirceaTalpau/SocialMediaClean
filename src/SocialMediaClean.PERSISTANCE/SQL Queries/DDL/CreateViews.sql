CREATE OR ALTER VIEW RecipePost 
AS
SELECT p.ID AS PostID,p.AuthorID,r.ID AS RecipeID,p.StatusID,p.GroupID,
p.SharedByID,us.FirstName + ' ' + us.LastName as SharedBy,p.Body,p.CreatedAt,
u.FirstName + ' ' + u.LastName AS AuthorName,
r.Name AS RecipeName,r.Description,r.Instructions,r.CookingTime,
r.Servings,r.Calories,r.Protein,r.Carbs,r.Fat

FROM Posts p
INNER JOIN Recipe r
ON p.ID = r.PostID
INNER JOIN Users u
ON p.AuthorID = u.ID
INNER JOIN Users us
ON p.AuthorID = us.ID

CREATE VIEW ProgressPost
AS
SELECT p.ID AS PostID,p.AuthorID,pr.ID AS ProgressID,p.StatusID,p.GroupID,p.SharedByID,p.Body,p.CreatedAt,
pr.BeforeWeight,pr.AfterWeight,pr.BeforePictureURI,pr.AfterPictureURI,pr.BeforeDate,pr.AfterDate
FROM Posts p
INNER JOIN Progress pr
ON p.ID = pr.PostID