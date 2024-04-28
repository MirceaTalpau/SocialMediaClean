using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Comments;
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

        public async Task CreateCommentAsync(AddCommentDTO comment)
        {
            try
            {
                await _commentRepository.CreateCommentAsync(comment);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
