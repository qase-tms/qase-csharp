using Qase.Csharp.Commons.Attributes;

namespace Qase.XUnit.Reporter.Tests
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

        [QaseIds(500)]
        public void TestMethodWithSingleQaseId()
        {
        }
    }

    public class TestClassWithoutAttributes
    {
        public void TestMethod()
        {
        }
    }

    public class ParameterizedTestClass
    {
        [QaseIds(600)]
        public void ParameterizedTestMethod(string param1, int param2)
        {
        }
    }
}
