using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Core.Repositories;

namespace Xant.Persistence.EfCoreRepositories
{
    public class EfCorePostCommentRepository : IPostCommentRepository
    {
        private readonly IApplicationDbContext _context;

        public EfCorePostCommentRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<PostComment> GetAll()
        {
            return _context.PostComments
                .Include(x => x.Children)
                .Include(x => x.User)
                .Include(x => x.Post)
                .AsQueryable();
        }

        public async Task<PostComment> GetById(int id)
        {
            return await _context.PostComments
                .Include(x => x.User)
                .Include(x => x.Post)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(PostComment postComment)
        {
            postComment.IsEdited = false;
            postComment.CreateDate = postComment.LastEditDate = DateTime.Now;

            _context.PostComments.Add(postComment);
        }

        public void Update(PostComment postComment)
        {
            postComment.IsEdited = true;
            postComment.LastEditDate = DateTime.Now;

            _context.PostComments.Update(postComment);
        }

        public async Task Delete(int id)
        {
            var postComment = await GetById(id);
            if (postComment != null)
                _context.PostComments.Remove(postComment);
        }

        public async Task<int> Count()
        {
            return await _context.PostComments.CountAsync();
        }
    }
}
