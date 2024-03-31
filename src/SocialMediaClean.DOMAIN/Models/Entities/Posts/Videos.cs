namespace LinkedFit.DOMAIN.Models.Entities.Posts
{
    public class Videos
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string VideoURI { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
