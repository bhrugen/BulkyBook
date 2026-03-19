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

        public async Task<ShoppingCart> AddToCartAsync(ShoppingCart cart)
        {
            var existingItem = await _context.ShoppingCarts.Include(u => u.Product)
                .FirstOrDefaultAsync(u => u.ApplicationUserId== cart.ApplicationUserId && u.ProductId==cart.ProductId);

            if (existingItem != null)
            {
                existingItem.Count += cart.Count;
                await _context.SaveChangesAsync();
                return existingItem;
            }
            else
            {
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
                return cart;
            }
        }

        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            if (cart.Count <= 0)
            {
                _context.ShoppingCarts.Remove(cart);
            }
            else
            {
                _context.ShoppingCarts.Update(cart);
            }
            await _context.SaveChangesAsync();
        }
    }
}
