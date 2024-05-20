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
    }
}
