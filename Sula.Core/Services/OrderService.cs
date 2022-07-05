using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _databaseContext;

        public OrderService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<Order>> GetOrdersAsync(OrderStatus? status)
        {
            var ordersQuery = _databaseContext.Orders.AsQueryable();

            if (status != null)
            {
                ordersQuery = ordersQuery.Where(order => order.Status == status);
            }

            return await ordersQuery.ToListAsync();
        }

        public async Task CreateOrderAsync(string stripeSessionId, int requestQuantity)
        {
            var order = new Order(stripeSessionId, requestQuantity);

            await _databaseContext.Orders.AddAsync(order);

            await _databaseContext.SaveChangesAsync();
        }
        
        public async Task AddUserToOrderAsync(string stripeSessionId, ApplicationUser user)
        {
            var order = await _databaseContext.GetOrderByStripeSessionId(stripeSessionId);

            if (order == null)
            {
                return;
            }

            order.Customer = user;
            order.UpdatedAt = DateTimeOffset.Now;

            await _databaseContext.SaveChangesAsync();
        }
        
        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _databaseContext.GetOrderById(orderId);

            if (order == null)
            {
                throw new OrderDoNotExistException();
            }

            order.Status = status;
            order.UpdatedAt = DateTimeOffset.Now;

            await _databaseContext.SaveChangesAsync();
        }
        
        public async Task ChangeOrderStatusAsync(string stripeSessionId, OrderStatus status)
        {
            var order = await _databaseContext.GetOrderByStripeSessionId(stripeSessionId);

            if (order == null)
            {
                return;
            }

            order.Status = status;
            order.UpdatedAt = DateTimeOffset.Now;

            await _databaseContext.SaveChangesAsync();
        }
    }
}