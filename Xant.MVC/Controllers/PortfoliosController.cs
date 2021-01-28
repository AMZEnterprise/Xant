using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Controllers
{
    public class PortfoliosController : PostBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PortfoliosController(
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Breadcrumb("نمونه کارها")]
        public IActionResult Index(int? page, int? postCategoryId, PostSortFilterType postSortFilterType = PostSortFilterType.SortByDateDesc)
        {
            var postSearch = new PostSearch()
            {
                PostCategoryId = postCategoryId,
                PostTag = null,
                PostSortFilterType = postSortFilterType
            };

            int pageSize = int.MaxValue;

            var posts = GetPosts(PostCategoryType.Portfolio, postSearch, pageIndex: page - 1 ?? 0, pageSize: pageSize);

            var postsViewModel = new PostsViewModel()
            {
                PostViewModels = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(posts),
                Pager = new Pager(posts.TotalCount, posts.TotalPages, page, pageSize),
                PostCategoryId = postCategoryId,
                PostTag = null,
                PostSortFilterType = postSortFilterType
            };

            return View(postsViewModel);
        }

        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Portfolio(int id)
        {
            var post = await _unitOfWork.PostRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Post, PostViewModel>(post));
        }
    }
}
