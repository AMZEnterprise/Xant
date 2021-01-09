using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Core.Repositories;

namespace Xant.Persistence.EfCoreRepositories
{
    public class EfCorePostCategoryRepository : IPostCategoryRepository
    {
        private readonly IApplicationDbContext _context;

        public EfCorePostCategoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<PostCategory> GetAll()
        {
            return _context.PostCategories.AsQueryable();
        }

        public async Task<PostCategory> GetById(int id)
        {
            return await _context.PostCategories.FindAsync(id);
        }

        public void Insert(PostCategory postCategory)
        {
            postCategory.CreateDate = postCategory.LastEditDate = DateTime.Now;

            _context.PostCategories.Add(postCategory);
        }

        public void Update(PostCategory postCategory)
        {
            postCategory.LastEditDate = DateTime.Now;

            _context.PostCategories.Update(postCategory);
        }

        public async Task Delete(int id)
        {
            var postCategory = await GetById(id);
            if (postCategory != null)
                _context.PostCategories.Remove(postCategory);
        }

        public async Task<int> Count()
        {
            return await _context.PostCategories.CountAsync();
        }
    }
}
