using Dapper;
using LinkedFit.DOMAIN.Models.Entities.NewFolder;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class RecipesRepository
    {
        private readonly IDbConnectionFactory _db;
        public RecipesRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Recipe>> GetMyRecipes(int AuthorID)
        {
            using(var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var query = "SELECT * FROM Recipes WHERE AuthorID = @AuthorID";
                    var recipes = await conn.QueryAsync<Recipe>(query, new { AuthorID });
                    foreach(var recipe in recipes)
                    {
                        var ingredientsQuery = "SELECT * FROM Ingredients WHERE RecipeID = @RecipeID";
                        var ingredients = await conn.QueryAsync<Ingredient>(ingredientsQuery, new { RecipeID = recipe.ID });
                        recipe.Ingredients = ingredients;
                    }
                    return recipes;
                }
                catch(Exception)
                {
                    throw;
                }
                
            }
        }
    }
}
