using System;
using System.Threading.Tasks;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class DecimalExtensionsTests 
    {
        [Fact]
        public void CompareWith_ReturnsTrue_When_Value_Is_WithinLimits()
        {
            var value = decimal.One;
            var limit = new Limit
            {
                DataType = DataType.Humidity,
                Operator = Operator.LessThan,
                Value = 10
            };

            Assert.True(value.CompareWith(limit));
        }
    }
}