namespace SocialMediaClean.APPLICATION.Response
{
    public class LoginResponse : Response
    {
        public bool Success { get; set; } = false;
        public string Token { get; set; } = string.Empty;
    }
}
