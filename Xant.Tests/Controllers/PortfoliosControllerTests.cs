using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Controllers;
using Xant.MVC.Models.ViewModels;

namespace Xant.Tests.Controllers
{
    [TestFixture]
    public class PortfoliosControllerTests
    {
        private PortfoliosController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _controller = new PortfoliosController(_unitOfWork.Object, _mapper.Object);
        }

        [Test]
        public async Task Portfolio_PostIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Post)null);

            var result = await _controller.Portfolio(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Portfolio_PostIsNotNull_ReturnPostViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _mapper
                .Setup(x => x.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns(new PostViewModel());

            var result = await _controller.Portfolio(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostViewModel>();
        }
    }
}
