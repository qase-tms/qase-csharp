using Qase.Csharp.Commons.Attributes;

namespace Qase.Csharp.Commons.Tests.Fixtures
{
    /// <summary>
    /// Sample test class used as a resolution target for TypeMethodResolver tests.
    /// Must be in the test assembly so AppDomain.GetAssemblies() can find it at runtime.
    /// </summary>
    public class SampleTestClass
    {
        public void SimpleMethod() { }

        public void OverloadedMethod(string value) { }

        public void OverloadedMethod(string value, int count) { }

        public void OverloadedMethod(int count) { }

        [QaseIds(42)]
        [Title("Attributed Method")]
        public void AttributedMethod() { }
    }

    /// <summary>
    /// Another test class for verifying resolution of different types.
    /// </summary>
    public class AnotherTestClass
    {
        public void TestMethod() { }
    }
}
