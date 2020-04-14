using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMai.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Data
{
    public class InsideMaiContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<UserCommentLike> UserCommentLikes { get; set; }
        public DbSet<UserPostLike> UserPostLike { get; set; }






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
            modelBuilder.Entity<IdentityRole>().HasData(
                //new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() },
                new IdentityRole { Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() }
            );

            modelBuilder.Initialize();

        }

    }
}
