namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateProgressPostDTO : CreateNormalPostDTO
    {
        public string BeforeWeight { get; set; } = default!;
        public string AfterWeight { get; set; } = default!;
        public IFormFile BeforePicture { get; set; } = default!;
        public string BeforePictureUri { get; set; } = default!;
        public IFormFile AfterPicture { get; set; } = default!;
        public string AfterPictureUri { get; set; } = default!;
        public string BeforeDate { get; set; } = default!;
        public string AfterDate { get; set; } = default!;
    }
}
