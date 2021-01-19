using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Controllers;
using Xant.MVC.Models;
using Xant.MVC.Models.ViewModels;

namespace Xant.Tests.Controllers
{
    [TestFixture]
    public class ContactUsControllerTests
    {
        private ContactUsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _accessor;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _accessor = new Mock<IHttpContextAccessor>();
            _controller = new ContactUsController(_unitOfWork.Object, _mapper.Object, _accessor.Object);
        }

        [Test]
        public async Task ContactPost_ModelStateIsNotValid_ReturnJsonResultWithJsonResultModelAndModelStateIsNotValidStatusCode()
        {
            _controller.ModelState.AddModelError(nameof(ContactFormViewModel.Id), "Error");
            var result = await _controller.ContactPost(new ContactFormViewModel());

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
        public async Task ContactPost_ModelStateIsValid_ReturnJsonResultWithJsonResultModelAndSuccessStatusCode()
        {
            _mapper
                .Setup(x => x.Map<ContactFormViewModel, Contact>(It.IsAny<ContactFormViewModel>()))
                .Returns(new Contact());

            var connection = new Mock<ConnectionInfo>(MockBehavior.Strict);
            connection.SetupGet(c => c.RemoteIpAddress).Returns(new IPAddress(It.IsAny<long>()));

            var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
            httpContext.SetupGet(x => x.Connection).Returns(connection.Object);

            _accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            _unitOfWork
                .Setup(x => x.ContactRepository.Insert(It.IsAny<Contact>()))
                .Verifiable();

            var result = await _controller.ContactPost(new ContactFormViewModel());
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
