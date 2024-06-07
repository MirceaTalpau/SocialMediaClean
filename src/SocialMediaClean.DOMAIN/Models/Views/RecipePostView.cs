using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.DOMAIN.Models.Views
{
    public class RecipePostView
    {
        public int PostID { get; set; } = default!;
        public int AuthorID { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public string AuthorProfilePictureURL { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public int RecipeID { get; set; } = default!;
        public int StatusID { get; set; } = default!;
        public int GroupID { get; set; } = default!;
        public int SharedByID { get; set; } = default!;
        public string SharedByName { get; set; } = default!;
        public DateTime SharedAt { get; set; } = default!;
        public string GroupName { get; set; } = default!;
        public string Body { get; set; }
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
        public IEnumerable<MediaPostView> Media { get; set; } = default!;

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
        public bool CurrentUserLiked { get; set; }
    }
}
