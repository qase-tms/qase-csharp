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

    /// <summary>
    /// Fixture with all five Qase attribute types on a single method.
    /// Used for ATTR-01 tests (single-attribute and combined extraction).
    /// </summary>
    public class FullyAnnotatedFixture
    {
        [QaseIds(1, 2)]
        [Title("Custom Title")]
        [Fields("env", "staging")]
        [Suites("Login", "Auth")]
        public void AnnotatedMethod() { }
    }

    /// <summary>
    /// Fixture with class-level and method-level attributes for override testing.
    /// Used for ATTR-02 (duplicate field keys) and ATTR-03 (class-first ordering).
    /// </summary>
    [Fields("env", "staging")]
    [Fields("priority", "high")]
    [Suites("ClassSuite")]
    [Ignore]
    public class ClassAndMethodAttributeFixture
    {
        [Fields("env", "production")]
        [Fields("browser", "chrome")]
        [Suites("MethodSuite")]
        public void MethodWithOverrides() { }

        public void PlainMethod() { }
    }

    /// <summary>
    /// Fixture with no Qase attributes (edge case for empty extraction).
    /// </summary>
    public class NoAttributeFixture
    {
        public void PlainMethod() { }
    }

    /// <summary>
    /// Fixture with Ignore attribute on class level only.
    /// Used for ATTR-03 (class-level Ignore preserved when no method-level override).
    /// </summary>
    [Ignore]
    public class IgnoredClassFixture
    {
        public void TestMethod() { }
    }

    /// <summary>
    /// Fixture with parameterized methods for ParameterParser.ParseAndMap tests.
    /// Provides typed method signatures so MethodInfo.GetParameters() returns named parameters.
    /// </summary>
    public class ParameterizedMethodFixture
    {
        public void TwoStringParams(string user, string value) { }
        public void ThreeParams(int quantity, double price, int discount) { }
        public void SingleParam(string email) { }
        public void NoParams() { }
    }
}
