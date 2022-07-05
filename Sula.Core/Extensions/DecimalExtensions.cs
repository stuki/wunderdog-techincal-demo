using Sula.Core.Models;
using Sula.Core.Models.Entity;

namespace Sula.Core.Extensions
{
    public static class DecimalExtensions {
        public static bool CompareWith(this decimal value, Limit limit)
        {
            return limit.Operator.Compare(value, limit.Value);
        }
    }
}