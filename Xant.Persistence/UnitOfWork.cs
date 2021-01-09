using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.Core.Repositories;
using Xant.Persistence.EfCoreRepositories;

namespace Xant.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UnitOfWork(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IPasswordHasher<User> passwordHasher
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;

            ContactRepository = new EfCoreContactRepository(_context);
            PostCategoryRepository = new EfCorePostCategoryRepository(_context);
            PostCommentRepository = new EfCorePostCommentRepository(_context);
            PostRepository = new EfCorePostRepository(_context);
            SettingRepository = new EfCoreSettingRepository(_context);
            UserRepository = new EfCoreUserRepository(_userManager, _signInManager, _passwordHasher);
        }
        public IContactRepository ContactRepository { get; set; }
        public IPostCategoryRepository PostCategoryRepository { get; set; }
        public IPostCommentRepository PostCommentRepository { get; set; }
        public IPostRepository PostRepository { get; set; }
        public ISettingRepository SettingRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
    }
}
