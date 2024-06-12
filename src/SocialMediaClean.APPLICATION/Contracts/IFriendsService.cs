using LinkedFit.DOMAIN.Models.DTOs.Friends;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IFriendsService
    {
        Task SendFriendRequest(FriendRequestDTO payload);
        Task AcceptFriendRequest(FriendRequestDTO payload);
        Task<IEnumerable<Friend>> GetMyFriendsAsync(int userID);
        Task<IEnumerable<Friend>> GetMyFriendRequests(int userID);
        Task RemoveFriendRequest(FriendRequestDTO payload);
        Task RemoveFriend(FriendRequestDTO payload);
        Task<bool> CheckIfTheyAreFriends(int userID, int friendID);
        Task<bool> IsRequestSent(FriendRequestDTO payload);
    }
}