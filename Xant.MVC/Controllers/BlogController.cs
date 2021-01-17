using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models;
using Xant.MVC.Models.Constants;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Controllers
{
    public class BlogController : PostBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public BlogController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor accessor) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessor = accessor;
        }

        [Breadcrumb("وبلاگ")]
        public async Task<IActionResult> Index(
            int? page, string searchString, int? postCategoryId, string postTag, PostSortFilterType postSortFilterType = PostSortFilterType.SortByDateDesc)
        {
            var postSearch = new PostSearch()
            {
                SearchString = searchString,
                PostCategoryId = postCategoryId,
                PostTag = postTag,
                PostSortFilterType = postSortFilterType,
            };

            int pageSize = 6;

            var posts = GetPosts(PostCategoryType.Blog, postSearch, pageIndex: page - 1 ?? 0, pageSize: pageSize);

            var blogViewModel = new PostsViewModel()
            {
                PostViewModels = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(posts),
                Pager = new Pager(posts.TotalCount, posts.TotalPages, page, pageSize),
                SearchString = searchString,
                PostCategoryId = postCategoryId,
                PostTag = postTag,
                PostSortFilterType = postSortFilterType
            };

            return View(blogViewModel);
        }

        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _unitOfWork.PostRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            var postViewModel = _mapper.Map<Post, PostViewModel>(post);

            var postComments = await _unitOfWork.PostCommentRepository
                .GetAllByPostId(post.Id);

            postViewModel.PostCommentViewModels =
                _mapper.Map<IEnumerable<PostComment>, IEnumerable<PostCommentViewModel>>(postComments);

            return View(postViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentPost(PostCommentFormViewModel postCommentFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var postComment = _mapper.Map<PostCommentFormViewModel, PostComment>(postCommentFormViewModel);

                postComment.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                postComment.Status = PostCommentStatus.Unclear;

                _unitOfWork.PostCommentRepository.Insert(postComment);
                await _unitOfWork.Complete();

                return new JsonResult(new JsonResultModel()
                {
                    StatusCode = JsonResultStatusCode.Success,
                    Message = ConstantMessages.CommentSentAndWillShowAfterAcceptance
                });
            }

            return new JsonResult(new JsonResultModel()
            {
                StatusCode = JsonResultStatusCode.ModelSatedNotValid,
                Message = ConstantMessages.CommentFailedToSend
            });
        }
    }
}
