using AutoFixture;
using AutoFixture.AutoMoq;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.PERSISTANCE.Interfaces;
using LinkedFit.PERSISTANCE.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedFit.UnitTests
{
    public class PostRepositoryTests
    {
        private readonly Mock<DbConnectionFactory> _db;
        private readonly IConfiguration _config;
        private readonly Mock<IPostRepository> _postRepo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Fixture _fixture;
        public PostRepositoryTests()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _db = new Mock<DbConnectionFactory>(_config);
            _postRepo = new Mock<IPostRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task CreateNormalPost_HappyFlow()
        {
            //ARRANGE
            var post = _fixture.Build<CreateNormalPostDTO>()
                .With(x => x.AuthorID, 1020)
                .With(x => x.Body, "Test")
                .With(x => x.CreatedAt, DateTime.Now)
                .With(x => x.StatusID, 1)
                .Create();
            _postRepo.Setup(x => x.CreatePostNormalAsync(post)).ReturnsAsync(50);
            _unitOfWork.Setup(x => x.Commit());
            var postRepository = new PostRepository(_db.Object);
            //ACT
            var result = await postRepository.CreatePostNormalAsync(post);
            //ASSERT
            result.ShouldBeOfType<int>();

        }
    }
}
