using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using InsideMaiWebApi.Data;
using InsideMaiWebApi.Models;
using InsideMaiWebApi.Services;
using InsideMaiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsideMaiWebApi.Controllers.Api
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
                    .Include(u => u.Department);
            }
        }

        /// <summary>
        /// Get all users form DataBase
        /// </summary>
        /// <returns>All users from DataBase</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await AllUsers.ToArrayAsync();

            return Ok(_mapper.Map<List<UserViewModel>>(users));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <returns>user by Id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await AllUsers.ToArrayAsync();
            var result = user.FirstOrDefault(u => u.Id == id);

            if (result == null)
            {
                return BadRequest("Пользователь не найден");
            }

            return Ok(_mapper.Map<UserViewModel>(result));
        }

        /// <summary>
        /// Get user by Email
        /// </summary>
        /// <returns>user by Email</returns>
        [HttpGet("{id}/email")]
        public async Task<IActionResult> SearchUser([FromRoute] string email)
        {
            var user = await AllUsers.ToArrayAsync();
            var result = user.FirstOrDefault(u => u.Email == email);

            if (result == null)
            {
                return BadRequest("Пользователь не найден");
            }

            return Ok(_mapper.Map<UserViewModel>(result));
        }

        /// <summary>
        /// Get users by Department
        /// </summary>
        /// <returns>users by Department</returns>
        [HttpGet("{departmentId}/department")]
        public async Task<IActionResult> UsersByDepartment([FromRoute] int departmentId)
        {
            if (! await _context.Departments.AnyAsync(d => d.Id == departmentId))
            {
                return BadRequest("Департамент не найден");
            }

            var users = await AllUsers.ToArrayAsync();
            var result = users.Where(u => u.Department.Id == departmentId);

            return Ok(_mapper.Map<UserViewModel>(result));
        }

        /// <summary>
        /// Delete user
        /// </summary>
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

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        /// <summary>
        /// Change user information
        /// </summary>
        /// <returns>current User</returns>
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

            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserViewModel>(user));
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
                //убрать хардкод, когда солью
                return "https://localhost:44350/" + picName;
            }

            return String.Empty;
        }
    }
}
