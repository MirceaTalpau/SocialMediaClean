using Microsoft.AspNetCore.Routing;

namespace LinkedFit.DOMAIN.Models.DTOs.Posts
{
    public class UnionPost
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public int? SharedByID { get; set; }
        public int StatusID { get; set; }
        public int? GroupID { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public string? CookingTime { get; set; }
        SELECT 'Posts' AS TableName, [ID], [AuthorID], [SharedByID], [StatusID], [GroupID], [Body], [CreatedAt], NULL AS PostID, NULL AS Name, NULL AS Description, NULL AS Instructions, NULL AS CookingTime, NULL AS Servings, NULL AS Calories, NULL AS Protein, NULL AS Carbs, NULL AS Fat, NULL AS BeforeWeight, NULL AS AfterWeight, NULL AS BeforePictureUri, NULL AS AfterPictureUri, NULL AS BeforeDate, NULL AS AfterDate
FROM[SocialMediaClean].[dbo].[Posts]

        UNION ALL

SELECT 'Recipe' AS TableName, NULL AS ID, NULL AS AuthorID, NULL AS SharedByID, NULL AS StatusID, NULL AS GroupID, NULL AS Body, NULL AS CreatedAt, [PostID], [Name], [Description], [Instructions], [CookingTime], [Servings], [Calories], [Protein], [Carbs], [Fat], NULL AS BeforeWeight, NULL AS AfterWeight, NULL AS BeforePictureUri, NULL AS AfterPictureUri, NULL AS BeforeDate, NULL AS AfterDate
FROM[SocialMediaClean].[dbo].[Recipe]

        UNION ALL

SELECT 'PROGRESS' AS TableName, NULL AS ID, NULL AS AuthorID, NULL AS SharedByID, NULL AS StatusID, NULL AS GroupID, NULL AS Body, NULL AS CreatedAt, [PostID], NULL AS Name, NULL AS Description, NULL AS Instructions, NULL AS CookingTime, NULL AS Servings, NULL AS Calories, NULL AS Protein, NULL AS Carbs, NULL AS Fat, [BeforeWeight], [AfterWeight], [BeforePictureUri], [AfterPictureUri], [BeforeDate], [AfterDate]
FROM[SocialMediaClean].[dbo].[PROGRESS];


    }
}
