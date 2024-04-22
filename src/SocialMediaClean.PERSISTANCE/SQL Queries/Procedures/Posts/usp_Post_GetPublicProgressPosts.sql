CREATE OR ALTER PROCEDURE usp_Post_GetPublicProgressPosts

AS

SELECT * 
FROM ProgressPost
WHERE StatusID = 1