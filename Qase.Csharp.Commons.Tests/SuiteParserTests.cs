using Qase.Csharp.Commons.Utils;

namespace Qase.Csharp.Commons.Tests
{
    public class SuiteParserTests
    {
        // === SUITE-01: FromTypeName ===

        [Fact]
        public void FromTypeName_WithNamespaceAndClass_ReturnsSuiteHierarchy()
        {
            // Arrange & Act
            var result = SuiteParser.FromTypeName("Namespace.Sub.ClassName");

            // Assert
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("Namespace");
            result[1].Title.Should().Be("Sub");
            result[2].Title.Should().Be("ClassName");
        }

        [Fact]
        public void FromTypeName_WithClassOnly_ReturnsSingleSuite()
        {
            // Arrange & Act
            var result = SuiteParser.FromTypeName("TestClass");

            // Assert
            result.Should().HaveCount(1);
            result[0].Title.Should().Be("TestClass");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FromTypeName_WithNullOrEmpty_ReturnsEmptyList(string? input)
        {
            // Arrange & Act
            var result = SuiteParser.FromTypeName(input);

            // Assert
            result.Should().BeEmpty();
        }

        // === SUITE-02: FromFullTestName ===

        [Fact]
        public void FromFullTestName_WithNamespaceClassMethod_StripsMethod()
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName("NunitExamples.Tests.Test1");

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("NunitExamples");
            result[1].Title.Should().Be("Tests");
        }

        [Fact]
        public void FromFullTestName_WithParameters_StripsMethodAndParams()
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName("NunitExamples.Tests.Test2(\"user1\",\"value2\")");

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("NunitExamples");
            result[1].Title.Should().Be("Tests");
        }

        [Fact]
        public void FromFullTestName_WithSingleNamespaceAndMethod_ReturnsSingleSuite()
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName("Tests.Test1");

            // Assert
            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Tests");
        }

        [Fact]
        public void FromFullTestName_WithMethodOnly_ReturnsEmptyList()
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName("Test1");

            // Assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FromFullTestName_WithNullOrEmpty_ReturnsEmptyList(string? input)
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName(input);

            // Assert
            result.Should().BeEmpty();
        }

        // === SUITE-03: Dots in parameters ===

        [Theory]
        [InlineData("Ns.Sub.Class.Method(50.0)", new[] { "Ns", "Sub", "Class" })]
        [InlineData("Ns.Class.Method(\"1.2.3\",\"text\")", new[] { "Ns", "Class" })]
        [InlineData("Ns.Class.Method(1.0,2.0,3.0)", new[] { "Ns", "Class" })]
        public void FromFullTestName_WithDotsInParams_DoesNotCreateSpuriousSuites(
            string fullName, string[] expectedTitles)
        {
            // Arrange & Act
            var result = SuiteParser.FromFullTestName(fullName);

            // Assert
            result.Should().HaveCount(expectedTitles.Length);
            for (int i = 0; i < expectedTitles.Length; i++)
            {
                result[i].Title.Should().Be(expectedTitles[i]);
            }
        }
    }
}
