using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    /// <summary>
    /// Tests for StatusMappingUtils
    /// </summary>
    public class StatusMappingUtilsTests
    {
        private readonly ILogger _logger = NullLogger.Instance;

        [Fact]
        public void ParseStatusMappingFromEnv_ValidMapping_ReturnsCorrectMapping()
        {
            // Arrange
            var statusMappingString = "invalid=failed,skipped=passed";

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("failed", result["invalid"]);
            Assert.Equal("passed", result["skipped"]);
        }

        [Fact]
        public void ParseStatusMappingFromEnv_EmptyString_ReturnsEmptyMapping()
        {
            // Arrange
            var statusMappingString = "";

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseStatusMappingFromEnv_NullString_ReturnsEmptyMapping()
        {
            // Arrange
            string? statusMappingString = null;

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseStatusMappingFromEnv_InvalidFormat_IgnoresInvalidPairs()
        {
            // Arrange
            var statusMappingString = "invalid=failed,invalidformat,skipped=passed";

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("failed", result["invalid"]);
            Assert.Equal("passed", result["skipped"]);
        }

        [Fact]
        public void ParseStatusMappingFromEnv_InvalidStatus_IgnoresInvalidStatuses()
        {
            // Arrange
            var statusMappingString = "invalid=failed,unknown=passed,skipped=passed";

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("failed", result["invalid"]);
            Assert.Equal("passed", result["skipped"]);
        }

        [Fact]
        public void ParseStatusMappingFromEnv_RedundantMapping_IgnoresRedundantMapping()
        {
            // Arrange
            var statusMappingString = "invalid=failed,passed=passed,skipped=passed";

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromEnv(statusMappingString, _logger);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("failed", result["invalid"]);
            Assert.Equal("passed", result["skipped"]);
        }

        [Fact]
        public void ParseStatusMappingFromJson_ValidMapping_ReturnsCorrectMapping()
        {
            // Arrange
            var jsonElement = System.Text.Json.JsonDocument.Parse(@"{
                ""invalid"": ""failed"",
                ""skipped"": ""passed""
            }").RootElement;

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromJson(jsonElement, _logger);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("failed", result["invalid"]);
            Assert.Equal("passed", result["skipped"]);
        }

        [Fact]
        public void ParseStatusMappingFromJson_EmptyObject_ReturnsEmptyMapping()
        {
            // Arrange
            var jsonElement = System.Text.Json.JsonDocument.Parse("{}").RootElement;

            // Act
            var result = StatusMappingUtils.ParseStatusMappingFromJson(jsonElement, _logger);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ValidateStatusMapping_ValidMapping_ReturnsTrue()
        {
            // Act
            var result = StatusMappingUtils.ValidateStatusMapping("invalid", "failed", _logger);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateStatusMapping_InvalidSourceStatus_ReturnsFalse()
        {
            // Act
            var result = StatusMappingUtils.ValidateStatusMapping("unknown", "failed", _logger);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateStatusMapping_InvalidTargetStatus_ReturnsFalse()
        {
            // Act
            var result = StatusMappingUtils.ValidateStatusMapping("invalid", "unknown", _logger);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateStatusMapping_RedundantMapping_ReturnsFalse()
        {
            // Act
            var result = StatusMappingUtils.ValidateStatusMapping("passed", "passed", _logger);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ApplyStatusMapping_ValidMapping_ChangesStatus()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };
            var statusMapping = new Dictionary<string, string> { ["invalid"] = "failed" };

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.True(result);
            Assert.Equal(TestResultStatus.Failed, testResult.Execution.Status);
        }

        [Fact]
        public void ApplyStatusMapping_NoMapping_ReturnsFalse()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Passed
                }
            };
            var statusMapping = new Dictionary<string, string> { ["invalid"] = "failed" };

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.False(result);
            Assert.Equal(TestResultStatus.Passed, testResult.Execution.Status);
        }

        [Fact]
        public void ApplyStatusMapping_NullTestResult_ReturnsFalse()
        {
            // Arrange
            TestResult? testResult = null;
            var statusMapping = new Dictionary<string, string> { ["invalid"] = "failed" };

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ApplyStatusMapping_NullExecution_ReturnsFalse()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = null
            };
            var statusMapping = new Dictionary<string, string> { ["invalid"] = "failed" };

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ApplyStatusMapping_EmptyMapping_ReturnsFalse()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };
            var statusMapping = new Dictionary<string, string>();

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.False(result);
            Assert.Equal(TestResultStatus.Invalid, testResult.Execution.Status);
        }

        [Fact]
        public void ApplyStatusMapping_NullMapping_ReturnsFalse()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = "test-1",
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Invalid
                }
            };
            Dictionary<string, string>? statusMapping = null;

            // Act
            var result = StatusMappingUtils.ApplyStatusMapping(testResult, statusMapping, _logger);

            // Assert
            Assert.False(result);
            Assert.Equal(TestResultStatus.Invalid, testResult.Execution.Status);
        }

        [Fact]
        public void ParseStatus_ValidStatus_ReturnsCorrectEnum()
        {
            // Act
            var result = StatusMappingUtils.ParseStatus("passed");

            // Assert
            Assert.Equal(TestResultStatus.Passed, result);
        }

        [Fact]
        public void ParseStatus_InvalidStatus_ReturnsNull()
        {
            // Act
            var result = StatusMappingUtils.ParseStatus("unknown");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ParseStatus_NullString_ReturnsNull()
        {
            // Act
            var result = StatusMappingUtils.ParseStatus(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ParseStatus_EmptyString_ReturnsNull()
        {
            // Act
            var result = StatusMappingUtils.ParseStatus("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValidStatusNames_ReturnsAllValidStatuses()
        {
            // Act
            var result = StatusMappingUtils.GetValidStatusNames();

            // Assert
            Assert.Contains("passed", result);
            Assert.Contains("failed", result);
            Assert.Contains("skipped", result);
            Assert.Contains("blocked", result);
            Assert.Contains("invalid", result);
            Assert.Equal(5, result.Length);
        }

        [Fact]
        public void ValidStatuses_ContainsAllExpectedStatuses()
        {
            // Assert
            Assert.Contains("passed", StatusMappingUtils.ValidStatuses);
            Assert.Contains("failed", StatusMappingUtils.ValidStatuses);
            Assert.Contains("skipped", StatusMappingUtils.ValidStatuses);
            Assert.Contains("blocked", StatusMappingUtils.ValidStatuses);
            Assert.Contains("invalid", StatusMappingUtils.ValidStatuses);
            Assert.Equal(5, StatusMappingUtils.ValidStatuses.Count);
        }

        [Fact]
        public void ValidStatuses_IsCaseInsensitive()
        {
            // Assert
            Assert.Contains("PASSED", StatusMappingUtils.ValidStatuses);
            Assert.Contains("Failed", StatusMappingUtils.ValidStatuses);
            Assert.Contains("SKIPPED", StatusMappingUtils.ValidStatuses);
            Assert.Contains("Blocked", StatusMappingUtils.ValidStatuses);
            Assert.Contains("INVALID", StatusMappingUtils.ValidStatuses);
        }
    }
}
