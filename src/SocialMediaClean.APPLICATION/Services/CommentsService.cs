using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Comments;
using LinkedFit.DOMAIN.Models.Views.Comments;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentsService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentView> CreateCommentAsync(AddCommentDTO comment)
        {
            try
            {
                var commentID = await _commentRepository.CreateCommentAsync(comment);
                return await _commentRepository.GetCommentAsync(commentID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentView> GetCommentAsync(int commentID)
        {
            try
            {
                return await _commentRepository.GetCommentAsync(commentID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<CommentView>> GetCommentsAsync(int postID)
        {
            try
            {
                return await _commentRepository.GetCommentsAsync(postID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteCommentAsync(int commentID)
        {
            try
            {
                await _commentRepository.DeleteCommentAsync(commentID);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
