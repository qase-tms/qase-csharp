using Qase.Csharp.Commons.Attributes;

namespace xUnitExamples;

[Fields("preconditions", "Some data")]
[Suites("Main suite", "Sub suite")]
public class SimpleTests
{
    [Fact]
    [Title("Test with title")]
    public void TestWithTitle()
    {
        Assert.True(true);
    }

    [Fact]
    [QaseIds(1)]
    public void TestWithSingleId()
    {
    }

    [Fact]
    [QaseIds(2, 3)]
    public void TestWithMultipleIds()
    {
        Assert.Equal(4, 2 + 2);
    }

    [Fact]
    [Fields("description", "Test description")]
    [Fields("severity", "critical")]
    public void TestWithFields()
    {
        Assert.NotEqual(5, 2 + 2);
    }

    [Fact]
    [Ignore]
    public void TestWithIgnore()
    {
        Assert.True(true);
    }


    [Fact]
    [Suites("Unique suite")]
    public void TestWithUniqueSuite()
    {
    }
}