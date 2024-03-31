using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using Microsoft.AspNetCore.Http;

namespace LinkedFit.INFRASTRUCTURE.Implementation
{
    public class UploadFiles
    {
        public async Task<IEnumerable<Pictures>> UploadPicturesAsync(IEnumerable<PicturesDTO> list)
        {
            IEnumerable<Pictures> Pictures = new List<Pictures>();
            foreach (var file in list)
            {
                if (file.PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(file.PictureFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.PictureFile.CopyToAsync(fileStream);
                    }
                    var fullPath = Path.Combine("uploads", "images", fileName);
                    Pictures picture = new Pictures
                    {
                        PictureURI = fullPath,
                        CreatedAt = file.CreatedAt
                    };
                    Pictures.Append(picture);
                }
            }
            return Pictures;
        }
    }
}
