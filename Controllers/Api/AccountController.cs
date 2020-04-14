using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InsideMai.Data;
using InsideMai.Models;
using InsideMai.Models.Configuration;
using InsideMai.Services;
using InsideMai.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InsideMai.Controllers.Api
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly InsideMaiContext _context;
        private readonly CurrentUser _currentUser;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AccountController(UserManager<User> userManager, InsideMaiContext context, IOptions<JwtSettings> jwtSettings, SignInManager<User> signInManager, CurrentUser currentUser)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings;
            _signInManager = signInManager;
            _currentUser = currentUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            string[] allowedDomains = {"gmail.com"};
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!allowedDomains.Any(d => model.Email.EndsWith("@" + d)))
                return BadRequest("Пожалуйста используйте другой домен");

            var user = new User
            {
                Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName,
                UserPic = "images/default.png"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return BadRequest("Пользователь не найден");

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                await _signInManager.SignInAsync(user, true);
                return Ok(new { token });
            }
            else
                return BadRequest("Неверная почта или пароль");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest($"Пользователь с ID {userId} не найден!");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Redirect("/authorization/login");
            return BadRequest("Код подтверждения неверный");
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _currentUser.GetCurrentUser(HttpContext));
        }
    }
}
