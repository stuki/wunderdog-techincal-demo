using Sula.Core.Models.Support;

namespace Sula.Core.Models.Request
{
    public class SensorLimitAddRequest
    {
        public DataType DataType { get; set; }
        public Operator Operator { get; set; }
        public decimal? Value { get; set; }
        public bool IsEnabled { get; set; }
    }
}