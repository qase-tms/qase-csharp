using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class TestResultBuilderTests
    {
        [Fact]
        public void Build_MinimalInput_SetsBasicFields()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                Status = TestResultStatus.Passed,
                StartTime = 1000,
                EndTime = 2000,
                Duration = 1000,
                Thread = "1"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Title.Should().Be("MyTest");
            result.Execution.Should().NotBeNull();
            result.Execution!.Status.Should().Be(TestResultStatus.Passed);
            result.Execution.StartTime.Should().Be(1000);
            result.Execution.EndTime.Should().Be(2000);
            result.Execution.Duration.Should().Be(1000);
            result.Execution.Thread.Should().Be("1");
        }

        [Fact]
        public void Build_WithErrorMessage_SetsMessageAndStacktrace()
        {
            var raw = new RawTestData
            {
                DisplayName = "FailingTest",
                Status = TestResultStatus.Failed,
                ErrorMessage = "Expected 1 but got 2",
                StackTrace = "at MyTest.cs:line 10"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Message.Should().Be("Expected 1 but got 2");
            result.Execution!.Stacktrace.Should().Be("at MyTest.cs:line 10");
        }

        [Fact]
        public void Build_WithFullTypeName_SetsSuiteFromTypeName()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                FullTypeName = "MyNamespace.MyClass",
                MethodName = "MyTest",
                Status = TestResultStatus.Passed
            };

            var result = new TestResultBuilder().Build(raw);

            result.Relations.Should().NotBeNull();
            result.Relations!.Suite.Data.Should().HaveCount(2);
            result.Relations.Suite.Data[0].Title.Should().Be("MyNamespace");
            result.Relations.Suite.Data[1].Title.Should().Be("MyClass");
        }

        [Fact]
        public void Build_WithFullTestName_SetsSuiteFromFullTestName()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                FullTestName = "MyNamespace.MyClass.MyTest",
                Status = TestResultStatus.Passed
            };

            var result = new TestResultBuilder().Build(raw);

            result.Relations.Should().NotBeNull();
            result.Relations!.Suite.Data.Should().HaveCount(2);
            result.Relations.Suite.Data[0].Title.Should().Be("MyNamespace");
            result.Relations.Suite.Data[1].Title.Should().Be("MyClass");
        }

        [Fact]
        public void Build_WithPreParsedParams_UsesThemDirectly()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                Status = TestResultStatus.Passed,
                PreParsedParams = new Dictionary<string, string>
                {
                    { "name", "Alice" },
                    { "age", "30" }
                }
            };

            var result = new TestResultBuilder().Build(raw);

            result.Params.Should().HaveCount(2);
            result.Params["name"].Should().Be("Alice");
            result.Params["age"].Should().Be("30");
        }

        [Fact]
        public void Build_WithEmptyPreParsedParam_ReplacesWithEmpty()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                Status = TestResultStatus.Passed,
                PreParsedParams = new Dictionary<string, string>
                {
                    { "email", "" },
                    { "reason", "empty string" }
                }
            };

            var result = new TestResultBuilder().Build(raw);

            result.Params["email"].Should().Be("empty");
            result.Params["reason"].Should().Be("empty string");
        }

        [Fact]
        public void Build_GeneratesSignature()
        {
            var raw = new RawTestData
            {
                DisplayName = "MyTest",
                FullTypeName = "MyNamespace.MyClass",
                MethodName = "MyTest",
                Status = TestResultStatus.Passed
            };

            var result = new TestResultBuilder().Build(raw);

            result.Signature.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Build_WithEmptySignature_FallsBackToTitle()
        {
            var raw = new RawTestData
            {
                DisplayName = "My Test Name",
                Status = TestResultStatus.Passed
            };

            var result = new TestResultBuilder().Build(raw);

            result.Signature.Should().Be("my-test-name");
        }

        [Fact]
        public void Build_FailedWithErrorLabel_ClassifiesAsInvalid()
        {
            var raw = new RawTestData
            {
                DisplayName = "ErrorTest",
                Status = TestResultStatus.Failed,
                FailureLabel = "Error",
                ErrorMessage = "NullReferenceException"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Execution!.Status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void Build_FailedWithAssertionStackTrace_StaysFailed()
        {
            var raw = new RawTestData
            {
                DisplayName = "AssertTest",
                Status = TestResultStatus.Failed,
                ErrorMessage = "Assert.Equal failed",
                StackTrace = "   at Xunit.Assert.Equal(String expected, String actual)"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Execution!.Status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Build_WithPreCollectedSteps_MergesThemIntoResult()
        {
            var bddStep = new StepResult
            {
                Data = new Data { Action = "Given a user exists" },
                Execution = new StepExecution { Status = StepResultStatus.Passed }
            };

            var raw = new RawTestData
            {
                DisplayName = "BddTest",
                Status = TestResultStatus.Passed,
                PreCollectedSteps = new List<StepResult> { bddStep }
            };

            var result = new TestResultBuilder().Build(raw);

            result.Steps.Should().Contain(s => s.Data!.Action == "Given a user exists");
        }

        [Fact]
        public void Build_PassedStatus_DoesNotReclassify()
        {
            var raw = new RawTestData
            {
                DisplayName = "PassedTest",
                Status = TestResultStatus.Passed,
                FailureLabel = "Error"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Execution!.Status.Should().Be(TestResultStatus.Passed);
        }

        [Fact]
        public void Build_SkippedStatus_DoesNotReclassify()
        {
            var raw = new RawTestData
            {
                DisplayName = "SkippedTest",
                Status = TestResultStatus.Skipped,
                ErrorMessage = "Test skipped because..."
            };

            var result = new TestResultBuilder().Build(raw);

            result.Execution!.Status.Should().Be(TestResultStatus.Skipped);
        }

        [Fact]
        public void Build_WithContextDisplayNameBase_UsesItForContextManager()
        {
            var raw = new RawTestData
            {
                DisplayName = "Scenario: User logs in",
                Status = TestResultStatus.Passed,
                ContextDisplayNameBase = "User logs in"
            };

            var result = new TestResultBuilder().Build(raw);

            result.Should().NotBeNull();
            result.Title.Should().Be("Scenario: User logs in");
        }
    }
}
