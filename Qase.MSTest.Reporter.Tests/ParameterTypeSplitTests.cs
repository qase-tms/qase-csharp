using FluentAssertions;
using Qase.MSTest.Reporter;
using Xunit;

namespace Qase.MSTest.Reporter.Tests
{
    public class ParameterTypeSplitTests
    {
        [Fact]
        public void SplitParameterTypes_ShouldKeepGenericArgumentsTogether()
        {
            var result = QaseMSTestExtension.SplitParameterTypes(
                "System.Int32,System.Collections.Generic.Dictionary`2[System.String,System.Int32], System.String");

            result.Should().Equal(
                "System.Int32",
                "System.Collections.Generic.Dictionary`2[System.String,System.Int32]",
                "System.String");
        }
    }
}
