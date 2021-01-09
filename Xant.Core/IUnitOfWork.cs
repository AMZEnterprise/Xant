using System.Threading.Tasks;
using Xant.Core.Repositories;

namespace Xant.Core
{
    /// <summary>
    /// Represents unit of work interface 
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Represents contact repository interface
        /// </summary>
        IContactRepository ContactRepository { get; set; }
        /// <summary>
        /// Represents post category repository interface
        /// </summary>
        IPostCategoryRepository PostCategoryRepository { get; set; }
        /// <summary>
        /// Represents post comment category repository interface
        /// </summary>
        IPostCommentRepository PostCommentRepository { get; set; }
        /// <summary>
        /// Represents post repository interface
        /// </summary>
        IPostRepository PostRepository { get; set; }
        /// <summary>
        /// Represents setting repository interface
        /// </summary>
        ISettingRepository SettingRepository { get; set; }
        /// <summary>
        /// Represents user repository interface
        /// </summary>
        IUserRepository UserRepository { get; set; }
        /// <summary>
        /// Commit changes to database
        /// </summary>
        /// <returns></returns>
        Task Complete();
    }
}
