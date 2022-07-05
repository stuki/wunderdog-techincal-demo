using System;
using Sula.Core.Models;
using Sula.Core.Models.Support;

namespace Sula.Core.Extensions
{
    public static class OperatorExtensions {
        public static bool Compare(this Operator @operator, decimal actual, decimal? limit)
        {
            if (limit == null)
            {
                return true;
            }

            return @operator switch
            {
                Operator.LessThan => actual < limit,
                Operator.MoreThan => actual > limit,
                Operator.Equal => actual == limit,
                _ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null)
            };
        }
    }
}