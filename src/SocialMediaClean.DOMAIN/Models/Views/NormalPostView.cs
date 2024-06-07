namespace LinkedFit.DOMAIN.Models.Views
{
    public class NormalPostView
    {
        public int PostID { get; set; } 
        public int AuthorID { get; set; }
        public int StatusID { get; set; }
        public int GroupID { get; set; }
        public int SharedByID { get; set; }
        public string SharedByName { get; set; }
        public DateTime SharedAt { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePictureURL { get; set; }
        public string SharedByProfilePictureURL { get; set; }
        public IEnumerable<MediaPostView> Media { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
        public bool CurrentUserLiked { get; set; }
    }
}
