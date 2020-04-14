using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InsideMai.Data;
using InsideMai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InsideMai.Services
{
    public class CurrentUser
    {
        private readonly InsideMaiContext _insideMaiContext;

        public CurrentUser(InsideMaiContext insideMaiContext)
        {
            _insideMaiContext = insideMaiContext;
        }

        public async Task<User> GetCurrentUser(HttpContext context)
        {
            var userId = Int32.Parse(context.User.Claims.FirstOrDefault()?.Value);
            var user = await _insideMaiContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        
    }
}
