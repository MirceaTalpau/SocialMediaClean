namespace LinkedFit.DOMAIN.Models.Views
{
    public class MediaPostView
    {
        public int PostID { get; set; }
        public DateTime PictureCreatedAt { get; set; } = default!;
        public string PictureURI { get; set; } = default!;
        public DateTime VideoCreatedAt { get; set; } = default!;
        public string VideoURI { get; set; } = default!;
    }
}
