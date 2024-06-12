using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.DOMAIN.Models.Entities.NewFolder
{
    public class Recipe
    {
        public int ID { get; set; } = default!;
        public int AuthorID { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Instructions { get; set; } = default!;
        public int CookingTime { get; set; } = default!;
        public int Servings { get; set; } = default!;
        public int Calories { get; set; } = default!;
        public int Protein { get; set; } = default!;
        public int Carbs { get; set; } = default!;
        public int Fat { get; set; } = default!;
        public IEnumerable<Ingredient> Ingredients { get; set; } = default!;

    }
}
