namespace LinkedFit.DOMAIN.Models.Views
{
    public class NormalPostView
    {
        public int PostID { get; set; } 
        public int AuthorID { get; set; }
        public int StatusID { get; set; }
        public int GroupID { get; set; }
        public int SharedByID { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}
