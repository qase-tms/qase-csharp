using Qase.Csharp.Commons.Utils;

namespace Qase.Csharp.Commons.Tests
{
    public class ClientHeadersBuilderTests
    {
        [Fact]
        public void BuildUserAgentHeader_ShouldContainQaseApiClientSubstring()
        {
            var userAgent = ClientHeadersBuilder.BuildUserAgentHeader();

            userAgent.Should().Contain("qase-api-client");
        }

        [Fact]
        public void BuildUserAgentHeader_ShouldMatchExpectedFormat()
        {
            var userAgent = ClientHeadersBuilder.BuildUserAgentHeader();

            userAgent.Should().MatchRegex(@"^qase-api-client-csharp/\d+\.\d+\.\d+");
        }

        [Fact]
        public void BuildUserAgentHeader_ShouldContainLanguageIdentifier()
        {
            var userAgent = ClientHeadersBuilder.BuildUserAgentHeader();

            userAgent.Should().StartWith("qase-api-client-csharp/");
        }

        [Theory]
        [InlineData("1.0.0.0", "1.0.0")]
        [InlineData("1.1.11.0", "1.1.11")]
        [InlineData("v2.0.0", "2.0.0")]
        [InlineData("1.1.11", "1.1.11")]
        [InlineData("V1.0.0", "1.0.0")]
        public void FormatVersion_ShouldFormatCorrectly(string input, string expected)
        {
            var result = ClientHeadersBuilder.FormatVersion(input);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void FormatVersion_ShouldReturnEmptyForInvalidInput(string? input)
        {
            var result = ClientHeadersBuilder.FormatVersion(input!);

            result.Should().BeEmpty();
        }
    }
}
