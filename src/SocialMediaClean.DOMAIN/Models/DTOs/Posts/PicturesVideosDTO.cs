namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class PicturesVideosDTO
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string PictureURI { get; set; } = default!;
        public string VideoURI { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}

