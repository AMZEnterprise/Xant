using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents post comments repository interface 
    /// </summary>
    public interface IPostCommentRepository
    {
        /// <summary>
        /// Get all post comments
        /// </summary>
        /// <returns>returns all post comments</returns>
        IQueryable<PostComment> GetAll();
        /// <summary>
        /// Get a post comment by id
        /// </summary>
        /// <param name="id">post comment id</param>
        /// <returns>returns a specific post comment or null</returns>
        Task<PostComment> GetById(int id);
        /// <summary>
        /// Insert new post comment
        /// </summary>
        /// <param name="postComment">new post comment</param>
        void Insert(PostComment postComment);
        /// <summary>
        /// Update existing post comment
        /// </summary>
        /// <param name="postComment">updated post comment</param>
        void Update(PostComment postComment);
        /// <summary>
        /// Delete a post comment by id
        /// </summary>
        /// <param name="id">post comment id</param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// Count total post comments
        /// </summary>
        /// <returns>returns post comments count</returns>
        Task<int> Count();
    }
}
