using BulkyBook.Models;
using BulkyBook.Utiltiy;
using BulkyBookWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if((await _db.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _db.Database.MigrateAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }


            if (!await _roleManager.RoleExistsAsync(SD.RoleCustomer))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee));
            }

            ApplicationUser user = await _userManager.FindByEmailAsync("admin@dotnetmastery.com");
            if (user == null)
            {
                var result = await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@dotnetmastery.com",
                    Email = "admin@dotnetmastery.com",
                    EmailConfirmed = true,
                    Name = "Bhrugen Patel",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 Ave",
                    State = "IL",
                    PostalCode = "23422",
                    City = "Chicago"
                }, "Admin123*");

                if (result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync("admin@dotnetmastery.com");
                    await _userManager.AddToRoleAsync(user, SD.RoleAdmin);
                }
            }

        }
    }
}
