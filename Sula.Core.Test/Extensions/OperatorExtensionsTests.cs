using System.Threading.Tasks;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Support;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class OperatorExtensionsTests 
    {
        [Fact]
        public void LessThanOperator_ShouldReturnTrue_When_Operator_IsLessThan_Value()
        {
            var @operator = Operator.LessThan;
            Assert.True(@operator.Compare(9, 10));
        }
        
        [Fact]
        public void LessThanOperator_ShouldReturnFalse_When_Operator_IsMoreThan_Value()
        {
            var @operator = Operator.LessThan;
            Assert.False(@operator.Compare(11, 10));
        }
        
        [Fact]
        public void LessThanOperator_ShouldReturnFalse_When_Operator_EqualTo_Value()
        {
            var @operator = Operator.LessThan;
            Assert.False(@operator.Compare(10, 10));
        }

        [Fact]
        public void MoreThanOperator_ShouldReturnTrue_When_Operator_IsMoreThan_Value()
        {
            var @operator = Operator.MoreThan;
            Assert.True(@operator.Compare(11, 10));
        }

        [Fact]
        public void MoreThanOperator_ShouldReturnFalse_When_Operator_IsLessThan_Value()
        {
            var @operator = Operator.MoreThan;
            Assert.False(@operator.Compare(9, 10));
        }

        [Fact]
        public void MoreThanOperator_ShouldReturnFalse_When_Operator_IsEqualTo_Value()
        {
            var @operator = Operator.MoreThan;
            Assert.False(@operator.Compare(10, 10));
        }
        
        [Fact]
        public void EqualOperator_ShouldReturnTrue_When_Operator_IsEqualTo_Value()
        {
            var @operator = Operator.Equal;
            Assert.True(@operator.Compare(10, 10));
        }
        
        [Fact]
        public void EqualOperator_ShouldReturnFalse_When_Operator_IsLessThan_Value()
        {
            var @operator = Operator.Equal;
            Assert.False(@operator.Compare(9, 10));
        }
        
        [Fact]
        public void EqualOperator_ShouldReturnFalse_When_Operator_IsMoreTHan_Value()
        {
            var @operator = Operator.Equal;
            Assert.False(@operator.Compare(11, 10));
        }
        
        
        [Fact]
        public void Compare_ShouldReturnTrue_When_Limit_IsNull()
        {
            var @operator = Operator.Equal;
            Assert.True(@operator.Compare(10, null));
        }
    }
}