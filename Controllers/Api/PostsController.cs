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


        public PostsController(InsideMaiContext context, CurrentUser currentUser,
            UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await AllPosts.ToListAsync();

            return Ok(_mapper.Map<List<PostViewModel>>(posts));
        }

        [HttpGet("all/like")]
        public async Task<IActionResult> GetAllPostsByLikes()
        {
            var posts = await AllPosts.ToListAsync();

            var result = posts.OrderByDescending(p => p.PostLikes);

            var viewModel = _mapper.Map<List<PostViewModel>>(result);

            return Ok(viewModel);
        }

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

            while (currentDepartment.ParentId != null && currentDepartment.Level != departmentLevel)
            {
                currentDepartment = allDepartments.FirstOrDefault(d => d.Id == currentDepartment.ParentId);
            }

            return currentDepartment;
        }

        private async Task FillPostWithUserReactions(List<PostViewModel> model)
        {
            var user = await _currentUser.GetCurrentUser(HttpContext);
            var userLikes = await _context.UserPostLike.Where(l => l.UserId == user.Id).ToListAsync();
            var userFavorites = await _context.Favorites.Where(l => l.UserId == user.Id).ToListAsync();

            foreach (var post in model)
            {
                post.LikedByCurrentUser = (userLikes.Any(l => l.PostId == post.Id)) ? true : false;
                post.AddedToFavByCurrentUser = (userFavorites.Any(l => l.PostId == post.Id)) ? true : false;
            }
        }

        [HttpGet("type/{id}")]
        public async Task<IActionResult> GetPostsByType([FromRoute] PostType type)
        {
            var posts = await AllPosts.Where(p => p.Type == type).ToListAsync();

            var result = posts.OrderByDescending(p => p.PublishDate);

            return Ok(_mapper.Map<List<PostViewModel>>(result));
        }

        [HttpGet("all/like")]
        public async Task<IActionResult> GetDeparmentPostsByLikes([FromRoute] int id)
        {
            var posts = await AllPosts.Where(p => p.Department.Id == id).ToListAsync();

            var result = posts.OrderByDescending(p => p.PostLikes);

            var viewModel = _mapper.Map<List<PostViewModel>>(result);

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostsById([FromRoute] int id)
        {
            var post = await AllPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return BadRequest("Пост не найден");
            }

            var viewModel = _mapper.Map<UserPostViewModel>(post);
            await FillPostWithUserReactions(new List<PostViewModel> {viewModel});

            return Ok(viewModel);
        }

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

        private async Task FillCommentsLikesStatus(List<CommentViewModel> model)
        {
            var user = await _currentUser.GetCurrentUser(HttpContext);
            var userLikes = await _context.UserCommentLikes.Where(l => l.UserId == user.Id).ToListAsync();

            foreach (var post in model)
            {
                post.LikedByCurrentUser = (userLikes.Any(l => l.CommentId == post.Id)) ? true : false;
            }
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetPostsByUser([FromRoute] int userId)
        {
            var posts = await AllPosts.Where(p => p.Author.Id == userId).ToListAsync();

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Ok(viewModel);
        }

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

            return Ok();
        }

        [HttpGet("{startId}/{count}")]
        public async Task<IActionResult> GetSomePosts([FromRoute] int startId, [FromRoute] int count)
        {
            var posts = await AllPosts.Where(p => p.Id == startId).Take(count).ToArrayAsync();

            return Ok(posts);
        }

        //Доработать, не работает!!!!
        [HttpGet("search/{terms}")]
        public async Task<IActionResult> GetPostByTitle([FromRoute] string terms)
        {
            var searchedWords = terms.Split('%');

            var posts = await AllPosts.Select(post => new
                {
                    Post = post,
                    CountSearchedWords = searchedWords.Count(sw => sw.All(word => post.Title.Contains(word)))
                }).Where(post => post.CountSearchedWords > 0).OrderByDescending(cw => cw.CountSearchedWords)
                .Select(post => post.Post).ToListAsync();

            if (posts == null)
            {
                return BadRequest("Пост не найден");
            }

            var viewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Ok(viewModel);
        }

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
            await _context.SaveChangesAsync();

            return Ok();
        }

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
