using LinkedFit.DOMAIN.Models.DTOs.PostActions;

namespace LinkedFit.APPLICATION.Services
{
    public interface IPostActionsService
    {
        Task AddLikeAsync(PostLikeDTO like);
    }
}