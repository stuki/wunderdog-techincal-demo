using System;
using System.Collections.Generic;
using Sula.Core.Extensions;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class DateTimeOffsetExtensionsTests
    {
        public static IEnumerable<object[]> DatesCloseToMidnight =>
            new List<object[]>
            {
                new object[] {new DateTimeOffset(2020, 11, 2, 19, 1, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 20, 2, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 21, 3, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 22, 4, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 23, 5, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 0, 6, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 1, 7, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 2, 8, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 3, 9, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 4, 10, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 5, 11, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 6, 12, 0, new TimeSpan(0))},
            };

        public static IEnumerable<object[]> DatesCloseToMidday =>
            new List<object[]>
            {
                new object[] {new DateTimeOffset(2020, 11, 2, 7, 1, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 8, 2, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 9, 3, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 10, 4, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 11, 5, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 12, 6, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 13, 7, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 14, 8, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 15, 9, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 16, 10, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 17, 11, 0, new TimeSpan(0))},
                new object[] {new DateTimeOffset(2020, 11, 2, 18, 12, 0, new TimeSpan(0))},
            };

        [Theory]
        [MemberData(nameof(DatesCloseToMidnight))]
        public void IsCloseToMidnight_Should_ReturnTrue_ForValuesCloseToMidnight(DateTimeOffset time)
        {
            Assert.True(time.IsCloseToMidnight());
        }

        [Theory]
        [MemberData(nameof(DatesCloseToMidday))]
        public void IsCloseToMidnight_Should_ReturnFalse_ForValuesCloseToMidday(DateTimeOffset time)
        {
            Assert.False(time.IsCloseToMidnight());
        }

        [Theory]
        [MemberData(nameof(DatesCloseToMidnight))]
        public void ToMidnight_Should_ReturnFalse_ForValuesCloseToMidday(DateTimeOffset time)
        {
            var midnight = time.ToMidnight();
            Assert.Equal(0, midnight.Hour);
            Assert.Equal(0, midnight.Minute);
            Assert.Equal(0, midnight.Second);
        }

        [Fact]
        public void ToMidnight_Should_Return_CorrectNextDaysMidnight_If_Evening()
        {
            var midnight = new DateTimeOffset(2020, 1, 1, 19, 0, 0, new TimeSpan(0)).ToMidnight();
            Assert.Equal(2, midnight.Day);
            Assert.Equal(0, midnight.Hour);
            Assert.Equal(0, midnight.Minute);
            Assert.Equal(0, midnight.Second);
        }

        [Fact]
        public void ToMidnight_Should_Return_CorrectNextDaysMidnight_If_Morning()
        {
            var midnight = new DateTimeOffset(2020, 1, 1, 5, 0, 0, new TimeSpan(0)).ToMidnight();
            Assert.Equal(1, midnight.Day);
            Assert.Equal(0, midnight.Hour);
            Assert.Equal(0, midnight.Minute);
            Assert.Equal(0, midnight.Second);
        }

        [Theory]
        [MemberData(nameof(DatesCloseToMidday))]
        public void ToMidday_Should_ReturnFalse_ForValuesCloseToMidday(DateTimeOffset time)
        {
            var midnight = time.ToMidday();
            Assert.Equal(12, midnight.Hour);
            Assert.Equal(0, midnight.Minute);
            Assert.Equal(0, midnight.Second);
        }
    }
}