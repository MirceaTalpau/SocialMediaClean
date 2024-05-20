using LinkedFit.DOMAIN.Models.DTOs.Friends;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IFriendsRepository
    {
        Task SendFriendRequest(FriendRequestDTO payload);
        Task AcceptFriendRequest(FriendRequestDTO payload);
    }
}