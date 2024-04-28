using LinkedFit.APPLICATION.Services;
using LinkedFit.DOMAIN.Models.DTOs.PostActions;
using LinkedFit.PERSISTANCE.Repositories;

namespace LinkedFit.APPLICATION.Contracts
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
    }
}
