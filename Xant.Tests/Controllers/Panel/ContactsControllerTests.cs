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
    public class ContactsControllerTests
    {
        private ContactsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _controller = new ContactsController(_unitOfWork.Object, _mapper.Object);
        }

        [Test]
        public async Task Details_ContactIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Contact)null);

            var result = await _controller.Details(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Details_ContactIsNotNull_ReturnContactFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Contact());

            _mapper
                .Setup(x => x.Map<Contact, ContactFormViewModel>(It.IsAny<Contact>()))
                .Returns(new ContactFormViewModel());

            var result = await _controller.Details(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ContactFormViewModel>();
        }

        [Test]
        public async Task Edit_ContactIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Contact)null);

            var result = await _controller.Edit(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Edit_ContactIsNotNull_ReturnContactFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Contact());

            _mapper
                .Setup(x => x.Map<Contact, ContactFormViewModel>(It.IsAny<Contact>()))
                .Returns(new ContactFormViewModel());

            var result = await _controller.Edit(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ContactFormViewModel>();
        }

        [Test]
        public async Task EditPost_IdAndContactFormViewModelIdAreNotEqual_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError(nameof(ContactFormViewModel.UserFullName), "Error");
            var result = await _controller.Edit(1, new ContactFormViewModel() { Id = 2 });
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsNotValid_ReturnView()
        {
            _controller.ModelState.AddModelError(nameof(ContactFormViewModel.UserFullName), "Error");
            var result = await _controller.Edit(1, new ContactFormViewModel() { Id = 1 });
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task EditPost_ModelStateIsValid_ReturnRedirectToActionResult()
        {
            _mapper
                .Setup(x => x.Map<ContactFormViewModel, Contact>(It.IsAny<ContactFormViewModel>()))
                .Returns(new Contact());

            _unitOfWork
                .Setup(x => x.ContactRepository.Insert(It.IsAny<Contact>()))
                .Verifiable();

            var result = await _controller.Edit(1, new ContactFormViewModel() { Id = 1 });
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Test]
        public async Task Delete_ContactIsNull_ReturnNotFound()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync((Contact)null);

            var result = await _controller.Delete(It.IsAny<int>());
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_ContactIsNotNull_ReturnContactFormViewModelToView()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Contact());

            _mapper
                .Setup(x => x.Map<Contact, ContactFormViewModel>(It.IsAny<Contact>()))
                .Returns(new ContactFormViewModel());

            var result = await _controller.Delete(It.IsAny<int>());
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ContactFormViewModel>();
        }

        [Test]
        public async Task DeleteConfirmed_DeleteContact_ReturnRedirectToActionResult()
        {
            _unitOfWork
                .Setup(x => x.ContactRepository.Delete(It.IsAny<int>()))
                .Verifiable();

            var result = await _controller.DeleteConfirmed(It.IsAny<int>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
