using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Controllers;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Services.FileHandler;

namespace Xant.Tests.Controllers.Panel
{
    [TestFixture]
    public class PostsControllerTests
    {
        private PostsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IFileHandler> _fileHandler;
        private Mock<IWebHostEnvironment> _env;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _fileHandler = new Mock<IFileHandler>();
            _env = new Mock<IWebHostEnvironment>();
            _controller = new PostsController(_unitOfWork.Object, _mapper.Object, _fileHandler.Object, _env.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
                }
            };
        }

        [Test]
        public async Task Details_PostIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Post)null);

            var result = await _controller.Details(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Details_PostIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Details(It.IsAny<int>());
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task Details_PostIsNotNullAndUserIsAllowedForOperation_ReturnPostFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetAll())
                .Returns(new Mock<IQueryable<PostCategory>>().Object);

            _mapper
                .Setup(x => x.Map<Post, PostFormViewModel>(It.IsAny<Post>()))
                .Returns(new PostFormViewModel());

            var result = await _controller.Details(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostFormViewModel>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsNotValid_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetAll())
                .Returns(new Mock<IQueryable<PostCategory>>().Object);

            _controller.ModelState.AddModelError(nameof(PostFormViewModel.Title), "Error");
            var result = await _controller.Create(new PostFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndOneOrMoreFilesSent_UploadFilesAndReturnRedirectToActionResult()
        {
            _mapper
                .Setup(x => x.Map<PostFormViewModel, Post>(It.IsAny<PostFormViewModel>()))
                .Returns(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.PostRepository.Insert(It.IsAny<Post>()))
                .Verifiable();

            _controller.HttpContext.Request.Form =
                new FormCollection(
                    new Dictionary<string, StringValues>(),
                    new FormFileCollection()
                    {
                        new FormFile(It.IsAny<Stream>(),It.IsAny<long>(),
                            It.IsAny<long>(),It.IsAny<string>(),It.IsAny<string>())
                    });

            _fileHandler
                .Setup(x => x.UploadMedia(
                    It.IsAny<IFormFileCollection>(), It.IsAny<string>(),
                    It.IsAny<string>(), FileHandlerFolder.Posts))
                .Verifiable();

            var result = await _controller.Create(new PostFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Edit_PostIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Post)null);

            var result = await _controller.Edit(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Edit_PostIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Edit(It.IsAny<int>());
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task Edit_PostIsNotNullAndUserIsAllowedForOperation_ReturnPostFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetAll())
                .Returns(new Mock<IQueryable<PostCategory>>().Object);

            _mapper
                .Setup(x => x.Map<Post, PostFormViewModel>(It.IsAny<Post>()))
                .Returns(new PostFormViewModel());

            var result = await _controller.Edit(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull().And
                .BeOfType<PostFormViewModel>();
        }

        [Test]
        public async Task EditPost_IdAndPostFormViewModelIdAreNotEqual_ReturnBadRequestResult()
        {
            _controller.ModelState.AddModelError(nameof(PostFormViewModel.Title), "Error");
            var result = await _controller.Edit(1, new PostFormViewModel() { Id = 2 });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetAll())
                .Returns(new Mock<IQueryable<PostCategory>>().Object);

            _controller.ModelState.AddModelError(nameof(PostFormViewModel.Title), "Error");
            var result = await _controller.Edit(1, new PostFormViewModel() { Id = 1 });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Edit(1, new PostFormViewModel() { Id = 1 });
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUserIsAllowedForOperation_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostFormViewModel, Post>(It.IsAny<PostFormViewModel>()))
                .Returns(new Post());

            _unitOfWork
                .Setup(x => x.PostRepository.Update(It.IsAny<Post>()))
                .Verifiable();

            _controller.HttpContext.Request.Form =
                new FormCollection(
                    new Dictionary<string, StringValues>(), new FormFileCollection());

            var result = await _controller.Edit(1, new PostFormViewModel() { Id = 1 });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Delete_PostIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Post)null);

            var result = await _controller.Delete(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_PostIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Delete(It.IsAny<int>());
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task Delete_PostIsNotNullAndUserIsAllowedForOperation_ReturnPostFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Post());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetAll())
                .Returns(new Mock<IQueryable<PostCategory>>().Object);

            _mapper
                .Setup(x => x.Map<Post, PostFormViewModel>(It.IsAny<Post>()))
                .Returns(new PostFormViewModel());

            var result = await _controller.Delete(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostFormViewModel>();
        }

        [Test]
        public async Task DeleteConfirmed_UserIdIsNull_ReturnBadRequestResult()
        {
            var result = await _controller.DeleteConfirmed(
                It.IsAny<int>(),null, "filesPathGuid");
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task DeleteConfirmed_filesPathGuidIsNull_ReturnBadRequestResult()
        {
            var result = await _controller.DeleteConfirmed(
                It.IsAny<int>(), "UserId", null);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task DeleteConfirmed_UserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteConfirmed(
                It.IsAny<int>(), "UserId", "filesPathGuid");
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task DeleteConfirmed_UserIsAllowedForOperation_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.PostRepository.Delete(It.IsAny<int>()))
                .Verifiable();

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteConfirmed(
                It.IsAny<int>(), "UserId", "filesPathGuid");
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
