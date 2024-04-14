using LinkedFit.DOMAIN.Models.DTOs.Posts;

namespace LinkedFit.DOMAIN.Models.Views
{
    public class PostView
    {
        public int AuthorID { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusID { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Instructions { get; set; } = default!;
        public string CookingTime { get; set; } = default!;
        public string Servings { get; set; } = default!;
        public IEnumerable<IngredientDTO> Ingredients { get; set; } = default!;
        public string Calories { get; set; } = default!;
        public string Protein { get; set; } = default!;
        public string Carbs { get; set; } = default!;
        public string Fat { get; set; } = default!;
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
