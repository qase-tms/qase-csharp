using FluentAssertions;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class AppUrlResolverTests
    {
        [Theory]
        [InlineData("qase.io", "https://app.qase.io/")]
        [InlineData("eu.qase.io", "https://app-eu.qase.io/")]
        [InlineData("EU.qase.io", "https://app-eu.qase.io/")]
        [InlineData("example.qase.io", "https://example.qase.io/")]
        [InlineData("test.example.com", "https://test.example.com/")]
        [InlineData(null, "https://app.qase.io/")]
        [InlineData("", "https://app.qase.io/")]
        public void Resolve_ReturnsWebAppUrlForHost(string? host, string expected)
        {
            AppUrlResolver.Resolve(host).Should().Be(expected);
        }
    }
}
