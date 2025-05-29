using Qase.Csharp.Commons.Attributes;

namespace xUnitExamples;

public class ParametrizedTests
{
    [Theory]
    [Title("Test with parameters")]
    [InlineData(1, 2, 3)]
    [InlineData(4, 5, 9)]
    public void TestWithParameters(int a, int b, int expected)
    {
        Assert.Equal(expected, a + b);
    }
}