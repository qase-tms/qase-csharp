using Qase.Csharp.Commons.Attributes;

namespace Qase.NUnit.Reporter.Tests
{
    [Fields("classField", "classValue")]
    [Suites("ClassSuite1", "ClassSuite2")]
    public class TestClassWithAttributes
    {
        [QaseIds(300, 400)]
        [Title("Test Method Title")]
        [Fields("methodField", "methodValue")]
        [Suites("MethodSuite1", "MethodSuite2")]
        public void TestMethodWithAttributes()
        {
        }

        [QaseIds(100, 200)]
        [Title("Test Class Title")]
        [Qase.Csharp.Commons.Attributes.Ignore]
        public void IgnoredTestMethod()
        {
        }

        public void TestMethodWithoutAttributes()
        {
        }
    }

    public class TestClassWithoutAttributes
    {
        public void TestMethod()
        {
        }
    }
}
