using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.DOMAIN.Models.Views
{
    public class RecipePostView
    {
        public int PostID { get; set; } = default!;
        public int AuthorID { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public int RecipeID { get; set; } = default!;
        public int StatusID { get; set; } = default!;
        public int GroupID { get; set; } = default!;
        public string GroupName { get; set; } = default!;
        public string RecipeName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Instructions { get; set; } = default!;
        public int CookingTime { get; set; } = default!;
        public int Servings { get; set; } = default!;
        public int Calories { get; set; } = default!;
        public int Fat { get; set; } = default!;
        public int Carbs { get; set; } = default!;
        public int Protein { get; set; } = default!;
        public IEnumerable<Ingredient> Ingredients { get; set;} = default!;
        public IEnumerable<Pictures> Pictures { get; set; } = default!;
        public IEnumerable<VideosDTO> Videos { get; set; } = default!;
        public IEnumerable<PicturesVideosDTO> PicturesVideos { get; set; } = default!;
    }
}
