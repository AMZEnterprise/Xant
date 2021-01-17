using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Infrastructure;
using Xant.MVC.Models;

namespace Xant.MVC.Controllers
{
    public class PostBaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostBaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [NonAction]
        public PagedList<Post> GetPosts(
            PostCategoryType postCategoryType, PostSearch postSearchDto = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _unitOfWork.PostRepository.GetAll(postCategoryType);

            if (postSearchDto != null)
            {
                if (postSearchDto.PostCategoryId != null)
                {
                    query = query.Where(x => x.PostCategoryId == postSearchDto.PostCategoryId);
                }

                if (!string.IsNullOrWhiteSpace(postSearchDto.PostTag))
                {
                    query = query.Where(x => x.Tags.Contains(postSearchDto.PostTag));
                }

                if (!string.IsNullOrWhiteSpace(postSearchDto.SearchString))
                {
                    string searchString = postSearchDto.SearchString?.ToLower();
                    query = query.Where(
                            x => (x.Title.Contains(searchString) ||
                                  x.Tags.Contains(searchString) ||
                                  x.PostCategory.Title.Contains(searchString) ||
                                  x.Body.Contains(searchString))
                        );
                }

                switch (postSearchDto.PostSortFilterType)
                {
                    case PostSortFilterType.SortByDateAsc:
                        query = query.OrderBy(x => x.CreateDate);
                        break;
                    case PostSortFilterType.SortByDateDesc:
                        query = query.OrderByDescending(x => x.CreateDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CreateDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CreateDate);
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }
    }
}
