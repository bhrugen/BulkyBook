using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utiltiy;
using BulkyBookWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.WebRequestMethods;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;


        [BindProperty]
        public OrderHeader OrderHeader { get; set; }

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
           
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details( int orderId)
        {
            OrderHeader = await _orderService.GetOrderByIdAsync(orderId, includeDetails: true, includeUser: true);
            return View(OrderHeader);
        }


        #region API CALLS
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrderAsync();
            return Json(new { data = orders });
        }
       
        #endregion
    }
}
