using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents post repository interface 
    /// </summary>
    public interface IPostRepository
    {
        /// <summary>
        /// Get all posts of specific post category type
        /// </summary>
        /// <param name="postCategoryType">post category type</param>
        /// <returns>returns all posts with specific post category type</returns>
        IQueryable<Post> GetAll(PostCategoryType postCategoryType);
        /// <summary>
        /// Get all posts
        /// </summary>
        /// <returns>returns all posts</returns>
        IQueryable<Post> GetAll();
        /// <summary>
        /// Get a post by id
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>returns a specific post or null</returns>
        Task<Post> GetById(int id);
        /// <summary>
        /// Insert new post
        /// </summary>
        /// <param name="post">new post</param>
        void Insert(Post post);
        /// <summary>
        /// Update existing post
        /// </summary>
        /// <param name="post">updated post</param>
        void Update(Post post);
        /// <summary>
        /// Delete a post by id
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// Count total posts
        /// </summary>
        /// <returns>returns posts count</returns>
        Task<int> Count();
    }
}
