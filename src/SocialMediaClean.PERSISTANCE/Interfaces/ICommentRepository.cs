using LinkedFit.DOMAIN.Models.DTOs.Comments;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(AddCommentDTO comment);
    }
}