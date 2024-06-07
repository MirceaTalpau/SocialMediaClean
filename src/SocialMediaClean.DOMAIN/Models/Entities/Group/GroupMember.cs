namespace LinkedFit.DOMAIN.Models.Entities.Group
{
    public class GroupMember
    {
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
