namespace LinkedFit.DOMAIN.Models.Views.Comments
{
    public class CommentView
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int AuthorID { get; set; }
        public int ParentID { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePictureURL { get; set; }
    }
}
