CREATE OR ALTER PROCEDURE usp_Chat_GetUserChats
    @UserID INT
AS
BEGIN
    SELECT u.FirstName, u.LastName, u.ProfilePictureURL, c.ID as ChatID, m.Body as LastMessage, m.CreatedAt as CreatedAt
    FROM Chat c
    INNER JOIN Users u ON (c.User1ID = u.ID AND c.User2ID = @UserId) OR (c.User2ID = u.ID AND c.User1ID = @UserId)
    INNER JOIN (
        SELECT m.ChatID, m.Body, m.CreatedAt,
               ROW_NUMBER() OVER (PARTITION BY m.ChatID ORDER BY m.CreatedAt DESC) as RowNum
        FROM ChatMessages m
    ) m ON c.ID = m.ChatID AND m.RowNum = 1
    WHERE c.User1ID = @UserId OR c.User2ID = @UserId
    ORDER BY m.CreatedAt DESC
END