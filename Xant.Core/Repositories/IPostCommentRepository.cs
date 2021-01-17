using System.Collections.Generic;
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
        /// Get a post all comments
        /// </summary>
        /// <param name="postId">post id</param>
        /// <returns>return an IEnumerable of the post comments</returns>
        Task<IEnumerable<PostComment>> GetAllByPostId(int postId);
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
        /// Update a post comment all children status
        /// </summary>
        /// <param name="id">post comment id</param>
        /// <param name="status">updated status</param>
        /// <returns></returns>
        Task UpdateChildrenStatus(int id, PostCommentStatus status);
        /// <summary>
        /// Delete a post comment by id
        /// </summary>
        /// <param name="id">post comment id</param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// Delete a post comment all children
        /// </summary>
        /// <param name="id">post comment id</param>
        /// <returns></returns>
        Task DeleteChildren(int id);
        /// <summary>
        /// Count total post comments
        /// </summary>
        /// <returns>returns post comments count</returns>
        Task<int> Count();
    }
}
