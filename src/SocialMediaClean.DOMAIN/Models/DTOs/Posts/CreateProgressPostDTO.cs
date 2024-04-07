namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateProgressPostDTO : CreateNormalPostDTO
    {
        public string BeforeWeight { get; set; }
        public string AfterWeight { get; set; }
        public string BeforePicture { get; set; }
        public string AfterPicture { get; set; }
        public string BeforeDate { get; set; }
        public string AfterDate { get; set; }
    }
}
