using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMaiWebApi.Data;
using InsideMaiWebApi.Models;
using InsideMaiWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsideMaiWebApi.Controllers.Api
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

        /// <summary>
        /// Get all departments form DataBase
        /// </summary>
        /// <returns>All departments from DataBase</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            var departments = await AllDepartments.ToListAsync();

            return Ok(departments);
        }

        /// <summary>
        /// Add new department
        /// </summary>
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

        /// <summary>
        /// Get department by id
        /// </summary>
        /// <returns>department</returns>
        /// 
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



        /// <summary>
        /// Delete department
        /// </summary>
        /// <returns>status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletDeleteDepartmentePost([FromRoute] int id)
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
