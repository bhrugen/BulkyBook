using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utiltiy;
using BulkyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BulkyBook.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CancelOrderWithRefundAsync(int orderId)
        {
            var order = await _db.OrderHeaders.FindAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order {orderId} not found");
            }

            if (order.OrderStatus == SD.StatusShipped)
            {
                throw new InvalidOperationException("Cannot cancel orders that have already been shipped. Customer must initiate a return instead.");
            }

            // Check if already cancelled or refunded
            if (order.OrderStatus == SD.StatusCancelled || order.OrderStatus == SD.StatusRefunded)
            {
                throw new InvalidOperationException("This order has already been cancelled.");
            }

            bool refundIssued = false;
            if (!string.IsNullOrEmpty(order.PaymentIntentId) &&
                (order.OrderStatus == SD.StatusApproved ||
                 order.OrderStatus == SD.StatusInProcess))
            {

                try
                {
                    //refund
                    var options = new RefundCreateOptions { PaymentIntent = order.PaymentIntentId, Reason= RefundReasons.RequestedByCustomer
                    };
                    var service = new RefundService();
                    Refund refund = service.Create(options);

                    if(refund.Status== "succeeded" || refund.Status== "pending")
                    {
                        refundIssued = true;
                        order.OrderStatus = SD.StatusRefunded;
                    }
                }
                catch(StripeException ex)
                {
                    order.OrderStatus = SD.StatusCancelled;
                    await _db.SaveChangesAsync();
                    throw new InvalidOperationException($"Stripe refund failed: {ex.Message}. Order has been cancelled, but refund must be processed manually.", ex);

                }

            }

            await _db.SaveChangesAsync();
            return refundIssued;

        }

        public async Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
        {
           _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();

            return orderHeader;
        }

        public async Task<string> CreateStripeCheckoutSessionAsync(OrderHeader orderHeader, IEnumerable<ShoppingCart> cartItems, string domain)
        {
            if (orderHeader == null)
            {
                throw new ArgumentNullException(nameof(orderHeader));
            }

            if (cartItems == null || !cartItems.Any())
            {
                throw new ArgumentException("Cart items cannot be empty", nameof(cartItems));
            }

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={orderHeader.Id}",
                CancelUrl = domain + "customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                Metadata = new Dictionary<string, string>
                        {
                            { "OrderId", orderHeader.Id.ToString() }
                        }
            };


            foreach (var item in cartItems)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            await UpdateStripePaymentAsync(orderHeader.Id, session.Id, session.PaymentIntentId);
            return session.Url;
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

        public async Task UpdateStripePaymentAsync(int orderId, string sessionId, string paymentIntentId)
        {
            var order = await _db.OrderHeaders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order {orderId} not found");
            }
            if (!string.IsNullOrEmpty(sessionId))
            {
                order.SessionId=sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                order.PaymentIntentId = paymentIntentId;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<bool> VerifyStripePaymentAsync(OrderHeader orderHeader)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                await UpdateStripePaymentAsync(orderHeader.Id, session.Id, session.PaymentIntentId);
                await UpdateOrderStatusAsync(orderHeader.Id, SD.StatusApproved);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
