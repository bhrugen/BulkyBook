using BulkyBook.Models.ViewModels;
using BulkyBook.Utiltiy;
using BulkyBookWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _db.OrderHeaders.ToListAsync();
            var productCount = await _db.Products.CountAsync();
            var usersCount = await _db.ApplicationUsers.CountAsync();

            DashboardVM dashboardVM = new()
            {
                TotalOrders = orders.Count,
                TotalProducts = productCount,
                TotalUsers = usersCount,
                TotalRevenue = orders.Where(o => o.OrderStatus == SD.StatusApproved || o.OrderStatus == SD.StatusShipped)
                .Sum(o => o.OrderTotal)
            };
            return View(dashboardVM);
        }
    }
}
