using BulkyBook.Business.Services.IServices;
using BulkyBook.Utiltiy;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IShoppingCartService _shoppingCartService;

        public CartCountViewComponent(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Remove(SD.SessionCart);
                return View(0);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim= claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                HttpContext.Session.Remove(SD.SessionCart);
                return View(0);
            }

            var sessionCount = HttpContext.Session.GetInt32(SD.SessionCart);
            if (sessionCount.HasValue)
            {
                return View(sessionCount.Value);
            }

            var cartCount = await _shoppingCartService.GetCartCountAsync(claim.Value);
            HttpContext.Session.SetInt32(SD.SessionCart, cartCount);
            return View(cartCount);
        }
    }
}
