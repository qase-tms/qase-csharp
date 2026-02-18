using System;
using System.Collections.Generic;
using System.Text.Json;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class TestResultTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var testResult = new TestResult();

            // Assert
            testResult.Id.Should().NotBeEmpty();
            testResult.Title.Should().BeNull();
            testResult.Signature.Should().BeNull();
            testResult.RunId.Should().BeNull();
            testResult.TestopsIds.Should().BeNull();
            testResult.Execution.Should().NotBeNull();
            testResult.Fields.Should().NotBeNull();
            testResult.Fields.Should().BeEmpty();
            testResult.Attachments.Should().NotBeNull();
            testResult.Attachments.Should().BeEmpty();
            testResult.Steps.Should().NotBeNull();
            testResult.Steps.Should().BeEmpty();
            testResult.Params.Should().NotBeNull();
            testResult.Params.Should().BeEmpty();
            testResult.ParamGroups.Should().NotBeNull();
            testResult.ParamGroups.Should().BeEmpty();
            testResult.Relations.Should().NotBeNull();
            testResult.Muted.Should().BeFalse();
            testResult.Message.Should().BeNull();
            testResult.Ignore.Should().BeFalse();
        }

        [Fact]
        public void Id_ShouldBeUniqueForEachInstance()
        {
            // Act
            var testResult1 = new TestResult();
            var testResult2 = new TestResult();

            // Assert
            testResult1.Id.Should().NotBe(testResult2.Id);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            // Arrange
            var testResult = new TestResult();
            var title = "Test Title";
            var signature = "test-signature";
            var runId = "run-123";
            var testopsIds = new List<long> { 1, 2, 3 };
            var fields = new Dictionary<string, string> { { "key", "value" } };
            var attachments = new List<Attachment> { new Attachment() };
            var steps = new List<StepResult> { new StepResult() };
            var parameters = new Dictionary<string, string> { { "param", "value" } };
            var paramGroups = new List<List<string>> { new List<string> { "group1" } };
            var message = "Test message";

            // Act
            testResult.Title = title;
            testResult.Signature = signature;
            testResult.RunId = runId;
            testResult.TestopsIds = testopsIds;
            testResult.Fields = fields;
            testResult.Attachments = attachments;
            testResult.Steps = steps;
            testResult.Params = parameters;
            testResult.ParamGroups = paramGroups;
            testResult.Muted = true;
            testResult.Message = message;
            testResult.Ignore = true;

            // Assert
            testResult.Title.Should().Be(title);
            testResult.Signature.Should().Be(signature);
            testResult.RunId.Should().Be(runId);
            testResult.TestopsIds.Should().BeEquivalentTo(testopsIds);
            testResult.Fields.Should().BeEquivalentTo(fields);
            testResult.Attachments.Should().BeEquivalentTo(attachments);
            testResult.Steps.Should().BeEquivalentTo(steps);
            testResult.Params.Should().BeEquivalentTo(parameters);
            testResult.ParamGroups.Should().BeEquivalentTo(paramGroups);
            testResult.Muted.Should().BeTrue();
            testResult.Message.Should().Be(message);
            testResult.Ignore.Should().BeTrue();
        }

        [Fact]
        public void ToString_ShouldReturnValidJson()
        {
            // Arrange
            var testResult = new TestResult
            {
                Title = "Test Title",
                Signature = "test-signature",
                RunId = "run-123",
                TestopsIds = new List<long> { 1, 2, 3 },
                Message = "Test message",
                Muted = true,
                Ignore = false
            };

            // Act
            var jsonString = testResult.ToString();

            // Assert
            jsonString.Should().NotBeEmpty();
            
            // Verify it's valid JSON by deserializing it
            var deserialized = JsonSerializer.Deserialize<TestResult>(jsonString);
            deserialized.Should().NotBeNull();
            deserialized!.Title.Should().Be("Test Title");
            deserialized.Signature.Should().Be("test-signature");
            deserialized.TestopsIds.Should().BeEquivalentTo(new List<long> { 1, 2, 3 });
            deserialized.Message.Should().Be("Test message");
            deserialized.Muted.Should().BeTrue();
            // RunId and Ignore are [JsonIgnore] and excluded from serialization
            deserialized.RunId.Should().BeNull();
            deserialized.Ignore.Should().BeFalse();
        }

        [Fact]
        public void Collections_ShouldBeMutable()
        {
            // Arrange
            var testResult = new TestResult();

            // Act
            testResult.Fields.Add("key1", "value1");
            testResult.Fields.Add("key2", "value2");
            testResult.Attachments.Add(new Attachment { FileName = "test.txt" });
            testResult.Steps.Add(new StepResult());
            testResult.Params.Add("param1", "value1");
            testResult.ParamGroups.Add(new List<string> { "group1", "group2" });

            // Assert
            testResult.Fields.Should().HaveCount(2);
            testResult.Fields["key1"].Should().Be("value1");
            testResult.Fields["key2"].Should().Be("value2");
            testResult.Attachments.Should().HaveCount(1);
            testResult.Attachments[0].FileName.Should().Be("test.txt");
            testResult.Steps.Should().HaveCount(1);
            testResult.Params.Should().HaveCount(1);
            testResult.Params["param1"].Should().Be("value1");
            testResult.ParamGroups.Should().HaveCount(1);
            testResult.ParamGroups[0].Should().BeEquivalentTo(new List<string> { "group1", "group2" });
        }

        [Fact]
        public void Execution_ShouldBeInitialized()
        {
            // Arrange
            var testResult = new TestResult();

            // Act & Assert
            testResult.Execution.Should().NotBeNull();
            testResult.Execution.Should().BeOfType<TestResultExecution>();
        }

        [Fact]
        public void Relations_ShouldBeInitialized()
        {
            // Arrange
            var testResult = new TestResult();

            // Act & Assert
            testResult.Relations.Should().NotBeNull();
            testResult.Relations.Should().BeOfType<Relations>();
        }
    }
} 
