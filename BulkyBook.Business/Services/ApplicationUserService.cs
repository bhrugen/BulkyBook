using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
