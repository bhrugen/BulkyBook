using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
