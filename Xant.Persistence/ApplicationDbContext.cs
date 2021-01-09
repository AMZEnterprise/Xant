using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xant.Core.Domain;

namespace Xant.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Contact

            builder.Entity<Contact>()
                .Property(x => x.UserFullName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Contact>()
                .Property(x => x.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Contact>()
                .Property(x => x.Subject)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Contact>()
                .Property(x => x.Body)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Entity<Contact>()
                .Property(x => x.Ip)
                .HasMaxLength(256)
                .IsRequired();

            #endregion

            #region Post

            builder.Entity<Post>()
                .Property(x => x.Title)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Post>()
                .Property(x => x.Body)
                .IsRequired();

            builder.Entity<Post>()
                .Property(x => x.Tags)
                .HasMaxLength(1000)
                .IsRequired();

            #endregion

            #region PostCategory

            builder.Entity<PostCategory>()
                .Property(x => x.Title)
                .HasMaxLength(256)
                .IsRequired();

            #endregion

            #region PostComment

            builder.Entity<PostComment>()
                .Property(x => x.UserFullName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<PostComment>()
                .Property(x => x.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<PostComment>()
                .Property(x => x.Body)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Entity<PostComment>()
                .Property(x => x.Ip)
                .HasMaxLength(256)
                .IsRequired();

            #endregion

            #region Setting

            builder.Entity<Setting>()
                .Property(x => x.Phone1)
                .HasMaxLength(20);

            builder.Entity<Setting>()
                .Property(x => x.Phone2)
                .HasMaxLength(20);

            builder.Entity<Setting>()
                .Property(x => x.Email1)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.Email2)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.Address1)
                .HasMaxLength(1000);

            builder.Entity<Setting>()
                .Property(x => x.Address2)
                .HasMaxLength(1000);

            builder.Entity<Setting>()
                .Property(x => x.Instagram)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.Telegram)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.GooglePlus)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.FaceBook)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.LinkedIn)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.Youtube)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.Aparat)
                .HasMaxLength(256);

            builder.Entity<Setting>()
                .Property(x => x.GitHub)
                .HasMaxLength(256);
            #endregion

            #region User

            builder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<User>()
                .Property(x => x.Biography)
                .HasMaxLength(1000);

            #endregion


            base.OnModelCreating(builder);
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
