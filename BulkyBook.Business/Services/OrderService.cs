using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBook.Utiltiy;
using BulkyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
        {
           _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();

            return orderHeader;
        }

        public async Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId = null, string? status = null, bool includeUser = false, bool includeDetails = false)
        {
            var query = _db.OrderHeaders.AsQueryable();

            if (includeUser)
            {
                query = query.Include(u => u.ApplicationUser);
            }
            if (includeDetails)
            {
                query = query.Include(u => u.OrderDetails).ThenInclude(u => u.Product);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(u => u.ApplicationUserId == userId);
            }
            if (!string.IsNullOrEmpty(status) && status.ToLower()!="all")
            {
                query = query.Where(u => u.OrderStatus.ToLower() == status.ToLower());
            }
            return await query.ToListAsync();
        }

        public async Task<OrderHeader?> GetOrderByIdAsync(int id, bool includeUser = false, bool includeDetails = false)
        {
            var query = _db.OrderHeaders.AsQueryable();

            if (includeUser)
            {
                query = query.Include(u => u.ApplicationUser);
            }
            if (includeDetails)
            {
                query = query.Include(u => u.OrderDetails).ThenInclude(u=>u.Product);
            }

            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateOrderAsync(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null)
        {
            var order = await _db.OrderHeaders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order {id} not found");
            }
            order.OrderStatus = orderStatus;

            if (orderStatus == SD.StatusShipped)
            {
                order.ShippingDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(carrier))
                {
                    order.Carrier = carrier;
                }
                if (!string.IsNullOrEmpty(trackingNumber))
                {
                    order.TrackingNumber = trackingNumber;
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
