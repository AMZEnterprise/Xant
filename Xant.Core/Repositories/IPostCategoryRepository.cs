using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents post category repository interface 
    /// </summary>
    public interface IPostCategoryRepository
    {
        /// <summary>
        /// Get all post categories of specific type
        /// </summary>
        /// <returns>returns all post categories with the specified type</returns>
        IQueryable<PostCategory> GetAll(PostCategoryType postCategoryType);
        /// <summary>
        /// Get all post categories
        /// </summary>
        /// <returns>returns all post categories</returns>
        IQueryable<PostCategory> GetAll();
        /// <summary>
        /// Get a post category by id
        /// </summary>
        /// <param name="id">post category id</param>
        /// <returns>returns a specific post category or null</returns>
        Task<PostCategory> GetById(int id);
        /// <summary>
        /// Insert new post category
        /// </summary>
        /// <param name="postCategory">new post category</param>
        void Insert(PostCategory postCategory);
        /// <summary>
        /// Update existing post category
        /// </summary>
        /// <param name="postCategory">updated post category</param>
        void Update(PostCategory postCategory);
        /// <summary>
        /// Delete a post category by id
        /// </summary>
        /// <param name="id">post category id</param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// Check whether post category currently exists
        /// </summary>
        /// <param name="postCategory">post category</param>
        /// <returns>true if exists, false if not</returns>
        Task<bool> IsPostCategoryExists(PostCategory postCategory);
        /// <summary>
        /// Count total post categories
        /// </summary>
        /// <returns>returns post categories count</returns>
        Task<int> Count();
    }
}
