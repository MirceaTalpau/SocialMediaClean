using LinkedFit.DOMAIN.Models.DTOs.Comments;
using LinkedFit.DOMAIN.Models.Views.Comments;

namespace LinkedFit.APPLICATION.Services
{
    public interface ICommentsService
    {
        Task<CommentView> CreateCommentAsync(AddCommentDTO comment);
        Task DeleteCommentAsync(int commentID);
        Task<IEnumerable<CommentView>> GetCommentsAsync(int postID);
    }
}