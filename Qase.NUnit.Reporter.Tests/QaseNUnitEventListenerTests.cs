using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Qase.NUnit.Reporter;

namespace Qase.NUnit.Reporter.Tests
{
    public class QaseNUnitEventListenerTests
    {
        private QaseNUnitEventListener _listener;
        private Type _listenerType;

        public QaseNUnitEventListenerTests()
        {
            _listener = new QaseNUnitEventListener();
            _listenerType = typeof(QaseNUnitEventListener);
        }

        #region ExtractMethodNameWithoutParameters Tests

        [Theory]
        [InlineData("Test2(\"user1\",\"value2\")", "Test2")]
        [InlineData("Test1", "Test1")]
        [InlineData("TestMethod()", "TestMethod")]
        [InlineData("Test(\"param\")", "Test")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ExtractMethodNameWithoutParameters_ShouldExtractMethodName(string testName, string expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("ExtractMethodNameWithoutParameters", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = method?.Invoke(_listener, new object[] { testName }) as string;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region ExtractParameterValuesFromName Tests

        [Theory]
        [InlineData("Test2(\"user1\",\"value2\")", new[] { "user1", "value2" })]
        [InlineData("Test(\"param1\", \"param2\")", new[] { "param1", "param2" })]
        [InlineData("Test(\"single\")", new[] { "single" })]
        [InlineData("Test()", new string[0])]
        [InlineData("Test", new string[0])]
        [InlineData("", new string[0])]
        [InlineData(null, new string[0])]
        public void ExtractParameterValuesFromName_ShouldExtractParameters(string testName, string[] expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("ExtractParameterValuesFromName", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = method?.Invoke(_listener, new object[] { testName }) as List<string>;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test(\"value with spaces\")", new[] { "value with spaces" })]
        [InlineData("Test(\"value,with,commas\")", new[] { "value,with,commas" })]
        [InlineData("Test(\"value\\\"with\\\"quotes\")", new[] { "value\"with\"quotes" })]
        public void ExtractParameterValuesFromName_ShouldHandleSpecialCharacters(string testName, string[] expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("ExtractParameterValuesFromName", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = method?.Invoke(_listener, new object[] { testName }) as List<string>;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region GenerateDisplayName Tests

        [Theory]
        [InlineData("NunitExamples.Tests.Test1", null, "NunitExamples.Tests.Test1")]
        [InlineData("NunitExamples.Tests.Test2", 
            new[] { "user", "user1" }, 
            "NunitExamples.Tests.Test2(user: user1)")]
        [InlineData("Tests.Test1", null, "Tests.Test1")]
        [InlineData("Test1", null, "Test1")]
        public void GenerateDisplayName_ShouldGenerateCorrectDisplayName(string fullName, string[] paramPairs, string expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("GenerateDisplayName", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            Dictionary<string, string> parameters = null;
            if (paramPairs != null && paramPairs.Length > 0)
            {
                parameters = new Dictionary<string, string>();
                for (int i = 0; i < paramPairs.Length; i += 2)
                {
                    parameters[paramPairs[i]] = paramPairs[i + 1];
                }
            }

            // Act
            var result = method?.Invoke(_listener, new object[] { fullName, parameters }) as string;

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GenerateDisplayName_WithMultipleParameters_ShouldFormatCorrectly()
        {
            // Arrange
            var method = _listenerType.GetMethod("GenerateDisplayName", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var fullName = "NunitExamples.Tests.Test2";
            var parameters = new Dictionary<string, string>
            {
                { "user", "user1" },
                { "value", "value2" }
            };

            // Act
            var result = method?.Invoke(_listener, new object[] { fullName, parameters }) as string;

            // Assert
            result.Should().Be("NunitExamples.Tests.Test2(user: user1, value: value2)");
        }

        [Theory]
        [InlineData("", null, "")]
        [InlineData(null, null, null)]
        public void GenerateDisplayName_WithEmptyOrNullInput_ShouldReturnInput(string fullName, Dictionary<string, string> parameters, string expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("GenerateDisplayName", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = method?.Invoke(_listener, new object[] { fullName, parameters }) as string;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region ParseSuiteFromFullName Tests

        [Theory]
        [InlineData("NunitExamples.Tests.Test1", new[] { "NunitExamples", "Tests" })]
        [InlineData("Tests.Test1", new[] { "Tests" })]
        [InlineData("Test1", new string[0])]
        [InlineData("", new string[0])]
        public void ParseSuiteFromFullName_ShouldParseSuiteHierarchy(string fullName, string[] expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("ParseSuiteFromFullName", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = method?.Invoke(_listener, new object[] { fullName }) as List<Qase.Csharp.Commons.Models.Domain.SuiteData>;

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(expected.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                result[i].Title.Should().Be(expected[i]);
            }
        }

        [Fact]
        public void ParseSuiteFromFullName_WithParameterizedTest_ShouldParseCorrectly()
        {
            // Arrange
            var method = _listenerType.GetMethod("ParseSuiteFromFullName", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var fullName = "NunitExamples.Tests.Test2(\"user1\",\"value2\")";

            // Act
            var result = method?.Invoke(_listener, new object[] { fullName }) as List<Qase.Csharp.Commons.Models.Domain.SuiteData>;

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("NunitExamples");
            result[1].Title.Should().Be("Tests");
        }

        #endregion

        #region MapResultStatus Tests

        [Theory]
        [InlineData("Passed", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Passed)]
        [InlineData("Failed", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Failed)]
        [InlineData("Skipped", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("Inconclusive", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("Unknown", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        public void MapResultStatus_ShouldMapCorrectly(string result, Qase.Csharp.Commons.Models.Domain.TestResultStatus expected)
        {
            // Arrange
            var method = _listenerType.GetMethod("MapResultStatus", 
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var status = method?.Invoke(_listener, new object[] { result });

            // Assert
            status.Should().Be(expected);
        }

        #endregion

        #region DetermineFailureType Tests

        [Fact]
        public void DetermineFailureType_WithErrorLabel_ShouldReturnInvalid()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "Error";
            int? assertsCount = null;
            var stackTrace = "";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeFalse(); // False means Invalid
        }

        [Fact]
        public void DetermineFailureType_WithZeroAsserts_ShouldReturnInvalid()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "";
            int? assertsCount = 0;
            var stackTrace = "";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeFalse(); // False means Invalid
        }

        [Fact]
        public void DetermineFailureType_WithPositiveAsserts_ShouldReturnFailed()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "";
            int? assertsCount = 1;
            var stackTrace = "";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeTrue(); // True means Failed
        }

        [Fact]
        public void DetermineFailureType_WithAssertionInStackTrace_ShouldReturnFailed()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "";
            int? assertsCount = null;
            var stackTrace = "at NUnit.Framework.Assert.AreEqual";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeTrue(); // True means Failed
        }

        [Fact]
        public void DetermineFailureType_WithNoIndicators_ShouldDefaultToFailed()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "";
            int? assertsCount = null;
            var stackTrace = "";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeTrue(); // Defaults to Failed
        }

        [Fact]
        public void DetermineFailureType_WithErrorLabelAndPositiveAsserts_ShouldReturnInvalid()
        {
            // Arrange
            var method = _listenerType.GetMethod("DetermineFailureType", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var label = "Error";
            int? assertsCount = 5;
            var stackTrace = "at NUnit.Framework.Assert.AreEqual";

            // Act
            var result = (bool)method.Invoke(_listener, new object[] { label, assertsCount, stackTrace });

            // Assert
            result.Should().BeFalse(); // Error label takes priority
        }

        #endregion
    }
}
