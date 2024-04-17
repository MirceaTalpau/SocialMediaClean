namespace LinkedFit.DOMAIN.Models.Views
{
    public class MediaPostView
    {
        public int PostID { get; set; }
        DateTime PictureCreatedAt { get; set; }
        string PictureURI { get; set; }
        DateTime VideoCreatedAt { get; set; }
        string VideoURI { get; set; }
    }
}
