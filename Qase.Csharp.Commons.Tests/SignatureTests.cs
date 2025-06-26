using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class SignatureTests
    {
        [Fact]
        public void Generate_WithIdsOnly_ShouldReturnIdsSignature()
        {
            // Arrange
            var ids = new List<long> { 1, 2, 3 };
            IEnumerable<string?>? suites = null;
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("1-2-3");
        }

        [Fact]
        public void Generate_WithSuitesOnly_ShouldReturnSuitesSignature()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            var suites = new List<string?> { "Test Suite", "Another Suite" };
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("test-suite::another-suite");
        }

        [Fact]
        public void Generate_WithParametersOnly_ShouldReturnParametersSignature()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            IEnumerable<string?>? suites = null;
            var parameters = new Dictionary<string, string>
            {
                { "Browser", "Chrome" },
                { "Environment", "Staging" }
            };

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("\"browser\":\"chrome\"::\"environment\":\"staging\"");
        }

        [Fact]
        public void Generate_WithAllParameters_ShouldReturnCombinedSignature()
        {
            // Arrange
            var ids = new List<long> { 1, 2 };
            var suites = new List<string?> { "Test Suite" };
            var parameters = new Dictionary<string, string>
            {
                { "Browser", "Chrome" }
            };

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("1-2::test-suite::\"browser\":\"chrome\"");
        }

        [Fact]
        public void Generate_WithNullValues_ShouldReturnEmptyString()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            IEnumerable<string?>? suites = null;
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Generate_WithEmptyCollections_ShouldReturnEmptyString()
        {
            // Arrange
            var ids = new List<long>();
            var suites = new List<string?>();
            var parameters = new Dictionary<string, string>();

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Generate_WithNullSuiteValues_ShouldSkipNullSuites()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            var suites = new List<string?> { "Test Suite", null, "Another Suite" };
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("test-suite::another-suite");
        }

        [Fact]
        public void Generate_WithSuiteSpacesAndCasing_ShouldNormalizeSuiteNames()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            var suites = new List<string?> { "  Test Suite  ", "ANOTHER SUITE" };
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("test-suite::another-suite");
        }

        [Fact]
        public void Generate_WithParameterSpacesAndCasing_ShouldNormalizeParameters()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            IEnumerable<string?>? suites = null;
            var parameters = new Dictionary<string, string>
            {
                { "  Browser  ", "  Chrome  " },
                { "ENVIRONMENT", "STAGING" }
            };

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("\"browser\":\"chrome\"::\"environment\":\"staging\"");
        }

        [Fact]
        public void Generate_WithSingleId_ShouldReturnSingleId()
        {
            // Arrange
            var ids = new List<long> { 42 };
            IEnumerable<string?>? suites = null;
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public void Generate_WithSingleSuite_ShouldReturnSingleSuite()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            var suites = new List<string?> { "Single Suite" };
            IDictionary<string, string>? parameters = null;

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("single-suite");
        }

        [Fact]
        public void Generate_WithSingleParameter_ShouldReturnSingleParameter()
        {
            // Arrange
            IEnumerable<long>? ids = null;
            IEnumerable<string?>? suites = null;
            var parameters = new Dictionary<string, string>
            {
                { "Key", "Value" }
            };

            // Act
            var result = Signature.Generate(ids, suites, parameters);

            // Assert
            result.Should().Be("\"key\":\"value\"");
        }
    }
} 
