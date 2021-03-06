﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Controllers
{
    [Area(ConstantAreas.Panel)]
    [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SettingsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var setting = await _unitOfWork.SettingRepository.Get();

            if (setting == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Setting, SettingFormViewModel>(setting));
        }

        [HttpPost, ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(int id, SettingFormViewModel settingFormViewModel)
        {
            if (id != settingFormViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var setting = _mapper.Map<SettingFormViewModel, Setting>(settingFormViewModel);
                _unitOfWork.SettingRepository.Update(setting);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(settingFormViewModel);
        }
    }
}
