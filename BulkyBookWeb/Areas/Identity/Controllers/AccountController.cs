using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager= signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerVM.Email,
                    Email = registerVM.Email,
                    Name = registerVM.Name,
                    PhoneNumber = registerVM.PhoneNumber,
                    StreetAddress = registerVM.StreetAddress,
                    City = registerVM.City,
                    State = registerVM.State,
                    PostalCode = registerVM.PostalCode
                };


                var result = await _userManager.CreateAsync(user);


                if (result.Succeeded)
                {
                    //user has been created
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(registerVM);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
