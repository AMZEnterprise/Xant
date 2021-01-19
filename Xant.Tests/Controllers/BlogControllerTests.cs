using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Controllers;
using Xant.MVC.Models;
using Xant.MVC.Models.ViewModels;
using PostCommentFormViewModel = Xant.MVC.Models.ViewModels.PostCommentFormViewModel;

namespace Xant.Tests.Controllers
{
    [TestFixture]
    public class BlogControllerTests
    {
        private BlogController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _accessor;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _accessor = new Mock<IHttpContextAccessor>();
            _controller = new BlogController(_unitOfWork.Object, _mapper.Object, _accessor.Object);
        }

        [Test]
        public async Task Post_PostIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Post)null);

            var result = await _controller.Post(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Post_PostIsNotNull_ReturnPostViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _mapper
                .Setup(x => x.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns(new PostViewModel());

            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetAllByPostId(It.IsAny<int>()))
                .ReturnsAsync(new List<PostComment>());

            var result = await _controller.Post(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostViewModel>();
        }

        [Test]
        public async Task CommentPost_ModelStateIsNotValid_ReturnJsonResultWithJsonResultModelAndModelStateIsNotValidStatusCode()
        {
            _controller.ModelState.AddModelError(nameof(PostCommentFormViewModel.PostId), "Error");
            var result = await _controller.CommentPost(new PostCommentFormViewModel());

            result.Should().BeOfType<JsonResult>();

            var jsonResult = result as JsonResult;
            jsonResult?.Value.Should().BeOfType<JsonResultModel>();

            if (jsonResult != null)
            {
                var jsonResultModel = jsonResult.Value as JsonResultModel;
                jsonResultModel?.StatusCode
                    .Should().Be(JsonResultStatusCode.ModelStateIsNotValid);
            }
        }

        [Test]
        public async Task CommentPost_ModelStateIsValid_ReturnJsonResultWithJsonResultModelAndSuccessStatusCode()
        {
            _mapper
                .Setup(x => x.Map<PostCommentFormViewModel, PostComment>(It.IsAny<PostCommentFormViewModel>()))
                .Returns(new PostComment());

            var connection = new Mock<ConnectionInfo>(MockBehavior.Strict);
            connection.SetupGet(c => c.RemoteIpAddress).Returns(new IPAddress(It.IsAny<long>()));

            var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
            httpContext.SetupGet(x => x.Connection).Returns(connection.Object);

            _accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            _unitOfWork
                .Setup(x => x.PostCommentRepository.Insert(It.IsAny<PostComment>()))
                .Verifiable();

            var result = await _controller.CommentPost(new PostCommentFormViewModel());
            result.Should().BeOfType<JsonResult>();

            var jsonResult = result as JsonResult;
            jsonResult?.Value.Should().BeOfType<JsonResultModel>();

            if (jsonResult != null)
            {
                var jsonResultModel = jsonResult.Value as JsonResultModel;
                jsonResultModel?.StatusCode
                    .Should().Be(JsonResultStatusCode.Success);
            }
        }
    }
}
