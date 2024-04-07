﻿using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;

namespace LinkedFit.INFRASTRUCTURE.Interfaces
{
    public interface IUploadFilesService
    {
        public Task<IEnumerable<Pictures>> UploadPicturesAsync(IEnumerable<PicturesDTO> list);

        public Task<IEnumerable<Videos>> UploadAndCompressVideosAsync(IEnumerable<VideosDTO> list);
        public Task DeleteUploadedFiles(CreateNormalPostDTO post);

    }
}
