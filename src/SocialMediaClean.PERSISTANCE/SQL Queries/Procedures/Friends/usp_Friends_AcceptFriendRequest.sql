USE [SocialMediaClean]
GO
/****** Object:  StoredProcedure [dbo].[usp_Friends_AcceptFriendRequest]    Script Date: 6/4/2024 2:55:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_Friends_AcceptFriendRequest]
    @ReceiverID INT,
    @SenderID INT
AS
BEGIN
    BEGIN TRY
        -- Check if a friendship already exists
        IF EXISTS (SELECT 1 FROM Friends WHERE (ReceiverID = @ReceiverID AND SenderID = @SenderID) OR (ReceiverID = @SenderID AND SenderID = @ReceiverID))
        BEGIN
            RAISERROR('Friendship already exists', 16, 1);
            RETURN;
        END
        
        -- Check if a friend request exists
        IF NOT EXISTS (SELECT 1 FROM FriendRequests WHERE (ReceiverID = @ReceiverID AND SenderID = @SenderID) OR (ReceiverID = @SenderID AND SenderID = @ReceiverID) )
        BEGIN
            RAISERROR('Friend request does not exist', 16, 1);
            RETURN;
        END
        
        -- Insert the new friendship
        INSERT INTO Friends (ReceiverID, SenderID) VALUES (@ReceiverID, @SenderID);
        
        -- Remove the friend request
        DELETE FROM FriendRequests WHERE (ReceiverID = @ReceiverID AND SenderID = @SenderID) OR (ReceiverID = @SenderID AND SenderID = @ReceiverID));
        
        PRINT 'Friend request accepted successfully';
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;
        
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;