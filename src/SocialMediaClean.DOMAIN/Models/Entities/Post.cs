namespace LinkedFit.DOMAIN.Models.Entities
{
    public class Post
    {
        int ID { get; set; }
        int AuthorID { get; set; }
        int SharedByID { get; set; }
        int  StatusID { get; set; }
        int GroupID { get; set; }
        string Body { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
