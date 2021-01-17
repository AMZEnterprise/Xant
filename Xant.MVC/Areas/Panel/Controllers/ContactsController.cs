using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Extensions;
using Xant.MVC.Areas.Panel.Models;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Controllers
{
    [Area(ConstantAreas.Panel)]
    [Authorize(Roles = ConstantUserRoles.SuperAdmin + "," + ConstantUserRoles.Admin)]
    public class ContactsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadContactsTable([FromBody] DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = _unitOfWork.ContactRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(r =>
                    (r.UserFullName != null && r.UserFullName.Contains(searchBy)) ||
                    (r.EmailOrPhoneNumber != null && r.EmailOrPhoneNumber.Contains(searchBy)) ||
                    (r.Subject != null && r.Subject.Contains(searchBy)) ||
                    (r.Body != null && r.Body.Contains(searchBy)) ||
                    (r.Ip != null && r.Ip.Contains(searchBy)) ||
                    (r.CreateDate.ToString("F") != null && r.CreateDate.ToString("F").Contains(searchBy)) ||
                    (r.LastEditDate.ToString("F") != null && r.LastEditDate.ToString("F").Contains(searchBy))
                );
            }

            result = orderAscendingDirection ?
                result.OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc) :
                result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _unitOfWork.ContactRepository.Count();

            var resultList = result
                .Skip(dtParameters.Start)
                .Take(dtParameters.Length)
                .ToList();

            return new JsonResult(new DatatableJsonResultModel()
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = _mapper
                    .Map<IEnumerable<Contact>, IEnumerable<ContactIndexViewModel>>(resultList)
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetById(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Contact, ContactFormViewModel>(contact));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetById(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Contact, ContactFormViewModel>(contact));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactFormViewModel contactFormViewModel)
        {
            if (id != contactFormViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<ContactFormViewModel, Contact>(contactFormViewModel);

                _unitOfWork.ContactRepository.Update(contact);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(contactFormViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetById(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Contact, ContactFormViewModel>(contact));
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.ContactRepository.Delete(id);
            await _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
