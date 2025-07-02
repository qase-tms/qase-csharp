using Qase.Csharp.Commons.Attributes;

namespace xUnitExamples;

public class ParametrizedTests
{
    [Theory]
    [Title("Test with parameters")]
    [InlineData(1, 2, 3)]
    [InlineData(4, 5, 9)]
    [Qase]
    public void TestWithParameters(int a, int b, int expected)
    {
        Step1();
        Assert.Equal(expected, a + b);
    }

    [Step]
    private void Step1()
    {
        Assert.True(true);
    }
}
