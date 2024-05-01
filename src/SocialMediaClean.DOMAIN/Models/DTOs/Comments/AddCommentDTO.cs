namespace LinkedFit.DOMAIN.Models.DTOs.Comments
{
    public class AddCommentDTO
    {
        public int PostID { get; set; }
        public int AuthorID { get; set; }
        public int ParentID { get; set; } = -1;
        public string Body { get; set; }
    }
}
