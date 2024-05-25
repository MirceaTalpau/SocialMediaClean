using LinkedFit.DOMAIN.Models.DTOs.Friends;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IFriendsRepository
    {
        Task AcceptFriendRequest(FriendRequestDTO payload);
        Task DeleteFried(FriendRequestDTO payload);
        Task DeleteFriendRequest(FriendRequestDTO payload);
        Task<IEnumerable<Friend>> GetMyFriendRequests(int userID);
        Task<IEnumerable<Friend>> GetMyFriends(int userID);
        Task SendFriendRequest(FriendRequestDTO payload);
    }
}