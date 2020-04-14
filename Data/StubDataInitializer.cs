using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMaiWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InsideMaiWebApi.Data
{
    public static class StubDataInitializer
    {
        public static void Initialize(this ModelBuilder modelBuilder)
        {
            InitializeRoles(modelBuilder);
            InitializeDepartments(modelBuilder);
            InitializeUsers(modelBuilder);
            InitializeIdentityUsers(modelBuilder);
            InitializePosts(modelBuilder);
            InitializeComments(modelBuilder);
        }

        private static void InitializeRolestest(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
            new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });
        }

        private static void InitializeUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 1,
                    FirstName = "Андрей",
                    DepartmentId = 16,
                    IsDeleted = false,
                    Role = User.Roles.Admin,
                    Email = "Admin",
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 2,
                    FirstName = "Петя",
                    DepartmentId = 16,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 3,
                    FirstName = "Алексей",
                    LastName = "Долматов",
                    Email = "guf@mail.ru",
                    DepartmentId = 17,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 4,
                    FirstName = "Виктор",
                    LastName = "Сергеев",
                    Email = "vitya@mail.ru",
                    DepartmentId = 16,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 5,
                    FirstName = "Ваня",
                    LastName = "Пук",
                    Email = "vanya@mail.ru",
                    DepartmentId = 17,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 6,
                    FirstName = "Алена",
                    Email = "alena@mail.ru",
                    LastName = "Водонаева",
                    DepartmentId = 17,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                },
                new
                {
                    Id = 7,
                    FirstName = "Петя",
                    Email = "senya@mail.ru",
                    DepartmentId = 14,
                    IsDeleted = false,
                    Role = User.Roles.Student,
                    AccessFailedCount = 4,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true
                });
        }

        private static void InitializeIdentityUsers(ModelBuilder modelBuilder)
        {
            var user = new User
            {
                FirstName = "Виталий",
                LastName = "Цаль",
                Id = 228,
                Email = "Admin",
                NormalizedEmail = "Admin".ToUpper(),
                UserName = "Admin",
                NormalizedUserName = "Admin".ToUpper(),
                AccessFailedCount = 4,
                EmailConfirmed = true,
                LockoutEnabled = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                SecurityStamp = string.Empty,
                DepartmentId = 18
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "123");

            modelBuilder.Entity<User>().HasData(user);
        }

        private static void InitializeRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                //new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() },
                new Role { Id = 1, Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                new Role { Id = 2, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new Role { Id = 3, Name = "Student", NormalizedName = "Student".ToUpper() }
            );
        }

        private static void InitializePosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Content = "Всем привет",
                    LikesCount = 2,
                    PublishDate = new DateTime(2020, 3, 2),
                    SavesCount = 1,
                    Title = "Тема",
                    AuthorId = 1,
                    CommentsCount = 22,
                    DepartmentId = 18,
                    Type = PostType.All
                },
                new Post
                {
                    Id = 2,
                    Content = "Вторая тема",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = ")))",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 17,
                    Type = PostType.Advert

                },
                new Post
                {
                    Id = 3,
                    Content = "Где этот ебучий корпус?",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "Хеееелп",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 18,
                    Type = PostType.Article

                },
                new Post
                {
                    Id = 4,
                    Content = "Как зайти на лмс?",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 4,
                    Type = PostType.Event

                },
                new Post
                {
                    Id = 5,
                    Content = "Всем завтра принести 30 000р на уборку кабинета",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "Объявление",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 19,
                    Type = PostType.Question
                },
                new Post
                {
                    Id = 6,
                    Content = "Да да, пошел я нахер",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 19,
                    Type = PostType.Event

                },
                new Post
                {
                    Id = 7,
                    Content = "Кто не сдаст - отчислен",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "Всем сдать справку",
                    AuthorId = 2,
                    CommentsCount = 12,
                    DepartmentId = 19,
                    Type = PostType.Event

                },
                new Post
                {
                    Id = 8,
                    Content = "Не работает интернет как пофиксить ?",
                    LikesCount = 223,
                    PublishDate = new DateTime(2020, 3, 3),
                    SavesCount = 2,
                    Title = "хелп",
                    AuthorId = 2,
                    CommentsCount = 12,
                    Type = PostType.Event

                });
        }

        private static void InitializeComments(ModelBuilder modeBuilder)
        {
            modeBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    AuthorId = 1,
                    PostId = 1,
                    Content = "Тупо лайкосик",
                },
                new Comment
                {
                    Id = 2,
                    AuthorId = 2,
                    PostId = 2,
                    Content = "я",
                },
                new Comment
                {
                    Id = 3,
                    AuthorId = 4,
                    PostId = 4,
                    Content = "Кто главный тут ?",
                },
                new Comment
                {
                    Id = 4,
                    AuthorId = 3,
                    PostId = 3,
                    Content = "Тупо лайк",
                },
                new Comment
                {
                    Id = 5,
                    AuthorId = 2,
                    PostId = 2,
                    Content = "Вчера ? не не",
                },
                new Comment
                {
                    Id = 6,
                    AuthorId = 3,
                    PostId = 3,
                    Content = "Ураааааааааа",
                },
                new Comment
                {
                    Id = 7,
                    AuthorId = 2,
                    PostId = 2,
                    Content = "луууулл",
                },
                new Comment
                {
                    Id = 8,
                    AuthorId = 3,
                    PostId = 3,
                    Content = "На самом деле, тут можно просто ничего не делать и все, спасибо, пока",
                },
                new Comment
                {
                    Id = 9,
                    AuthorId = 4,
                    PostId = 4,
                    Content = "ЧИ ДАААА?",
                },
                new Comment
                {
                    Id = 10,
                    AuthorId = 6,
                    PostId = 5,
                    Content = "Чегооооо",
                },
                new Comment
                {
                    Id = 11,
                    AuthorId = 7,
                    PostId = 7,
                    Content = "Огрмное спасиюо, все заработало",
                },
                new Comment
                {
                    Id = 12,
                    AuthorId = 6,
                    PostId = 5,
                    Content = "мдааа без комментариев",
                },
                new Comment
                {
                    Id = 13,
                    AuthorId = 4,
                    PostId = 4,
                    Content = "привет)))))",
                },
                new Comment
                {
                    Id = 14,
                    AuthorId = 3,
                    PostId = 3,
                    Content = "НЫЫЫЫААА",
                },
                new Comment
                {
                    Id = 15,
                    AuthorId = 2,
                    PostId = 2,
                    Content = "Не согласен!",
                });
        }

        private static void InitializeDepartments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new
                {
                    Id = 1,
                    Level = 1,
                    Name = "МАИ",
                    Code = "МАИ",
                    IsDeleted = false
                },
                new
                {
                    Id = 2,
                    Level = 2,
                    Name = "Авиационная техника",
                    Code = "Институт №1",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 3,
                    Level = 2,
                    Name = "Авиационные, ракетные двигатели и энергетические установки",
                    Code = "Институт №2",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 4,
                    Level = 2,
                    Name = "Системы управления, информатика и электроэнергетика",
                    Code = "Институт №3",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 5,
                    Level = 2,
                    Name = "Радиоэлектроника, инфокоммуникации и информационная безопасность",
                    Code = "Институт №4",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 6,
                    Level = 2,
                    Name = "Инженерная экономика и гуманитарные науки",
                    Code = "Институт №5",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 7,
                    Level = 2,
                    Name = "Аэрокосмический",
                    Code = "Институт №6",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 8,
                    Level = 2,
                    Name = "Робототехнические и интеллектуальные системы",
                    Code = "Институт №7",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 9,
                    Level = 2,
                    Name = "Информационные технологии и прикладная математика",
                    Code = "Институт №8",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 10,
                    Level = 2,
                    Name = "Институт общеинженерной подготовки",
                    Code = "Институт №9",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 11,
                    Level = 2,
                    Name = "Институт иностранных языков",
                    Code = "Институт №10",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 12,
                    Level = 2,
                    Name = "Институт материаловедения и технологий материалов",
                    Code = "Институт №11",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 13,
                    Level = 2,
                    Name = "Аэрокосмические наукоёмкие технологии и производства",
                    Code = "Институт №12",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 14,
                    Level = 2,
                    Name = "Военный институт МАИ",
                    Code = "Военный институт МАИ",
                    ParentId = 1,
                    IsDeleted = false
                },
                new
                {
                    Id = 15,
                    Level = 3,
                    Name = "Системы автоматического и интеллектуального управления",
                    Code = "Кафедра 301",
                    ParentId = 4,
                    IsDeleted = false
                },
                new
                {
                    Id = 16,
                    Level = 3,
                    Name = "Вычислительные машины, системы и сети",
                    Code = "Кафедра 304",
                    ParentId = 4,
                    IsDeleted = false
                },
                new
                {
                    Id = 17,
                    Level = 3,
                    Name = "Системное моделирование и автоматизированное проектирование",
                    Code = "Кафедра 316",
                    ParentId = 4,
                    IsDeleted = false
                },
                new
                {
                    Id = 18,
                    Level = 4,
                    Name = "Группа Т3О-408Б-16",
                    Code = "Т3О-408Б-16",
                    ParentId = 17,
                    IsDeleted = false
                },
                new
                {
                    Id = 19,
                    Level = 4,
                    Name = "Группа Т3О-407Б-16",
                    Code = "Т3О-407Б-16",
                    ParentId = 17,
                    IsDeleted = false
                }
            );
        }

    }

}
