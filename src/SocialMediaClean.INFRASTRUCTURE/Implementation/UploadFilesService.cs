using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.INFRASTRUCTURE.Interfaces;
using Microsoft.AspNetCore.Http;
using Xabe.FFmpeg;

namespace LinkedFit.INFRASTRUCTURE.Implementation
{
    public class UploadFilesService : IUploadFilesService
    {

        public async Task DeleteUploadedFiles(CreateNormalPostDTO post)
        {
            if (post.Pictures != null)
            {
                foreach (var picture in post.Pictures)
                {
                    if (File.Exists(picture.PictureURI))
                    {
                        File.Delete(picture.PictureURI);
                    }
                }
            }
            if (post.Videos != null)
            {
                foreach (var video in post.Videos)
                {
                    if (File.Exists(video.VideoURI))
                    {
                        File.Delete(video.VideoURI);
                    }
                }
            }
        }
        public async Task<IEnumerable<Pictures>> UploadPicturesAsync(IEnumerable<PicturesDTO> list)
        {
            List<Pictures> Pictures = new List<Pictures>();
            foreach (var file in list)
            {
                if (file.PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(file.PictureFile.FileName);
                    var baseFileName = Path.GetFileNameWithoutExtension(fileName);
                    var fileExtension = Path.GetExtension(fileName);

                    var counter = 1;
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var newFileName = $"{baseFileName}_{timestamp}_{counter}{fileExtension}";
                    var filePath = Path.Combine(@"C:\Storage\Pictures", newFileName);

                    // Check if file with the same name already exists
                    while (File.Exists(filePath))
                    {
                        counter++;
                        newFileName = $"{baseFileName}_{timestamp}_{counter}{fileExtension}";
                        filePath = Path.Combine(@"C:\Storage\Pictures", newFileName);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.PictureFile.CopyToAsync(fileStream);
                    }
                    var fullPath = filePath; // Using the same path for PictureURI
                    Pictures.Add(new Pictures // Creating and adding Pictures object directly
                    {
                        PictureURI = fullPath,
                        CreatedAt = file.CreatedAt
                    });
                }
            }
            return Pictures;
        }
        public async Task<IEnumerable<Videos>> UploadAndCompressVideosAsync(IEnumerable<VideosDTO> list)
        {
            List<Videos> compressedVideos = new List<Videos>();

            foreach (var file in list)
            {
                if (file.VideoFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.VideoFile.FileName);
                    var fileExtension = Path.GetExtension(file.VideoFile.FileName);
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var newFileName = $"{fileName}_{timestamp}{fileExtension}";
                    var filePath = Path.Combine(@"C:\Storage\Videos", newFileName);
                    var compressedFilePath = Path.Combine(@"C:\Storage\Videos", "compressed_" + newFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.VideoFile.CopyToAsync(fileStream);
                    }

                    FFmpeg.SetExecutablesPath(@"C:\Program Files\ffmpeg\bin");
                    var info = await FFmpeg.GetMediaInfo(filePath);
                    var videoStream = info.VideoStreams.First()
                        .SetCodec(VideoCodec.h264)
                        .SetSize(VideoSize.Hd480);

                    await FFmpeg.Conversions.New()
                        .AddStream(videoStream)
                        .SetOutput(compressedFilePath)
                        .Start();

                    // Check if compressed file already exists and increment filename
                    int fileIncrement = 1;
                    while (File.Exists(compressedFilePath))
                    {
                        compressedFilePath = Path.Combine(@"C:\Storage\Videos", $"compressed_{fileName}_{timestamp}_{fileIncrement}{fileExtension}");
                        fileIncrement++;
                    }

                    Videos video = new Videos
                    {
                        VideoURI = compressedFilePath,
                        CreatedAt = file.CreatedAt
                    };
                    compressedVideos.Add(video);
                    // Delete the original video file
                    if (File.Exists(info.Path))
                    {
                        File.Delete(info.Path);
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("$File path " + filePath);
                    }
                }

            }

            return compressedVideos;
        }


    }
}
