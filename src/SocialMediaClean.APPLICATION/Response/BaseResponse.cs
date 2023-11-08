namespace SocialMediaClean.APPLICATION.Response
{
    public class BaseResponse
    {
        
        public string Message = string.Empty;
        public bool Success { get; set; } = false;
        public int Code { get; set; } = 400;
        public IDictionary<string, string> ErrorMessages { get; set; } = new Dictionary<string, string>();
    }
}
