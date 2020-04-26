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
    public class CommentsController : Controller
    {
        private readonly InsideMaiContext _context;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public CommentsController(InsideMaiContext context, CurrentUser currentUser,
            UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
        }
        private IQueryable<Comment> AllComments
        {
            get
            {
                return _context.Comments.Where(c => c.IsDeleted == false)
                    .Include(c => c.Author)
                    .Include(p => p.Post)
                    .OrderByDescending(p => p.PublishDate);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return BadRequest("Комментарий не найден");
            if (!await CheckRights(comment))
                return BadRequest("Недостаточно прав");

            comment.IsDeleted = true;
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<CommentViewModel>(comment);

            return Ok(viewModel);
        }

        [HttpPost("{id}/edit")]
        public async Task<IActionResult> EditComment([FromRoute] int id, [FromBody] Comment comment)
        {
            if (!await CommentExists(id))
                return NotFound($"Комментарий не существует");

            comment.Id = id;

            if (!await CheckRights(comment))
                return BadRequest("Недостаточно прав");
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<CommentViewModel>(comment);

            return Ok(viewModel);
        }

        private async Task<bool> CheckRights(Comment comment)
        {
            string identityId = User.Claims.FirstOrDefault().Value;
            User user = await _context.Users.FirstAsync(u => u.Id.ToString() == identityId);

            if (comment.AuthorId == user.Id)
                return true;

            User identity = await _userManager.FindByIdAsync(identityId);

            var test = _userManager.GetRolesAsync(identity);
            var test2 = _userManager.GetRolesAsync(identity);

            if (identity.Role == Models.User.Roles.Admin)
                return true;

            return false;
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeComment([FromRoute] int id)
        {
            if (!await CommentExists(id))
                return BadRequest($"Комментарий не существует");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var like = await _context.UserCommentLikes.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.CommentId == id);
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);


            if (like != null)
                return BadRequest("Не удалось поставить лайк");

            like = new UserCommentLike {UserId = user.Id, CommentId = id};
            _context.UserCommentLikes.Add(like);
            comment.LikesCount++;
            
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/like")]
        public async Task<IActionResult> RemoveLike([FromRoute] int id)
        {
            if (!await CommentExists(id))
                return BadRequest($"Комментарий не найден");

            var user = await _currentUser.GetCurrentUser(HttpContext);
            var like = await _context.UserCommentLikes.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.CommentId == id);

            if (like == null)
                return BadRequest("Не удалось убрать лайк");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            _context.UserCommentLikes.Remove(like);
            comment.LikesCount--;
            
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private Task<bool> CommentExists(int id)
        {
            return _context.Comments.AnyAsync(e => e.Id == id);
        }
    }
}
