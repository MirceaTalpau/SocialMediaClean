using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateNormalPostDTO
    {
        public int AuthorID { get; set; }
        public string Body { get; set; }
        public string CreatedAt { get; set; }
        public string StatusID { get; set; }
        public IEnumerable<PicturesDTO> PicturesDTO { get; set; } = default!;
        public IEnumerable<VideosDTO> VideosDTO { get; set; } = default!;

    }
}
