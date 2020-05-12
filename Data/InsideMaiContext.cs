using InsideMai.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Data
{
    public sealed class InsideMaiContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<UserCommentLike> UserCommentLikes { get; set; }
        public DbSet<UserPostLike> UserPostLike { get; set; }
        public DbSet<SubscribersObservables> SubscribersObservables { get; set; }
        public DbSet<NotificationsOfNewPosts> NotificationsOfNewPosts { get; set; }

        public InsideMaiContext(DbContextOptions<InsideMaiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Department>();
            modelBuilder.Entity<Comment>();
            modelBuilder.Entity<Favorite>();
            modelBuilder.Entity<UserPostLike>();
            modelBuilder.Entity<UserCommentLike>();

            modelBuilder.Entity<NotificationsOfNewPosts>().HasKey(so => new { so.PostId, so.UserId });
            modelBuilder.Entity<SubscribersObservables>().HasKey(so => new { so.SubscriberId, so.ObservableId });

            modelBuilder.Entity<SubscribersObservables>()
                .HasOne(so => so.Observable)
                .WithMany(b => b.Observables)
                .HasForeignKey(so => so.ObservableId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubscribersObservables>()
                .HasOne(so => so.Subscriber)
                .WithMany(c => c.Subscribers)
                .HasForeignKey(so => so.SubscriberId)
                . OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() }
            );

            modelBuilder.Entity<SubscribersObservables>().HasKey(so => new
            {
                so.SubscriberId,
                so.ObservableId
            });

            modelBuilder.Initialize();
        }
    }
}
