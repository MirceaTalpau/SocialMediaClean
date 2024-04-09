namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateProgressPostDTO : CreateNormalPostDTO
    {
        public string BeforeWeight { get; set; }
        public string AfterWeight { get; set; }
        public IFormFile BeforePicture { get; set; }
        public string BeforePictureUri { get; set; }
        public IFormFile AfterPicture { get; set; }
        public string AfterPictureUri { get; set; }
        public string BeforeDate { get; set; }
        public string AfterDate { get; set; }
    }
}
