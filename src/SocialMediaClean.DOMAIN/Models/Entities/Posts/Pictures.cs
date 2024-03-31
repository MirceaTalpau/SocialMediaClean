namespace LinkedFit.DOMAIN.Models.Entities.Posts
{
    public class Pictures
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string PictureURI { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
