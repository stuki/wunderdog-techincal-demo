using Sula.Core.Extensions;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("https://localhost/")]
        [InlineData("https://sula.app/")]
        [InlineData("https://localhost/api/thing/and/stuff/")]
        public void TrimUrl_Should_RemoveTrailingDash(string value)
        {
            Assert.False(value.TrimUrl().EndsWith("/"));
            Assert.Contains(value.TrimUrl(), value);
        }
    }
}