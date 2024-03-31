using AutoFixture;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.PERSISTANCE.Interfaces;
using LinkedFit.PERSISTANCE.Repositories;
using Microsoft.Extensions.Configuration;
using Shouldly;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedFit.IntegrationTests.Post
{
    public class PostRepositoryTest
    {
        private readonly IDbConnectionFactory _db;
        private readonly IPostRepository _postRepository;
        private readonly IConfiguration _config;
        private readonly IFixture _fixture;


        public PostRepositoryTest()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _db = new DbConnectionFactory(_config);
            _postRepository = new PostRepository(_db);
        }

        //[Fact]
        //public async Task CreatePostNormalAsync_WhenPostIsValid_ReturnsPostID()
        //{
        //    //ARRANGE
        //    var post = new CreateNormalPostDTO
        //    {
        //        AuthorID = 1020,
        //        Body = "Test",
        //        StatusID = 1,
        //        pictures = new List<Pictures>
        //        {
        //            new Pictures
        //            {
        //                PictureURI = "test",
        //                CreatedAt = DateTime.Now,
        //            }
        //        },
        //        videos = new List<Videos>
        //        {
        //            new Videos
        //            {
        //                VideoURI = "test",
        //                CreatedAt = DateTime.Now
        //            }
        //        }
        //    };
        //    //ACT
        //    var response = await _postRepository.CreatePostNormalAsync(post);
        //    //ASSERT
        //    response.ShouldBeGreaterThan(0);
        //}
        //[Fact]
        //public async Task CreatePostNormalAsync_WhenPostIsNotValid_ReturnsError()
        //{
        //    //ARRANGE
        //    var post = new CreateNormalPostDTO
        //    {
        //        AuthorID = 1020,
        //        StatusID = 1,
        //        pictures = new List<Pictures>
        //        {
        //            new Pictures
        //            {
        //                PictureURI = "test",
        //                CreatedAt = DateTime.Now
        //            }
        //        },
        //        videos = new List<Videos>
        //        {
        //            new Videos
        //            {
        //                VideoURI = "test",
        //                CreatedAt = DateTime.Now
        //            }
        //        }
        //    };
        //    //ACT
        //    var response = await _postRepository.CreatePostNormalAsync(post);
        //    //ASSERT
        //    response.ShouldBe(0);
        //}
    }
}
