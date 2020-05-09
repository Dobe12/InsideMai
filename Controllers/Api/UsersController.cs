using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using InsideMai.Data;
using InsideMai.Models;
using InsideMai.Services;
using InsideMai.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly InsideMaiContext _context;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public UsersController(InsideMaiContext context, CurrentUser currentUser,
            UserManager<User> userManager, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        private IQueryable<User> AllUsers
        {
            get
            {
                return _context.Users.Where(u => u.IsDeleted == false)
                    .Include(u => u.Department)
                    .Include(u => u.Subscribers);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await AllUsers.ToArrayAsync();

            var viewModel = _mapper.Map<List<UserViewModel>>(users);

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var allUsers = await AllUsers.ToArrayAsync();
            var user = allUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("Пользователь не найден");
            }

            var viewModel = _mapper.Map<UserViewModel>(user);

            viewModel.IsSubscribe = await IsSubscribeOnUser(user);
            if (user == await _currentUser.GetCurrentUser(HttpContext))
            {
                viewModel.NotificationsCount =
                    await _context.NotificationsOfNewPosts.Where(np => np.User == user).CountAsync();
            }
            

            return Ok(viewModel);
        }

        private async Task<bool> IsSubscribeOnUser(User user)
        {
            var currentUser = await _currentUser.GetCurrentUser(HttpContext);

            var isSubscribe = await _context.SubscribersObservables.FirstOrDefaultAsync(so =>
                so.SubscriberId == currentUser.Id
                && so.ObservableId == user.Id);

            return isSubscribe != null;
        }

        [HttpGet("{id}/email")]
        public async Task<IActionResult> SearchUser([FromRoute] string email)
        {
            var user = await AllUsers.ToArrayAsync();
            var result = user.FirstOrDefault(u => u.Email == email);

            if (result == null)
            {
                return BadRequest("Пользователь не найден");
            }

            var viewModel = _mapper.Map<UserViewModel>(result);

            return Ok(viewModel);
        }

        [HttpGet("{departmentId}/department")]
        public async Task<IActionResult> UsersByDepartment([FromRoute] int departmentId)
        {
            if (! await _context.Departments.AnyAsync(d => d.Id == departmentId))
            {
                return BadRequest("Департамент не найден");
            }

            var users = await AllUsers.ToArrayAsync();
            var result = users.Where(u => u.Department.Id == departmentId);

            var viewModel = _mapper.Map<UserViewModel>(result);

            return Ok(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest("Пользователь не найден");
            }

            var currentUser = await _currentUser.GetCurrentUser(HttpContext);
            if (id != currentUser.Id || !await _userManager.IsInRoleAsync(currentUser, Models.User.Roles.Admin.ToString()))
                return BadRequest("Недостаточно прав");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<UserViewModel>(user);

            return Ok(viewModel);
        }

        [HttpPost("{userId}/subscribe")]
        public async Task<IActionResult> SubscribeOnUser([FromRoute] int userId)
        {
            var observableUser = await _context.Users.FindAsync(userId);

            if (observableUser == null)
            {
                return NotFound("Пользователь не найден");
            }

            var currentUser = await _currentUser.GetCurrentUser(HttpContext);

            if (currentUser == null)
            {
                return NotFound("Недостаточно прав");
            }

            var subscribe = new SubscribersObservables()
                { Subscriber = currentUser, Observable = observableUser };

            _context.SubscribersObservables.Add(subscribe);
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpPost("{userId}/unsubscribe")]
        public async Task<IActionResult> UnsubscribeOnUser([FromRoute] int userId)
        {
            var observableUser = await _context.Users.FindAsync(userId);

            if (observableUser == null)
            {
                return NotFound("Пользователь не найден");
            }

            var currentUser = await _currentUser.GetCurrentUser(HttpContext);

            if (currentUser == null)
            {
                return NotFound("Недостаточно прав");
            }

            var subscribe = await _context.SubscribersObservables
                .FirstOrDefaultAsync(so => so.ObservableId == observableUser.Id 
                                           && so.SubscriberId == currentUser.Id);

            if (subscribe == null)
            {
                return BadRequest("Вы уже отписаны");
            }

            _context.SubscribersObservables.Remove(subscribe);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _context.Users.AnyAsync(u => u.Id == id))
                return NotFound($"Пользователь не найден");

            var currentUser = await _currentUser.GetCurrentUser(HttpContext);
            if (user.Id != currentUser.Id)
                return BadRequest("Недостаточно прав");

            if (currentUser.UserPic != user.UserPic)
            {
                user.UserPic = UploadUserPic(user.UserPic);
            }

            currentUser.Department = user.Department;
            currentUser.Email = user.Email;
            currentUser.UserPic = user.UserPic;
            currentUser.Role = user.Role;

            //_context.Users.Update(currentUser);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<UserViewModel>(user);

            return Ok(viewModel);
        }

        private string UploadUserPic(string base64Img)
        {
            base64Img = base64Img.Replace("data:image/png;base64,", String.Empty);

            byte[] imageBytes = Convert.FromBase64String(base64Img);

            if (imageBytes.Length > 0)
            {
                var picName = Guid.NewGuid().ToString() + ".png";
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, picName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                    stream.Flush();
                }

                return picName;
            }

            return String.Empty;
        }
    }
}
