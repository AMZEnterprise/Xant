using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
    [Area("Panel")]
    [Authorize(Roles = ConstantUserRoles.SuperAdmin + "," + ConstantUserRoles.Admin + "," + ConstantUserRoles.Writer)]
    public class PostCommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        public PostCommentsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor accessor
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadPostCommentsTable([FromBody] DTParameters dtParameters)
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

            var result = _unitOfWork.PostCommentRepository.GetAll();

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);
            if (!await _unitOfWork.UserRepository.IsInRole(user, ConstantUserRoles.SuperAdmin))
            {
                result = result.Where(x => x.Post.UserId == user.Id);
            }

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(r =>
                    (r.UserFullName != null && r.UserFullName.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.Post.Title != null && r.Post.Title.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.Body != null && r.Body.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.Email != null && r.Email.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.Ip != null && r.Ip.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.CreateDate.ToString("F") != null && r.CreateDate.ToString("F").Contains(searchBy)) ||
                    (r.LastEditDate.ToString("F") != null && r.LastEditDate.ToString("F").Contains(searchBy))
                );
            }

            if (string.Equals(orderCriteria,
                nameof(PostCommentIndexViewModel.UserFullName), StringComparison.InvariantCultureIgnoreCase))
            {
                result = orderAscendingDirection
                    ? result.OrderBy(x => x.User == null ? x.UserFullName : x.User.FirstName + " " + x.User.LastName)
                    : result.OrderByDescending(x => x.User == null ? x.UserFullName : x.User.FirstName + " " + x.User.LastName);
            }
            else if (string.Equals(orderCriteria,
                nameof(PostCommentIndexViewModel.PostTitle), StringComparison.InvariantCultureIgnoreCase))
            {
                result = orderAscendingDirection
                    ? result.OrderBy(x => x.Post.Title)
                    : result.OrderByDescending(x => x.Post.Title);
            }
            else
            {
                result = orderAscendingDirection ?
                    result.OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc) :
                    result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);
            }

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _unitOfWork.PostCommentRepository.Count();

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
                    .Map<IEnumerable<PostComment>, IEnumerable<PostCommentIndexViewModel>>(resultList)
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var postComment = await _unitOfWork.PostCommentRepository.GetById(id);

            if (postComment == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, postComment.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<PostComment, PostCommentFormViewModel>(postComment));
        }

        public async Task<IActionResult> Reply(int id)
        {
            var parentPostComment = await _unitOfWork.PostCommentRepository.GetById(id);
            if (parentPostComment == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, parentPostComment.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<PostComment, PostCommentReplyFormViewModel>(parentPostComment));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(PostCommentReplyFormViewModel postCommentReplyFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var parentPostComment = await _unitOfWork.PostCommentRepository
                    .GetById(postCommentReplyFormViewModel.ParentId);

                if (parentPostComment == null)
                {
                    return NotFound();
                }

                var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

                if (!await _unitOfWork.UserRepository
                    .IsUserAllowedForOperation(user, parentPostComment.UserId, ConstantUserRoles.SuperAdmin))
                {
                    return Unauthorized();
                }

                var postComment = _mapper.Map<PostCommentReplyFormViewModel, PostComment>(postCommentReplyFormViewModel);

                postComment.UserId = user.Id;
                postComment.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.PostCommentRepository.Insert(postComment);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(postCommentReplyFormViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var postComment = await _unitOfWork.PostCommentRepository.GetById(id);

            if (postComment == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, postComment.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<PostComment, PostCommentFormViewModel>(postComment));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCommentFormViewModel postCommentFormViewModel)
        {
            if (id != postCommentFormViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

                if (!await _unitOfWork.UserRepository
                    .IsUserAllowedForOperation(user, postCommentFormViewModel.PostUserId, ConstantUserRoles.SuperAdmin))
                {
                    return Unauthorized();
                }

                var postComment = _mapper.Map<PostCommentFormViewModel, PostComment>(postCommentFormViewModel);

                await _unitOfWork.PostCommentRepository.UpdateChildrenStatus(postComment.Id, postComment.Status);
                _unitOfWork.PostCommentRepository.Update(postComment);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(postCommentFormViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var postComment = await _unitOfWork.PostCommentRepository.GetById(id);

            if (postComment == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, postComment.UserId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<PostComment, PostCommentFormViewModel>(postComment));
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var user = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(user, userId, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            await _unitOfWork.PostCommentRepository.DeleteChildren(id);
            await _unitOfWork.PostCommentRepository.Delete(id);
            await _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
