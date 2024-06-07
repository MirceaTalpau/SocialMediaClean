namespace LinkedFit.DOMAIN.Models.Entities.Group
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
