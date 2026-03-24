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

        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var orders = await _db.OrderHeaders.ToListAsync();
            var products = await _db.Products.Include(u=>u.Category).ToListAsync();
            var categories = await _db.Categories.ToListAsync();


            //Revenue by month(last 6 months)
            var now = DateTime.UtcNow;
            var sixMonthsAgo = now.AddMonths(-5);
            var monthlyRevenue = Enumerable.Range(0, 6).Select(i =>
            {
                var month = sixMonthsAgo.AddMonths(i);
                var revenue = orders.Where(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month
                && (o.OrderStatus == SD.StatusApproved || o.OrderStatus == SD.StatusShipped)).Sum(o => o.OrderTotal);

                return new { Label = month.ToString("MMM yyyy"), Revenue = revenue };
            }).ToList();

            return Json(new
            {
                monthlyRevenue,
            });
        }
    }
}
