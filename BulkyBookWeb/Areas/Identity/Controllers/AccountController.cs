using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utiltiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IShoppingCartService _shoppingCartService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IShoppingCartService shoppingCartService)
        {
            _userManager = userManager;
            _shoppingCartService= shoppingCartService;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email,
                    loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Redirect to returnUrl if valid, otherwise go to Home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    if(User.IsInRole(SD.RoleAdmin) || User.IsInRole(SD.RoleEmployee))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginVM);
        }

        public IActionResult Register(string? returnUrl = null)
        {

            var model = new RegisterVM();


            if (User.IsInRole(SD.RoleAdmin))
            {
                model.RoleList =
                    [
                        new SelectListItem{Text=SD.RoleCustomer, Value=SD.RoleCustomer},
                    new SelectListItem{Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                    new SelectListItem{Text=SD.RoleEmployee, Value=SD.RoleEmployee},
                ];
            }
            else
            {
                model.RoleList = new List<SelectListItem>();
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM, string? returnUrl = null)
        {
            if(!await _roleManager.RoleExistsAsync(SD.RoleCustomer))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee));
            }
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


                var result = await _userManager.CreateAsync(user,registerVM.Password);


                if (result.Succeeded)
                {
                    if(User.IsInRole(SD.RoleAdmin) && !string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                        TempData["success"] = $"User '{user.Name}' created successfully with role '{registerVM.Role}'.";
                        return RedirectToAction("Index", "User", new { area = "Admin" });
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.RoleCustomer);

                        //user has been created
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }

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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.SetInt32(SD.SessionCart, 0);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
