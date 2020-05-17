using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InsideMai.Data;
using InsideMai.Models;
using InsideMai.Services;
using InsideMai.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly InsideMaiContext _context;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SearchEngine _searchEngine;

        public PostsController(InsideMaiContext context, CurrentUser currentUser,
            UserManager<User> userManager, IMapper mapper, SearchEngine search)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
            _searchEngine = search;
        }

        private IQueryable<Post> AllPosts
        {
            get
            {
                return _context.Posts.Where(p => p.IsDeleted == false)
                    .Include(p => p.Author)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                    .Include(p => p.Department)
                    .OrderByDescending(p => p.PublishDate);
            }
        }

        // GET api/posts
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        { 
            var posts = await AllPosts.ToListAsync();

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Ok(viewModel);
        }

        // GET api/posts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostsById([FromRoute] int id)
        {
            var post = await AllPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return BadRequest("Пост не найден");
            }

            var viewModel = _mapper.Map<UserPostViewModel>(post);
            await FillPostWithUserReactions(new List<PostViewModel> { viewModel });

            return Ok(viewModel);
        }

        // POST api/posts
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Posts.Any(p => p.Content == post.Content))
            {
                return BadRequest("Такой пост уже существует");
            }

            post.PublishDate = DateTime.Now;
            post.Author = await _currentUser.GetCurrentUser(HttpContext);

            _context.Posts.Add(post);
            if (!post.IsAnonymous)
            {
                await NotifySubscribers(post);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task NotifySubscribers(Post post)
        {
            var notificationsOfNewPosts = new List<NotificationsOfNewPosts>();

            var currentUser = await _currentUser.GetCurrentUser(HttpContext);
            var subscribers = await _context.SubscribersObservables.Where(so => so.Observable == currentUser)
                .Select(os => os.Observable).ToListAsync();

            foreach (var subscriber in subscribers)
            {
                notificationsOfNewPosts.Add(new NotificationsOfNewPosts()
                {
                    Post = post,
                    User = subscriber
                });
            }

            await _context.AddRangeAsync(notificationsOfNewPosts);
        }

        // GET api/posts/notified
        [HttpGet("notified")]
        public async Task<IActionResult> GetNotifiedPosts()
        {
            var currentUser = await _currentUser.GetCurrentUser(HttpContext);
            var notificationsOfNewPosts =
                await _context.NotificationsOfNewPosts.Where(np => np.User == currentUser).Include(np => np.Post)
                    .ToListAsync();

            var posts = notificationsOfNewPosts.Select(np => np.Post).OrderByDescending(p => p.PublishDate);

            await ClearNotification(notificationsOfNewPosts);

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Ok(viewModel);
        }

        private async Task ClearNotification(IEnumerable<NotificationsOfNewPosts> posts)
        {
            _context.NotificationsOfNewPosts.RemoveRange(posts);
            await _context.SaveChangesAsync();
        }

        // GET api/posts/all/like
        [HttpGet("all/like")]
        public async Task<IActionResult> GetAllPostsByLikes()
        {
            var posts = await AllPosts.ToListAsync();
            var result = posts.OrderByDescending(p => p.PostLikes);

            var viewModel = _mapper.Map<List<PostViewModel>>(result);

            return Ok(viewModel);
        }

        // GET api/posts/filter/2/3
        [HttpGet("filter/{type}/{departmentLevel}")]
        public async Task<IActionResult> GetFilteredPosts([FromRoute] int type, [FromRoute] int departmentLevel)
        {
            var currentUser = await _currentUser.GetCurrentUser(HttpContext);
            var currentUserDepartmentId = currentUser.DepartmentId;

            var department = await SearchDepartmentByLevel(currentUserDepartmentId, departmentLevel);

            if (department == null)
            {
                return BadRequest("Нет доступа");
            }

            var posts = await AllPosts
                .OrderByDescending(p => p.PublishDate)
                .ToListAsync();

            if ((DepartmentLevel) departmentLevel != DepartmentLevel.MaiLvl)
            {
                posts = posts.Where(p => p.Department?.Id == department.Id).ToList();
            }

            if ((PostType) type != PostType.All)
            {
                posts = posts.Where(p => p.Type == (PostType) type).ToList();
            }

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);
            await FillPostWithUserReactions(viewModel);

            return Ok(viewModel);
        }

        private async Task<Department> SearchDepartmentByLevel(int? currentDepartmentId, int departmentLevel)
        {
            var allDepartments = await _context.Departments.ToListAsync();
            var currentDepartment = allDepartments.FirstOrDefault(d => d.Id == currentDepartmentId);

            while (currentDepartment?.ParentId != null && currentDepartment.Level != departmentLevel)
            {
                currentDepartment = allDepartments.FirstOrDefault(d => d.Id == currentDepartment.ParentId);
            }

            return currentDepartment;
        }

        private async Task FillPostWithUserReactions(IEnumerable<PostViewModel> model)
        {
            var user = await _currentUser.GetCurrentUser(HttpContext);
            var userLikes = await _context.UserPostLike.Where(l => l.UserId == user.Id).ToListAsync();
            var userFavorites = await _context.Favorites.Where(l => l.UserId == user.Id).ToListAsync();

            foreach (var post in model)
            {
                post.LikedByCurrentUser = (userLikes.Any(l => l.PostId == post.Id));
                post.AddedToFavByCurrentUser = (userFavorites.Any(l => l.PostId == post.Id));
            }
        }

        // GET api/posts/type/2
        [HttpGet("type/{id}")]
        public async Task<IActionResult> GetPostsByType([FromRoute] PostType type)
        {
            var posts = await AllPosts.Where(p => p.Type == type).ToListAsync();
            var result = posts.OrderByDescending(p => p.PublishDate);

            return Ok(_mapper.Map<List<PostViewModel>>(result));
        }

        // GET api/posts/departments/like
        [HttpGet("departments/like")]
        public async Task<IActionResult> GetDepartmentPostsByLikes([FromRoute] int id)
        {
            var posts = await AllPosts.Where(p => p.Department.Id == id).ToListAsync();
            var result = posts.OrderByDescending(p => p.PostLikes);

            var viewModel = _mapper.Map<List<PostViewModel>>(result);

            return Ok(viewModel);
        }

        // GET api/posts/5/comments
        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments([FromRoute] int id)
        {
            var post = await AllPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return BadRequest("Пост не найден");
            }

            var comments = post.Comments;

            var viewModel = _mapper.Map<List<CommentViewModel>>(comments);
            await FillCommentsLikesStatus(viewModel);

            return Ok(viewModel);
        }

        private async Task FillCommentsLikesStatus(IEnumerable<CommentViewModel> model)
        {
            var user = await _currentUser.GetCurrentUser(HttpContext);
            var userLikes = await _context.UserCommentLikes.Where(l => l.UserId == user.Id).ToListAsync();

            foreach (var post in model)
            {
                post.LikedByCurrentUser = (userLikes.Any(l => l.CommentId == post.Id));
            }
        }

        // GET api/posts/user/4
        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetUserPosts([FromRoute] int userId)
        {
            var posts = await AllPosts.Where(p => p.Author.Id == userId && !p.IsAnonymous).ToListAsync();

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);
            await FillPostWithUserReactions(viewModel);

            return Ok(viewModel);
        }

        // GET api/posts/user/4/fav
        [HttpGet("user/{userid}/fav")]
        public async Task<IActionResult> GetUserFavPosts([FromRoute] int userId)
        {
            var allPosts = await AllPosts.ToListAsync();
            var userFavorites = await _context.Favorites.Where(l => l.UserId == userId).ToListAsync();
            var favPosts = allPosts.Where(p => userFavorites.Any(f => f.PostId == p.Id));

            var viewModel = _mapper.Map<List<PostViewModel>>(favPosts);
            await FillPostWithUserReactions(viewModel);

            return Ok(viewModel);
        }

        // DELETE api/posts/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var post = await AllPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return BadRequest("Пост не найден");
            }

            post.IsDeleted = true;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET api/posts/4/10
        [HttpGet("{startId}/{count}")]
        public async Task<IActionResult> GetSomePosts([FromRoute] int startId, [FromRoute] int count)
        {
            var posts = await AllPosts.Where(p => p.Id == startId).Take(count).ToArrayAsync();

            return Ok(posts);
        }

        // GET api/posts/search/apple
        [HttpGet("search/{terms}")]
        public async Task<IActionResult> GetPostByTitle([FromRoute] string terms)
        {
            var allPosts = await AllPosts.ToListAsync();
            var result = _searchEngine.SearchPosts(allPosts, terms);

            var viewModel = _mapper.Map<List<PostViewModel>>(result);

            return Ok(viewModel);
        }

        // GET api/posts/3/addComment
        [HttpPost("{id}/addComment")]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound($"Пост не найден");

            post.CommentsCount++;

            comment.Author = await _currentUser.GetCurrentUser(HttpContext);
            comment.Post = post;
            comment.PublishDate = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<CommentViewModel>(comment);

            return Ok(viewModel);
        }

        // GET api/posts/3/like
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost([FromRoute] int id)
        {
            if (!await PostExist(id))
                return BadRequest($"Пост не найден");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var like = await _context.UserPostLike.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.PostId == id);
            var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);

            if (like != null)
                return BadRequest("Вы уже поставили лайк");

            like = new UserPostLike {UserId = user.Id, PostId = id};
            _context.UserPostLike.Add(like);
            post.LikesCount++;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/posts/3/like
        [HttpDelete("{id}/like")]
        public async Task<IActionResult> RemoveLike([FromRoute] int id)
        {
            if (!await PostExist(id))
                return BadRequest($"Комментарий не найден");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var like = await _context.UserPostLike.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.PostId == id);
            var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);

            if (like == null)
                return BadRequest("Не удалось убрать лайк");

            _context.UserPostLike.Remove(like);
            post.LikesCount--;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST api/posts/3/fav
        [HttpPost("{id}/fav")]
        public async Task<IActionResult> FavoritePost([FromRoute] int id)
        {
            if (!await PostExist(id))
                return BadRequest($"Пост не найден");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var fav = await _context.Favorites.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.PostId == id);
            var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);

            if (fav != null)
                return BadRequest("Пост уже добавлен в избранное");

            fav = new Favorite {UserId = user.Id, PostId = id};
            _context.Favorites.Add(fav);
            post.SavesCount++;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/posts/3/fav
        [HttpDelete("{id}/fav")]
        public async Task<IActionResult> RemoveFavorite([FromRoute] int id)
        {
            if (!await PostExist(id))
                return BadRequest($"Пост не существует");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var fav = await _context.Favorites.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.PostId == id);
            var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);

            if (fav == null)
                return BadRequest("Ошибка.");

            _context.Favorites.Remove(fav);
            post.SavesCount--;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private Task<bool> PostExist(int id)
        {
            return _context.Posts.AnyAsync(e => e.Id == id);
        }
    }
}
