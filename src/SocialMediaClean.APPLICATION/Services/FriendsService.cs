using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Friends;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly IFriendsRepository _friendsRepository;
        public FriendsService(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        public async Task SendFriendRequest(FriendRequestDTO payload)
        {
            await _friendsRepository.SendFriendRequest(payload);
        }
        public async Task AcceptFriendRequest(FriendRequestDTO payload)
        {
            await _friendsRepository.AcceptFriendRequest(payload);
        }
        public async Task<IEnumerable<Friend>> GetMyFriendsAsync(int userID)
        {
            return await _friendsRepository.GetMyFriends(userID);
        }
        public async Task<IEnumerable<Friend>> GetMyFriendRequests(int userID)
        {
            return await _friendsRepository.GetMyFriendRequests(userID);
        }
        public async Task RemoveFriend(FriendRequestDTO payload)
        {
            await _friendsRepository.DeleteFried(payload);
        }
        public async Task RemoveFriendRequest(FriendRequestDTO payload)
        {
            await _friendsRepository.DeleteFriendRequest(payload);
        }
    }
}
