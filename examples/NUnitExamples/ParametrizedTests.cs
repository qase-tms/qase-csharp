using System;
using Qase.Csharp.Commons.Attributes;

namespace NUnitExamples;

[TestFixture]
public class ParametrizedTests
{
    [Test]
    [TestCase(1, 2, 3)]
    [TestCase(4, 5, 9)]
    [TestCase(10, 20, 30)]
    [QaseIds(100)]
    [Qase]
    public void TestWithParameters(int a, int b, int expected)
    {
        var result = a + b;
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("user1", "password1")]
    [TestCase("user2", "password2")]
    [QaseIds(101)]
    [Title("Login Test")]
    [Qase]
    public void TestWithStringParameters(string username, string password)
    {
        Assert.That(username, Is.Not.Empty);
        Assert.That(password, Is.Not.Empty);
    }

    [Test]
    [TestCase(1, 2, ExpectedResult = 3)]
    [TestCase(4, 5, ExpectedResult = 9)]
    [QaseIds(102)]
    [Qase]
    public int TestWithReturnValue(int a, int b)
    {
        return a + b;
    }
}
