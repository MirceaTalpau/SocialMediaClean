CREATE OR ALTER PROCEDURE usp_Post_GetPublicNormalPosts
AS
SELECT np.*,u.FirstName + ' ' + u.LastName AS AuthorName,
shared.FirstName + ' ' + shared.LastName AS SharedBy
FROM NormalPost np
INNER JOIN Users u
ON u.ID = np.AuthorID
LEFT JOIN Users shared
ON shared.ID = np.SharedByID
WHERE np.StatusID = 1
