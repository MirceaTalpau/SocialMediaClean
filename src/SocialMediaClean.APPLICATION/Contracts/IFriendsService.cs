using LinkedFit.DOMAIN.Models.DTOs.Friends;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IFriendsService
    {
        Task SendFriendRequest(FriendRequestDTO payload);
        Task AcceptFriendRequest(FriendRequestDTO payload);
    }
}