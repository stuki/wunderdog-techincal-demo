using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class OrderItem
    {
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public int Quantity { get; set; }
    }
}