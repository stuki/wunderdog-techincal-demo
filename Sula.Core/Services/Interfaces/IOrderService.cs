using System.Collections.Generic;
using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync(OrderStatus? status);
        Task CreateOrderAsync(string stripeSessionId, int requestQuantity);
        Task AddUserToOrderAsync(string stripeSessionId, ApplicationUser user);
        Task ChangeOrderStatusAsync(int orderId, OrderStatus status);
        Task ChangeOrderStatusAsync(string stripeSessionId, OrderStatus status);
    }
}