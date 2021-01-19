using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models;
using Xant.MVC.Models.Constants;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public ContactUsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessor = accessor;
        }

        [Breadcrumb("ViewData.Title")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactPost(ContactFormViewModel contactFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<ContactFormViewModel, Contact>(contactFormViewModel);

                contact.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.ContactRepository.Insert(contact);
                await _unitOfWork.Complete();

                return new JsonResult(new JsonResultModel()
                {
                    StatusCode = JsonResultStatusCode.Success,
                    Message = ConstantMessages.ContactSentSuccessfully
                });
            }

            return new JsonResult(new JsonResultModel()
            {
                StatusCode = JsonResultStatusCode.ModelStateIsNotValid,
                Message = ConstantMessages.ContactFailedToSend
            });
        }
    }
}
