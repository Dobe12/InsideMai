using System.Linq;
using System.Threading.Tasks;
using InsideMai.Data;
using InsideMai.Models;
using InsideMai.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly InsideMaiContext _context;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<User> _userManager;
        public DepartmentsController(InsideMaiContext context, CurrentUser currentUser,
            UserManager<User> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        private IQueryable<Department> AllDepartments
        {
            get
            {
                return _context.Departments.Where(d => d.IsDeleted == false)
                    .Include(d => d.ParentId);
            }
        }

        // GET api/departments/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            var departments = await AllDepartments.ToListAsync();

            return Ok(departments);
        }

        // POST api/departments/all/5
        [HttpPost("all/{parentId}")]
        public async Task<IActionResult> AddNewDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (department.ParentId != null &&  !await DepartmentExists(department.ParentId))
            {
                return BadRequest($"Департамент не существует");
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        // GET api/departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> AddNewDepartment([FromRoute] int id)
        {
            if (!await DepartmentExists(id))
            {
                return BadRequest($"Департамент не существует");
            }

            var departments = await AllDepartments.ToArrayAsync();
            var result = departments.FirstOrDefault(d => d.Id == id);

            if (result == null)
            {
                return BadRequest($"Департамент не существует");
            }
            
            return Ok(result);
        }

        // DELETE api/departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentPost([FromRoute] int id)
        {
            var department = await AllDepartments.FirstOrDefaultAsync(p => p.Id == id);

            if (department == null)
            {
                return BadRequest("Департамент не найден");
            }

            department.IsDeleted = true;

            _context.Update(department);

            return Ok();
        }

        private Task<bool> DepartmentExists(int? id)
        {
            return _context.Departments.AnyAsync(e => e.Id == id);
        }
    }
}
