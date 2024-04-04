namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class CreateRecipePostDTO : CreateNormalPostDTO
    {
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

    }
}
