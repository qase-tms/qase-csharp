using System.Reflection;
using Qase.Csharp.Commons.Utils;
using Qase.Csharp.Commons.Tests.Fixtures;

namespace Qase.Csharp.Commons.Tests
{
    public class ParameterParserTests
    {
        /// <summary>
        /// Helper to get MethodInfo from ParameterizedMethodFixture by method name.
        /// </summary>
        private static MethodInfo GetMethod(string name) =>
            typeof(ParameterizedMethodFixture).GetMethod(name)!;

        // === PARAM-01: ParseValues extracts parameter values from display names ===

        [Fact]
        public void ParseValues_WithNull_ReturnsEmptyList()
        {
            var result = ParameterParser.ParseValues(null);
            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseValues_WithEmptyString_ReturnsEmptyList()
        {
            var result = ParameterParser.ParseValues("");
            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseValues_WithNoParens_ReturnsEmptyList()
        {
            var result = ParameterParser.ParseValues("Method");
            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseValues_WithEmptyParens_ReturnsEmptyList()
        {
            var result = ParameterParser.ParseValues("Method()");
            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseValues_MSTestFormat_ExtractsValues()
        {
            // MSTest format: space before paren, values may be unquoted
            var result = ParameterParser.ParseValues(
                "Method (admin@example.com,Admin123!,True)");

            result.Should().BeEquivalentTo(
                new[] { "admin@example.com", "Admin123!", "True" });
        }

        [Fact]
        public void ParseValues_NUnitFormat_ExtractsValues()
        {
            // NUnit format: fully qualified, escaped quotes around values
            var result = ParameterParser.ParseValues(
                "Ns.Class.Method(\"admin@example.com\",\"Admin123!\",true)");

            result.Should().BeEquivalentTo(
                new[] { "admin@example.com", "Admin123!", "true" });
        }

        [Fact]
        public void ParseValues_UnquotedNumerics_ExtractsAllValues()
        {
            var result = ParameterParser.ParseValues("Method(1,29.99,0)");

            result.Should().BeEquivalentTo(new[] { "1", "29.99", "0" });
        }

        [Fact]
        public void ParseValues_CommasInsideQuotes_TreatedAsPartOfValue()
        {
            var result = ParameterParser.ParseValues("Method(\"value,with,commas\")");

            result.Should().BeEquivalentTo(new[] { "value,with,commas" });
        }

        [Fact]
        public void ParseValues_EscapedQuotesInsideQuotedValue_HandledCorrectly()
        {
            var result = ParameterParser.ParseValues(
                "Method(\"value\\\"with\\\"quotes\")");

            result.Should().BeEquivalentTo(new[] { "value\"with\"quotes" });
        }

        [Fact]
        public void ParseValues_EmptyQuotedStringAndUnquoted_PreservesBoth()
        {
            var result = ParameterParser.ParseValues("Method(\"\",nouser)");

            result.Should().BeEquivalentTo(new[] { "", "nouser" });
        }

        // === PARAM-02: ParseAndMap maps parsed values to MethodInfo parameter names ===

        [Fact]
        public void ParseAndMap_TwoStringParams_ReturnsMappedDictionary()
        {
            var method = GetMethod("TwoStringParams");

            var result = ParameterParser.ParseAndMap(
                "Method (admin@example.com,pass)", method);

            result.Should().ContainKey("user").WhoseValue.Should().Be("admin@example.com");
            result.Should().ContainKey("value").WhoseValue.Should().Be("pass");
        }

        [Fact]
        public void ParseAndMap_ThreeParams_ReturnsMappedDictionary()
        {
            var method = GetMethod("ThreeParams");

            var result = ParameterParser.ParseAndMap(
                "Method (1,29.99,0)", method);

            result.Should().ContainKey("quantity").WhoseValue.Should().Be("1");
            result.Should().ContainKey("price").WhoseValue.Should().Be("29.99");
            result.Should().ContainKey("discount").WhoseValue.Should().Be("0");
        }

        [Fact]
        public void ParseAndMap_SingleParam_ReturnsMappedDictionary()
        {
            var method = GetMethod("SingleParam");

            var result = ParameterParser.ParseAndMap("Method (value)", method);

            result.Should().ContainKey("email").WhoseValue.Should().Be("value");
        }

        [Fact]
        public void ParseAndMap_WithNullDisplayName_ReturnsEmptyDictionary()
        {
            var method = GetMethod("TwoStringParams");

            var result = ParameterParser.ParseAndMap(null, method);

            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseAndMap_WithNullMethodInfo_ReturnsEmptyDictionary()
        {
            var result = ParameterParser.ParseAndMap("Method (a,b)", null);

            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseAndMap_NoParamsInDisplayName_ReturnsEmptyDictionary()
        {
            var method = GetMethod("TwoStringParams");

            var result = ParameterParser.ParseAndMap("Method", method);

            result.Should().BeEmpty();
        }

        [Fact]
        public void ParseAndMap_MoreValuesThanParams_UsesMinGuard()
        {
            // 3 values but method only has 1 parameter -- Math.Min guard
            var method = GetMethod("SingleParam");

            var result = ParameterParser.ParseAndMap("Method (a,b,c)", method);

            result.Should().HaveCount(1);
            result.Should().ContainKey("email").WhoseValue.Should().Be("a");
        }

        [Fact]
        public void ParseAndMap_EmptyStringValue_NormalizedToEmpty()
        {
            // Empty quoted string should be normalized to "empty"
            var method = GetMethod("SingleParam");

            var result = ParameterParser.ParseAndMap("Method (\"\")", method);

            result.Should().ContainKey("email").WhoseValue.Should().Be("empty");
        }

        // === PARAM-03: Both MSTest and NUnit format support ===

        [Fact]
        public void ParseValues_MSTestFormatWithSpaceBeforeParen_Works()
        {
            // MSTest format: "Method (a,b)" -- space before opening paren
            var result = ParameterParser.ParseValues("Method (a,b)");

            result.Should().BeEquivalentTo(new[] { "a", "b" });
        }

        [Fact]
        public void ParseValues_NUnitFullyQualifiedWithEscapedQuotes_Works()
        {
            // NUnit format: "Ns.Class.Method(\"a\",\"b\")" -- no space, escaped quotes
            var result = ParameterParser.ParseValues(
                "NUnitExamples.ParameterizedTests.Method(\"a\",\"b\")");

            result.Should().BeEquivalentTo(new[] { "a", "b" });
        }
    }
}
