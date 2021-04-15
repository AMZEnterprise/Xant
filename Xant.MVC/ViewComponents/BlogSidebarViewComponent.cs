using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.ViewComponents
{
    public class BlogSidebarViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogSidebarViewComponent(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _unitOfWork
                .PostRepository
                .GetAll(PostCategoryType.Blog)
                .Take(3)
                .ToListAsync();

            var postCategoryViewModels = await _unitOfWork
                .PostCategoryRepository
                .GetAll(PostCategoryType.Blog)
                .Include(x => x.Posts)
                .Where(x => x.Posts.Count > 0)
                .Select(x => new PostCategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    PostsCount = x.Posts.Count()
                }).ToListAsync();

            var postTags = _unitOfWork.PostRepository.GetAll(PostCategoryType.Blog)
                .OrderByDescending(x => x.CreateDate)
                .FirstOrDefault()?.Tags?.Split(',').ToList();

            var blogSidebarViewModel = new BlogSidebarViewModel()
            {
                PostViewModels = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(posts),
                PostCategoryViewModels = postCategoryViewModels,
                PostTags = postTags ?? new List<string>()
            };

            return View(blogSidebarViewModel);
        }
    }
}
