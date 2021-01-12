using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class PostCategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostCategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadPostCategoriesTable([FromBody] DTParameters dtParameters)
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

            var result = _unitOfWork.PostCategoryRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(r =>
                    (r.Title != null && r.Title.Contains(searchBy)) ||
                    (r.CreateDate.ToString("F") != null && r.CreateDate.ToString("F").Contains(searchBy)) ||
                    (r.LastEditDate.ToString("F") != null && r.LastEditDate.ToString("F").Contains(searchBy))
                );
            }

            result = orderAscendingDirection ?
                result.OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc) :
                result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _unitOfWork.PostCategoryRepository.Count();

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
                    .Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryIndexViewModel>>(resultList)
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var postCategory = await _unitOfWork.PostCategoryRepository.GetById(id);

            if (postCategory == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<PostCategory, PostCategoryFormViewModel>(postCategory));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCategoryFormViewModel postCategoryFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var postCategory = _mapper.Map<PostCategoryFormViewModel, PostCategory>(postCategoryFormViewModel);

                if (await _unitOfWork.PostCategoryRepository.IsPostCategoryExists(postCategory))
                {
                    ModelState.AddModelError(nameof(PostCategoryFormViewModel.Title),
                        string.Format(ConstantMessages.DuplicateError, "عنوان"));
                    return View(postCategoryFormViewModel);
                }

                _unitOfWork.PostCategoryRepository.Insert(postCategory);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(postCategoryFormViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var postCategory = await _unitOfWork.PostCategoryRepository.GetById(id);

            if (postCategory == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<PostCategory, PostCategoryFormViewModel>(postCategory));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCategoryFormViewModel postCategoryFormViewModel)
        {
            if (id != postCategoryFormViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var postCategory = _mapper.Map<PostCategoryFormViewModel, PostCategory>(postCategoryFormViewModel);

                if (await _unitOfWork.PostCategoryRepository.IsPostCategoryExists(postCategory))
                {
                    ModelState.AddModelError(nameof(PostCategoryFormViewModel.Title),
                        string.Format(ConstantMessages.DuplicateError, "عنوان"));
                    return View(postCategoryFormViewModel);
                }

                _unitOfWork.PostCategoryRepository.Update(postCategory);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(postCategoryFormViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var postCategory = await _unitOfWork.PostCategoryRepository.GetById(id);

            if (postCategory == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<PostCategory, PostCategoryFormViewModel>(postCategory));
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PostCategoryRepository.Delete(id);
            await _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
