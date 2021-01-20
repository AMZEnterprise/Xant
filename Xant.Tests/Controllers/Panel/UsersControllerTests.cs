using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Controllers;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Services.EmailSender;
using Xant.MVC.Services.FileHandler;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Xant.Tests.Controllers.Panel
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IEmailSender> _emailSender;
        private Mock<IFileHandler> _fileHandler;
        private Mock<IWebHostEnvironment> _env;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _emailSender = new Mock<IEmailSender>();
            _fileHandler = new Mock<IFileHandler>();
            _env = new Mock<IWebHostEnvironment>();
            _controller = new UsersController(_unitOfWork.Object, _mapper.Object, _emailSender.Object, _fileHandler.Object, _env.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
                }
            };
        }

        [Test]
        public async Task Details_UserIdIsNull_ShouldReturnBadRequestResult()
        {
            var result = await _controller.Details(null);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Details_UserIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.Details("UserId");
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Details_UserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Details("UserId");
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task Details_UserIsAllowedForOperation_ReturnUserFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<User, UserFormViewModel>(It.IsAny<User>()))
                .Returns(new UserFormViewModel());

            var result = await _controller.Details("UserId");
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UserFormViewModel>();
        }

        [Test]
        public async Task CreatePost_UserRoleIsSuperAdmin_ShouldReturnBadRequest()
        {
            var result = await _controller.Create(new UserCreateFormViewModel() { Role = UserRoleEnumViewModel.SuperAdmin });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(UserCreateFormViewModel.FirstName), "Error");
            var result = await _controller.Create(new UserCreateFormViewModel() { Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndAddUserToIdentityFailed_ShouldReturnViewResult()
        {
            _mapper
                .Setup(x => x.Map<UserCreateFormViewModel, User>(It.IsAny<UserCreateFormViewModel>()))
                .Returns(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.Insert(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.Create(new UserCreateFormViewModel() { Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndAddUserIdentitySucceedAndAddUserToRoleFailed_ShouldReturnViewResult()
        {
            _mapper
                .Setup(x => x.Map<UserCreateFormViewModel, User>(It.IsAny<UserCreateFormViewModel>()))
                .Returns(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.Insert(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWork
                .Setup(x => x.UserRepository.AddToRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.Create(new UserCreateFormViewModel() { Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CreatePost_ModelStateIsValidAndAddUserIdentitySucceedAndAddUserToRoleSucceed_ShouldRedirectToAction()
        {
            _mapper
                .Setup(x => x.Map<UserCreateFormViewModel, User>(It.IsAny<UserCreateFormViewModel>()))
                .Returns(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.Insert(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWork
                .Setup(x => x.UserRepository.AddToRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _controller.HttpContext.Request.Form =
                new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection());

            var result = await _controller.Create(new UserCreateFormViewModel() { Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Edit_UserIdIsNull_ShouldReturnBadRequestResult()
        {
            var result = await _controller.Edit(null);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Edit_UserIsNull_ReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.Edit("UserId");
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Edit_UserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Edit("UserId");
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task Edit_UserIsAllowedForOperation_ReturnUserFormViewModelToViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.GetByClaimsPrincipal(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<User, UserFormViewModel>(It.IsAny<User>()))
                .Returns(new UserFormViewModel());

            var result = await _controller.Edit("UserId");
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult?.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UserFormViewModel>();
        }

        [Test]
        public async Task EditPost_IdAndUserFormViewModelIdAreNotEqual_ReturnBadRequestResult()
        {
            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "WrongUserId" });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_UserIsNull_ShouldReturnNotFoundResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId" });
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task EditPost_UserRoleIsSuperAdmin_ShouldReturnBadRequest()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.SuperAdmin });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _controller.ModelState.AddModelError(nameof(UserFormViewModel.Id), "Error");
            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUserIsNotAllowedForOperation_ReturnUnauthorizedResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndPasswordIsNotNullOrWhiteSpaceAndRemovePasswordFailed_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.UserRepository.RemovePassword(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.Edit("UserId", new UserFormViewModel()
            {
                Id = "UserId",
                Role = UserRoleEnumViewModel.Writer,
                Password = "Password"
            });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUpdateRoleFailed_ShouldReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.UserRepository.UpdateRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUpdateRoleSucceedAndUpdateUserFailed_ShouldReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.UserRepository.UpdateRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWork
                .Setup(x => x.UserRepository.Update(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValidAndUpdateRoleSucceedAndUpdateUserSucceed_ShouldReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsUserAllowedForOperation(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _unitOfWork
                .Setup(x => x.UserRepository.UpdateRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWork
                .Setup(x => x.UserRepository.Update(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            _controller.HttpContext.Request.Form =
                        new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection());

            var result = await _controller.Edit("UserId", new UserFormViewModel() { Id = "UserId", Role = UserRoleEnumViewModel.Writer });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task ToggleActive_IdIsNull_ReturnJsonResultWithBadRequest()
        {
            var result = await _controller.ToggleActive(null);
            result.Should().BeOfType<JsonResult>();
            result.Value.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task ToggleActive_UserIsNull_ReturnJsonResultWithBadRequest()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            _unitOfWork
                .Setup(x => x.UserRepository.IsInRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _controller.ToggleActive("UserId");
            result.Should().BeOfType<JsonResult>();
            result.Value.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task ToggleActive_UserIsSuperAdmin_ReturnJsonResultWithBadRequest()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsInRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _controller.ToggleActive("UserId");
            result.Should().BeOfType<JsonResult>();
            result.Value.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task ToggleActive_UserIsNotNullAndUserIsNotSuperAdmin_ReturnJsonResultWithOKResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.IsInRole(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _unitOfWork
                .Setup(x => x.UserRepository.Update(It.IsAny<User>()))
                .Verifiable();

            var result = await _controller.ToggleActive("UserId");
            result.Should().BeOfType<JsonResult>();
            result.Value.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task LoginPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(UserLoginFormViewModel.Username), "Error");
            var result = await _controller.Login(new UserLoginFormViewModel(), It.IsAny<string>());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task LoginPost_UserIsNull_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByUserName(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.Login(new UserLoginFormViewModel(), It.IsAny<string>());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task LoginPost_UserIsNotNullAndUserIsNotActive_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User() { IsActive = false });

            var result = await _controller.Login(new UserLoginFormViewModel(), It.IsAny<string>());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task LoginPost_UserIsNotNullAndUserIsActiveAndUserLoginFailed_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User() { IsActive = true });

            _unitOfWork
                .Setup(x => x.UserRepository.SignInByPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            var result = await _controller.Login(new UserLoginFormViewModel(), It.IsAny<string>());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task LoginPost_UserIsNotNullAndUserIsActiveAndUserLoginSucceed_ReturnLocalRedirectResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User() { IsActive = true });

            _unitOfWork
                .Setup(x => x.UserRepository.SignInByPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var result = await _controller.Login(new UserLoginFormViewModel(), It.IsAny<string>());
            result.Should().BeOfType<LocalRedirectResult>();
        }

        [Test]
        public async Task ForgotPasswordPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(UserForgotPasswordFormViewModel.Email), "Error");
            var result = await _controller.ForgotPassword(new UserForgotPasswordFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ForgotPasswordPost_ModelStateIsValidAndUserIsNull_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.ForgotPassword(new UserForgotPasswordFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ForgotPasswordPost_ModelStateIsValidAndUserIsNotNullAndEmailIsNotSent_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();
            _controller.Url = mockUrlHelper.Object;

            _emailSender
                .Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var result = await _controller.ForgotPassword(new UserForgotPasswordFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ForgotPasswordPost_ModelStateIsValidAndUserIsNotNullAndEmailIsSent_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();
            _controller.Url = mockUrlHelper.Object;

            _emailSender
                .Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var result = await _controller.ForgotPassword(new UserForgotPasswordFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task ResetPasswordPost_ModelStateIsNotValid_ReturnViewResult()
        {
            _controller.ModelState.AddModelError(nameof(UserResetPasswordFormViewModel.Email), "Error");
            var result = await _controller.ResetPassword(new UserResetPasswordFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ResetPasswordPost_ModelStateIsValidAndUserIsNull_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _controller.ResetPassword(new UserResetPasswordFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task ResetPasswordPost_ModelStateIsValidAndUserIsNotNullAndResetPasswordFailed_ReturnViewResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.ResetPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _controller.ResetPassword(new UserResetPasswordFormViewModel());
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task ResetPasswordPost_ModelStateIsValidAndUserIsNotNullAndResetPasswordSucceed_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByEmail(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _unitOfWork
                .Setup(x => x.UserRepository.ResetPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.ResetPassword(new UserResetPasswordFormViewModel());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
