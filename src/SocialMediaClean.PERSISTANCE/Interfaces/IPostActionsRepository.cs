using LinkedFit.DOMAIN.Models.DTOs.PostActions;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public interface IPostActionsRepository
    {
        Task AddLikeAsync(PostLikeDTO like);
    }
}