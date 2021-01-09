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
    public class SettingsControllerTests
    {
        private SettingsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _controller = new SettingsController(_unitOfWork.Object, _mapper.Object);
        }

        [Test]
        public async Task Index_SettingIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.SettingRepository.Get())
                .ReturnsAsync((Setting)null);

            var result = await _controller.Index();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Index_SettingIsNotNull_ReturnSettingFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.SettingRepository.Get())
                .ReturnsAsync(new Setting());

            _mapper
                .Setup(x => x.Map<Setting, SettingFormViewModel>(It.IsAny<Setting>()))
                .Returns(new SettingFormViewModel());

            var result = await _controller.Index();
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<SettingFormViewModel>();
        }


        [Test]
        public async Task IndexPost_IdAndSettingFormViewModelIdAreNotEqual_ReturnBadRequest()
        {
            var result = await _controller.IndexPost(1, new SettingFormViewModel() { Id = 2 });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task IndexPost_ModelStateIsNotValid_ReturnSettingFormViewModelToView()
        {
            _controller.ModelState.AddModelError(nameof(Setting.Phone1), "Error");

            var result = await _controller.IndexPost(1, new SettingFormViewModel() { Id = 1 });
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<SettingFormViewModel>();
        }

        [Test]
        public async Task IndexPost_ModelStateIsValid_ReturnRedirectToActionResult()
        {
            _mapper
                .Setup(x => x.Map<SettingFormViewModel, Setting>(It.IsAny<SettingFormViewModel>()))
                .Returns(new Setting());

            _unitOfWork
                .Setup(x => x.SettingRepository.Update(It.IsAny<Setting>()))
                .Verifiable();

            var result = await _controller.IndexPost(1, new SettingFormViewModel() { Id = 1 });
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
