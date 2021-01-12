using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Extensions;
using Xant.MVC.Areas.Panel.Models;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Models.Constants;
using Xant.MVC.Services.FileHandler;

namespace Xant.MVC.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = ConstantUserRoles.SuperAdmin + "," + ConstantUserRoles.Admin + "," + ConstantUserRoles.Writer)]
    public class PostsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandler _fileHandler;
        private readonly IWebHostEnvironment _env;

        public PostsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileHandler fileHandler,
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandler = fileHandler;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadPostsTable([FromBody] DTParameters dtParameters)
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

            var result = _unitOfWork.PostRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(r =>
                    (r.Title != null && r.Title.Contains(searchBy)) ||
                    (r.User != null && (r.User.FirstName.Contains(searchBy) || r.User.LastName.Contains(searchBy))) ||
                    (r.PostCategory != null && r.PostCategory.Title.Contains(searchBy)) ||
                    (r.Body != null && r.Body.Contains(searchBy)) ||
                    (r.Tags != null && r.Tags.Contains(searchBy)) ||
                    (r.CreateDate.ToString("F") != null && r.CreateDate.ToString("F").Contains(searchBy)) ||
                    (r.LastEditDate.ToString("F") != null && r.LastEditDate.ToString("F").Contains(searchBy))
                );
            }

            result = orderAscendingDirection ?
                result.OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc) :
                result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _unitOfWork.PostRepository.Count();

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
                    .Map<IEnumerable<Post>, IEnumerable<PostIndexViewModel>>(resultList)
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _unitOfWork.PostRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, post.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            ViewData["PostCategoryId"] = GetPostCategories();
            return View(_mapper.Map<Post, PostFormViewModel>(post));
        }

        public IActionResult Create()
        {
            ViewData["PostCategoryId"] = GetPostCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostFormViewModel postFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var post = _mapper.Map<PostFormViewModel, Post>(postFormViewModel);

                var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

                post.UserId = user.Id;

                _unitOfWork.PostRepository.Insert(post);
                await _unitOfWork.Complete();

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    await _fileHandler.UploadMedia(
                        files,
                        _env.WebRootPath,
                        post.FilesPathGuid.ToString(),
                        FileHandlerFolder.Posts);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["PostCategoryId"] = GetPostCategories();
            return View(postFormViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _unitOfWork.PostRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, post.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            ViewData["PostCategoryId"] = GetPostCategories();
            return View(_mapper.Map<Post, PostFormViewModel>(post));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostFormViewModel postFormViewModel)
        {
            if (id != postFormViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

                if (!await _unitOfWork.UserRepository
                    .IsUserAllowedForOperation(user, postFormViewModel.UserId, ConstantUserRoles.SuperAdmin))
                {
                    return Unauthorized();
                }

                var post = _mapper.Map<PostFormViewModel, Post>(postFormViewModel);

                _unitOfWork.PostRepository.Update(post);
                await _unitOfWork.Complete();

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    //Delete old file
                    _fileHandler.DeleteMedia(
                        _env.WebRootPath,
                        post.FilesPathGuid.ToString(),
                        FileHandlerFolder.Posts
                    );

                    //Upload new file
                    await _fileHandler.UploadMedia(
                        files,
                        _env.WebRootPath,
                        post.FilesPathGuid.ToString(),
                        FileHandlerFolder.Posts);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["PostCategoryId"] = GetPostCategories();
            return View(postFormViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _unitOfWork.PostRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, post.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            ViewData["PostCategoryId"] = GetPostCategories();
            return View(_mapper.Map<Post, PostFormViewModel>(post));
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string userId, string filesPathGuid)
        {
            if (userId == null || filesPathGuid == null)
            {
                return BadRequest();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, userId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.Complete();

            _fileHandler.DeleteMedia(
                _env.WebRootPath,
                filesPathGuid,
                FileHandlerFolder.Posts
                );

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public SelectList GetPostCategories()
        {
            return new SelectList(
                _unitOfWork.PostCategoryRepository.GetAll(),
                "Id",
                "Title"
            );
        }
    }
}
