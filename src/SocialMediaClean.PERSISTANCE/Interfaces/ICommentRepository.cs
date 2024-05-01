using LinkedFit.DOMAIN.Models.DTOs.Comments;
using LinkedFit.DOMAIN.Models.Views.Comments;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface ICommentRepository
    {
        Task<int> CreateCommentAsync(AddCommentDTO comment);
        Task DeleteCommentAsync(int commentID);
        Task<CommentView> GetCommentAsync(int commentID);
        Task<IEnumerable<CommentView>> GetCommentsAsync(int postID);
    }
}