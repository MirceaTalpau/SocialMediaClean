using LinkedFit.DOMAIN.Models.DTOs.PostActions;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IPostActionsService
    {
        Task AddLikeAsync(PostLikeDTO like);
        Task SharePostAsync(PostShareDTO share);

    }
}