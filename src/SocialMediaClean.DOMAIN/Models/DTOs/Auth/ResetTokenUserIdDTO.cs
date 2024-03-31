namespace LinkedFit.DOMAIN.Models.DTOs.Auth
{
    public class ResetTokenUserIdDTO
    {
        public int UserId { get; set; } = 0;
        public bool IsValid { get; set; } = false;
    }
}
