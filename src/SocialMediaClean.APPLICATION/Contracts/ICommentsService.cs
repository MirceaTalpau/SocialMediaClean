using LinkedFit.DOMAIN.Models.DTOs.Comments;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface ICommentsService
    {
        Task CreateCommentAsync(AddCommentDTO comment);
    }
}