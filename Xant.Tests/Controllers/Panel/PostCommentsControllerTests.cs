using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Controllers;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.Tests.Controllers.Panel
{
    [TestFixture]
    public class PostCommentsControllerTests
    {
        private PostCommentsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _accessor;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _accessor = new Mock<IHttpContextAccessor>();
            _controller = new PostCommentsController(_unitOfWork.Object, _mapper.Object, _accessor.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
                }
            };
        }

        [Test]
        public async Task Details_PostCommentIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostComment)null);

            var result = await _controller.Details(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Details_PostCommentIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

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
        public async Task
            Details_PostCommentIsNotNullAndUserIsAllowedForOperation_ReturnPostCommentFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostComment, PostCommentFormViewModel>(It.IsAny<PostComment>()))
                .Returns(new PostCommentFormViewModel());

            var result = await _controller.Details(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCommentFormViewModel>();
        }

        [Test]
        public async Task Reply_ParentPostCommentIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostComment)null);

            var result = await _controller.Reply(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Reply_ParentPostCommentIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Reply(It.IsAny<int>());
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task
            Reply_ParentPostCommentIsNotNullAndUserIsAllowedForOperation_ReturnPostCommentReplyFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostComment, PostCommentReplyFormViewModel>(It.IsAny<PostComment>()))
                .Returns(new PostCommentReplyFormViewModel());

            var result = await _controller.Reply(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCommentReplyFormViewModel>();
        }

        [Test]
        public async Task ReplyPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(PostCommentReplyFormViewModel.PostId), "Error");
            var result = await _controller.Reply(new PostCommentReplyFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ReplyPost_ModelStateIsValidAndParentPostCommentIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostComment)null);

            var result = await _controller.Reply(new PostCommentReplyFormViewModel());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task
            ReplyPost_ModelStateIsValidAndParentPostCommentIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Reply(new PostCommentReplyFormViewModel());
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task
            ReplyPost_ModelStateIsValidAndParentPostCommentIsNotNullAndUserIsAllowedForOperation_ReturnPostCommentReplyFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostCommentReplyFormViewModel, PostComment>
                    (It.IsAny<PostCommentReplyFormViewModel>()))
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

            var result = await _controller.Reply(new PostCommentReplyFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Edit_PostCommentIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostComment)null);

            var result = await _controller.Edit(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Edit_PostCommentIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

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
        public async Task
            Edit_PostCommentIsNotNullAndUserIsAllowedForOperation_ReturnPostCommentFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostComment, PostCommentFormViewModel>(It.IsAny<PostComment>()))
                .Returns(new PostCommentFormViewModel());

            var result = await _controller.Edit(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCommentFormViewModel>();
        }

        [Test]
        public async Task EditPost_IdAndPostCommentFormViewModelIdAreNotEqual_ReturnBadRequestResult()
        {
            var result = await _controller.Edit(1, new PostCommentFormViewModel() { Id = 2 });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(PostFormViewModel.Title), "Error");
            var result = await _controller.Edit(1, new PostCommentFormViewModel() { Id = 1 });
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

            var result = await _controller.Edit(1, new PostCommentFormViewModel() { Id = 1 });
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
                .Setup(x => x.Map<PostCommentFormViewModel, PostComment>(It.IsAny<PostCommentFormViewModel>()))
                .Returns(new PostComment());

            _unitOfWork
                .Setup(x => x.PostCommentRepository.UpdateChildrenStatus(It.IsAny<int>(), It.IsAny<PostCommentStatus>()))
                .Verifiable();

            _unitOfWork
                .Setup(x => x.PostCommentRepository.Update(It.IsAny<PostComment>()))
                .Verifiable();

            var result = await _controller.Edit(1, new PostCommentFormViewModel() { Id = 1 });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Delete_PostCommentIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((PostComment)null);

            var result = await _controller.Delete(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_PostCommentIsNotNullAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

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
        public async Task Delete_PostCommentIsNotNullAndUserIsAllowedForOperation_ReturnPostCommentFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.PostCommentRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PostComment());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<PostComment, PostCommentFormViewModel>(It.IsAny<PostComment>()))
                .Returns(new PostCommentFormViewModel());

            var result = await _controller.Delete(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PostCommentFormViewModel>();
        }

        [Test]
        public async Task DeleteConfirmed_UserIdIsNull_ReturnBadRequestResult()
        {
            var result = await _controller.DeleteConfirmed(It.IsAny<int>(), null);
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

            var result = await _controller.DeleteConfirmed(It.IsAny<int>(), "UserId");
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

            _unitOfWork
                .Setup(x => x.PostCommentRepository.DeleteChildren(It.IsAny<int>()))
                .Verifiable();

            _unitOfWork
                .Setup(x => x.PostCommentRepository.Delete(It.IsAny<int>()))
                .Verifiable();

            var result = await _controller.DeleteConfirmed(It.IsAny<int>(), "UserId");
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}