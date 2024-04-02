using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateNormalPostDTO
    {
        public int AuthorID { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusID { get; set; }
        public IFormFile PictureDTOs { get; set; }
        public IEnumerable<PicturesDTO> PicturesDTO { get; set; } = default!;
        public IEnumerable<VideosDTO> VideosDTO { get; set; } = default!;
        public IEnumerable<Pictures> Pictures { get; set; } = default!;
        public IEnumerable<Videos> Videos { get; set; } = default!;

    }
}
