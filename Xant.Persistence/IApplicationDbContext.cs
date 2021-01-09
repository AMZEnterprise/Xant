using Microsoft.EntityFrameworkCore;
using Xant.Core.Domain;

namespace Xant.Persistence
{
    /// <summary>
    /// Represents application database context  interface 
    /// </summary>
    public interface IApplicationDbContext
    {
        DbSet<Contact> Contacts { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<PostCategory> PostCategories { get; set; }
        DbSet<PostComment> PostComments { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<User> Users { get; set; }
    }
}
