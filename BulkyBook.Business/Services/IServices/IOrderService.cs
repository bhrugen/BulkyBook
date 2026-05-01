using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);

        Task<OrderHeader?> GetOrderByIdAsync(int id, bool includeUser = false, bool includeDetails = false);

        Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId=null,string? status=null, bool includeUser = false, bool includeDetails = false);

        Task UpdateOrderAsync(OrderHeader orderHeader);

        Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null);

        Task UpdateStripePaymentAsync(int orderId, string sessionId, string paymentIntentId);

        Task<bool> CancelOrderWithRefundAsync(int orderId);

        Task<string> CreateStripeCheckoutSessionAsync(OrderHeader orderHeader, IEnumerable<ShoppingCart> cartItems, string domain);

        Task<bool> VerifyStripePaymentAsync(OrderHeader orderHeader);

    }
}
