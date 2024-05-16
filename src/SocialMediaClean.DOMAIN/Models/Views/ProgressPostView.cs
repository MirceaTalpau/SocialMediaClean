namespace LinkedFit.DOMAIN.Models.Views
{
    public class ProgressPostView
    {
        public int PostID { get; set; } = default(int);
        public int AuthorID { get; set; } = default!;
        public int ProgressID { get; set; } = default!;
        public int StatusID { get; set; } = default!;
        public int GroupID { get; set; } = default!;
        public int SharedByID { get; set; } = default!;
        public string Body { get; set; } 
        public DateTime CreatedAt { get; set; } =default!;
        public double BeforeWeight { get; set; } = default!;
        public double AfterWeight { get; set; } = default!;
        public string BeforePictureURI { get; set; } = default!;
        public string AfterPictureURI { get; set; } = default!;
        public DateTime BeforeDate { get; set; } = default!;
        public DateTime AfterDate { get; set; } = default!;
        public string ProfilePictureURL { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public IEnumerable<MediaPostView> Media { get; set; } = default!;
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
        public bool CurrentUserLiked { get; set; }
    }
}
