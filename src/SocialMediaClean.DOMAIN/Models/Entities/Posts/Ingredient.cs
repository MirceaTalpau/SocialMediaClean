namespace LinkedFit.DOMAIN.Models.Entities.Posts
{
    public class Ingredient
    {
        public int ID { get; set; } = default!;
        public int RecipeID { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public string Unit { get; set; } = default!;
    }
}
