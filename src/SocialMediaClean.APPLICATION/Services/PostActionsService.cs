using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.PostActions;
using LinkedFit.PERSISTANCE.Interfaces;
using LinkedFit.PERSISTANCE.Repositories;

namespace LinkedFit.APPLICATION.Services
{
    public class PostActionsService : IPostActionsService
    {
        private readonly IPostActionsRepository _postActionsRepository;
        public PostActionsService(IPostActionsRepository postActionsRepository)
        {
            _postActionsRepository = postActionsRepository;
        }

        public async Task AddLikeAsync(PostLikeDTO like)
        {
            try
            {
                await _postActionsRepository.AddLikeAsync(like);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SharePostAsync(PostShareDTO share)
        {
            await _postActionsRepository.SharePostAsync(share);
        }
        
    }
}
