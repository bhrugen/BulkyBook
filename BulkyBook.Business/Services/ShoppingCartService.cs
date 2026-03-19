using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.ShoppingCarts
                .Include(u => u.Product).Where(u => u.ApplicationUserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                _context.ShoppingCarts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ShoppingCart?> GetCartByIdAsync(int cartId)
        {
            return await _context.ShoppingCarts.Include(u=>u.Product).FirstOrDefaultAsync(u => u.Id == cartId);
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _context.ShoppingCarts.Where(u => u.ApplicationUserId == userId).SumAsync(u => u.Count);
        }

        public async Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId)
        {
            return await _context.ShoppingCarts.Include(u => u.Product).Where(u => u.ApplicationUserId == userId)
                .ToListAsync();
        }

        public Task<ShoppingCart> AddToCartAsync(ShoppingCart cart)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCartAsync(ShoppingCart cart)
        {
            throw new NotImplementedException();
        }
    }
}
