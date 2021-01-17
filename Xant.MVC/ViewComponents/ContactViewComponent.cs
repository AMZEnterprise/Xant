using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactViewComponent(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _unitOfWork.SettingRepository.Get();
            return View(_mapper.Map<Setting, ContactViewModel>(setting));
        }
    }
}
