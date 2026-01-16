using System;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons;

namespace NUnitExamples;

[TestFixture]
[Fields("preconditions", "Some data")]
[Suites("Main suite", "Sub suite")]
public class SimpleTests
{
    [Test]
    [Title("Test with title")]
    [Qase]
    public void TestWithTitle()
    {
        Step1();
        Step2();
        Assert.That(true, Is.True);
    }

    [Test]
    [QaseIds(1)]
    public void TestWithSingleId()
    {
        Assert.That(true, Is.True);
    }

    [Test]
    [QaseIds(2, 3)]
    public void TestWithMultipleIds()
    {
        Assert.That(2 + 2, Is.EqualTo(4));
    }

    [Test]
    [Fields("description", "Test description")]
    [Fields("severity", "critical")]
    public void TestWithFields()
    {
        Assert.That(2 + 2, Is.Not.EqualTo(5));
    }

    [Test]
    [Qase.Csharp.Commons.Attributes.Ignore]
    public void TestWithIgnore()
    {
        Assert.That(true, Is.True);
    }

    [Test]
    [Suites("Unique suite")]
    public void TestWithUniqueSuite()
    {
        Assert.That(true, Is.True);
    }

    [Test]
    [Qase]
    public void TestWithQaseFeature()
    {
        // This test should have its name set in ContextManager
        Assert.That(true, Is.True);
    }

    [Step]
    private void Step1()
    {
        Assert.That(true, Is.True);
    }

    [Step]
    private void Step2()
    {
        Assert.That(true, Is.True);
    }
}
