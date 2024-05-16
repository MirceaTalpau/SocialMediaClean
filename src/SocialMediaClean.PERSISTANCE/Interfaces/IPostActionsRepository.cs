using LinkedFit.DOMAIN.Models.DTOs.PostActions;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IPostActionsRepository
    {
        Task AddLikeAsync(PostLikeDTO like);
        Task SharePostAsync(PostShareDTO share);
    }
}