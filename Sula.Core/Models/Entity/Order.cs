using System;
using System.Collections.Generic;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public ApplicationUser Customer { get; set; }
        public OrderStatus Status { get; set; }
        public string StripeSessionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; }

        public Order()
        {
        }

        public Order(string stripeSessionId, int quantity)
        {
            StripeSessionId = stripeSessionId;
            Status = OrderStatus.None;
            CreatedAt = DateTimeOffset.Now;
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Type = ItemType.Airwits,
                    Quantity = quantity
                }
            };
        }
    }
}