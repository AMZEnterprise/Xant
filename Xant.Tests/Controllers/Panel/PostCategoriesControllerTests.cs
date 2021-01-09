using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Controllers;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.Tests.Controllers.Panel
{
    [TestFixture]
    public class PostCategoriesControllerTests
    {
        private PostCategoriesController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _controller = new PostCategoriesController(_unitOfWork.Object, _mapper.Object);
        }

        [Test]
        public async Task Details_PostCategoryIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostCategory)null);

            var result = await _controller.Details(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Details_PostCategoryIsNotNull_ReturnPostCategoryFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostCategory());

            _mapper
                .Setup(x => x.Map<PostCategory, PostCategoryFormViewModel>(It.IsAny<PostCategory>()))
                .Returns(new PostCategoryFormViewModel());

            var result = await _controller.Details(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCategoryFormViewModel>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsNotValid_ReturnView()
        {
            _controller.ModelState.AddModelError(nameof(PostCategoryFormViewModel.Title), "Error");
            var result = await _controller.Create(new PostCategoryFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndPostCategoryExists_ReturnViewAndAddModelStateError()
        {
            _mapper
                .Setup(x => x.Map<PostCategoryFormViewModel, PostCategory>(It.IsAny<PostCategoryFormViewModel>()))
                .Returns(new PostCategory());

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.IsPostCategoryExists(It.IsAny<PostCategory>()))
                .ReturnsAsync(true);

            var result = await _controller.Create(new PostCategoryFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndPostCategoryDoesNotExists_ReturnRedirectToActionResult()
        {
            _mapper
                .Setup(x => x.Map<PostCategoryFormViewModel, PostCategory>(It.IsAny<PostCategoryFormViewModel>()))
                .Returns(new PostCategory());

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.IsPostCategoryExists(It.IsAny<PostCategory>()))
                .ReturnsAsync(false);

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.Insert(It.IsAny<PostCategory>()))
                .Verifiable();

            var result = await _controller.Create(new PostCategoryFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Edit_PostCategoryIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostCategory)null);

            var result = await _controller.Edit(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Edit_PostCategoryIsNotNull_ReturnPostCategoryFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostCategory());

            _mapper
                .Setup(x => x.Map<PostCategory, PostCategoryFormViewModel>(It.IsAny<PostCategory>()))
                .Returns(new PostCategoryFormViewModel());

            var result = await _controller.Edit(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCategoryFormViewModel>();
        }

        [Test]
        public async Task EditPost_IdAndPostCategoryFormViewModelIdAreNotEqual_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError(nameof(PostCategoryFormViewModel.Title), "Error");
            var result = await _controller.Edit(1, new PostCategoryFormViewModel() { Id = 2 });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsNotValid_ReturnView()
        {
            _controller.ModelState.AddModelError(nameof(PostCategoryFormViewModel.Title), "Error");
            var result = await _controller.Edit(1, new PostCategoryFormViewModel() { Id = 1 });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndPostCategoryExists_ReturnViewAndAddModelStateError()
        {
            _mapper
                .Setup(x => x.Map<PostCategoryFormViewModel, PostCategory>(It.IsAny<PostCategoryFormViewModel>()))
                .Returns(new PostCategory());

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.IsPostCategoryExists(It.IsAny<PostCategory>()))
                .ReturnsAsync(true);

            var result = await _controller.Edit(1, new PostCategoryFormViewModel() { Id = 1 });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndPostCategoryDoesNotExists_ReturnRedirectToActionResult()
        {
            _mapper
                .Setup(x => x.Map<PostCategoryFormViewModel, PostCategory>(It.IsAny<PostCategoryFormViewModel>()))
                .Returns(new PostCategory());

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.IsPostCategoryExists(It.IsAny<PostCategory>()))
                .ReturnsAsync(false);

            _unitOfWork
                .Setup(x => x.PostCategoryRepository.Insert(It.IsAny<PostCategory>()))
                .Verifiable();

            var result = await _controller.Edit(1, new PostCategoryFormViewModel() { Id = 1 });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Delete_PostCategoryIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostCategory)null);

            var result = await _controller.Delete(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_PostCategoryIsNotNull_ReturnPostCategoryFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostCategory());

            _mapper
                .Setup(x => x.Map<PostCategory, PostCategoryFormViewModel>(It.IsAny<PostCategory>()))
                .Returns(new PostCategoryFormViewModel());

            var result = await _controller.Delete(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCategoryFormViewModel>();
        }

        [Test]
        public async Task DeleteConfirmed_DeletePostCategory_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.PostCategoryRepository.Delete(It.IsAny<int>()))
                .Verifiable();

            var result = await _controller.DeleteConfirmed(It.IsAny<int>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
