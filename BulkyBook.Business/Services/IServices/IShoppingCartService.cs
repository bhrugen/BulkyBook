using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services.IServices
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart?> GetCartByIdAsync(int cartId);
        Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
        Task<ShoppingCart> AddToCartAsync(ShoppingCart cart);
        Task UpdateCartAsync(ShoppingCart cart);
        Task ClearCartAsync(string userId);

    }
}
