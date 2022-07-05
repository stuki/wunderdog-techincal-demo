using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Platform;

namespace Sula.Core.Extensions
{
    public static class DatabaseContextExtensions
    {
        public static async Task<Sensor> GetSensor(this DatabaseContext context, string sensorId, bool setTracking = true)
        {
            var query = context.Sensors.AsQueryable();

            if (!setTracking)
            {
                query = query.AsNoTracking();
            }
            
            return await query
                .Include(sensor => sensor.Limits)
                .SingleOrDefaultAsync(sensor => sensor.Id == sensorId);
        }

        public static async Task<Order> GetOrderById(this DatabaseContext context, int orderId)
        {
            return await context.Orders.FindAsync(orderId);
        }

        public static async Task<Order> GetOrderByStripeSessionId(this DatabaseContext context, string stripeSessionId)
        {
            return await context.Orders.SingleOrDefaultAsync(item => item.StripeSessionId == stripeSessionId);
        }
    }
}