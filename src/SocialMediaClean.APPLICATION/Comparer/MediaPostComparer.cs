using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.APPLICATION.Comparer
{
    public class MediaPostComparer : IComparer<MediaPostView>
    {
        public int Compare(MediaPostView x, MediaPostView y)
        {
            // Compare based on PictureCreatedAt
            int pictureComparison = DateTime.Compare(x.PictureCreatedAt, y.PictureCreatedAt);
            if (pictureComparison != 0)
            {
                return pictureComparison;
            }

            // If PictureCreatedAt is equal, compare based on VideoCreatedAt
            return DateTime.Compare(x.VideoCreatedAt, y.VideoCreatedAt);
        }
    }
}
