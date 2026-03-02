using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestHost;
using Microsoft.Testing.Platform.TestHost;
using Moq;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Methods;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.MSTest.Reporter;
using Xunit;

namespace Qase.MSTest.Reporter.Tests
{
    // Fixture classes for attribute extraction tests (reflection targets only)
    internal class QaseAnnotatedTestFixture
    {
        [QaseIds(42, 100)]
        [Title("Custom Login Test")]
        [Fields("priority", "high")]
        [Fields("severity", "critical")]
        [Suites("Auth", "Login")]
        public void AnnotatedTestMethod() { }

        public void UnannotatedTestMethod() { }

        [Ignore]
        public void IgnoredTestMethod() { }

        [Title("Method Title Override")]
        public void MethodWithTitleOnly() { }
    }

    [Suites("ClassLevel Suite")]
    internal class ClassLevelSuiteFixture
    {
        public void SomeTest() { }
    }

    /// <summary>
    /// Fixture class for parameterized test reflection. Methods simulate DataRow/DynamicData patterns.
    /// </summary>
    internal class ParameterizedTestFixture
    {
        public void AddNumbers(int a, int b) { }

        public void LoginUser(string username, string password) { }

        public void MixedParams(int id, string name, bool active) { }

        public void SingleParam(string value) { }

        [Title("Custom Parameterized Title")]
        public void TitledParameterized(int x) { }
    }

    /// <summary>
    /// Fixture class with overloaded methods to test ParameterTypeFullNames resolution.
    /// </summary>
    internal class OverloadedTestFixture
    {
        public void Calculate(int a, int b) { }
        public void Calculate(string expression) { }
        public void Calculate(double a, double b, double c) { }
    }

    public class ConsumeAsyncTests
    {
        private static readonly FieldInfo ReporterField =
            typeof(QaseMSTestExtension).GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Instance)!;

        private static readonly FieldInfo ConfigField =
            typeof(QaseMSTestExtension).GetField("_config", BindingFlags.NonPublic | BindingFlags.Instance)!;

        /// <summary>
        /// Creates a QaseMSTestExtension and overwrites its private _config field
        /// with a QaseConfig set to the specified Mode.
        /// </summary>
        private static QaseMSTestExtension CreateExtensionWithConfig(Mode mode)
        {
            var extension = new QaseMSTestExtension();
            var config = new QaseConfig { Mode = mode };
            ConfigField.SetValue(extension, config);
            return extension;
        }

        /// <summary>
        /// Injects a mock ICoreReporter into the private _reporter field via reflection.
        /// </summary>
        private static void InjectReporter(QaseMSTestExtension extension, ICoreReporter reporter)
        {
            ReporterField.SetValue(extension, reporter);
        }

        /// <summary>
        /// Constructs a TestNodeUpdateMessage with the given display name and properties.
        /// </summary>
        private static TestNodeUpdateMessage CreateTestMessage(string displayName, params IProperty[] properties)
        {
            var testNode = new TestNode
            {
                Uid = new TestNodeUid(Guid.NewGuid().ToString()),
                DisplayName = displayName,
                Properties = new PropertyBag(properties)
            };
            return new TestNodeUpdateMessage(new SessionUid("test-session"), testNode);
        }

        /// <summary>
        /// Creates a mock reporter that captures the TestResult passed to addResult.
        /// Returns both the mock and a function to retrieve the captured result.
        /// </summary>
        private static (Mock<ICoreReporter> mock, Func<TestResult?> getCaptured) CreateCapturingReporter()
        {
            TestResult? capturedResult = null;
            var mockReporter = new Mock<ICoreReporter>();
            mockReporter
                .Setup(r => r.addResult(It.IsAny<TestResult>()))
                .Callback<TestResult>(r => capturedResult = r)
                .Returns(Task.CompletedTask);
            return (mockReporter, () => capturedResult);
        }

        [Fact]
        public async Task ConsumeAsync_WithPassedState_MapsToPassedStatus()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("PassedTest", new PassedTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Once);
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Passed);
        }

        [Fact]
        public async Task ConsumeAsync_WithFailedState_MapsToFailedStatus()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("FailedTest", new FailedTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Once);
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public async Task ConsumeAsync_WithErrorState_MapsToInvalidStatus()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("ErrorTest", new ErrorTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Once);
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public async Task ConsumeAsync_WithSkippedState_MapsToSkippedStatus()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("SkippedTest", new SkippedTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Once);
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Skipped);
        }

        [Fact]
        public async Task ConsumeAsync_WithTimingProperty_ExtractsStartEndDuration()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var start = new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero);
            var end = start.AddSeconds(1.5);
            var duration = end - start;
            var timing = new TimingProperty(new TimingInfo(start, end, duration));

            var message = CreateTestMessage("TimedTest", new PassedTestNodeStateProperty(), timing);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.StartTime.Should().Be(start.ToUnixTimeMilliseconds());
            result.Execution.EndTime.Should().Be(end.ToUnixTimeMilliseconds());
            result.Execution.Duration.Should().Be(1500);
        }

        [Fact]
        public async Task ConsumeAsync_WithFailedStateAndException_ExtractsMessageAndStackTrace()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            // Create a real exception with a stack trace by throwing and catching
            Exception testException;
            try
            {
                throw new InvalidOperationException("Assertion failed: expected true but was false");
            }
            catch (Exception ex)
            {
                testException = ex;
            }

            var message = CreateTestMessage("FailedWithException",
                new FailedTestNodeStateProperty(testException));

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Failed);
            result.Message.Should().Be(testException.Message);
            result.Execution.Stacktrace.Should().Be(testException.StackTrace);
        }

        [Fact]
        public async Task ConsumeAsync_WithErrorStateAndException_ExtractsMessageAndStackTrace()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            // Create a real exception with a stack trace by throwing and catching
            Exception testException;
            try
            {
                throw new InvalidOperationException("Runtime error occurred");
            }
            catch (Exception ex)
            {
                testException = ex;
            }

            var message = CreateTestMessage("ErrorWithException",
                new ErrorTestNodeStateProperty(testException));

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Invalid);
            result.Message.Should().Be(testException.Message);
            result.Execution.Stacktrace.Should().Be(testException.StackTrace);
        }

        [Fact]
        public async Task ConsumeAsync_WithFailedStateExplanationOnly_UsesExplanationAsMessage()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("FailedWithExplanation",
                new FailedTestNodeStateProperty("test failed due to timeout"));

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Failed);
            result.Message.Should().Be("test failed due to timeout");
            result.Execution.Stacktrace.Should().BeNull();
        }

        [Fact]
        public async Task ConsumeAsync_WithSkippedStateAndExplanation_SetsMessage()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("SkippedWithExplanation",
                new SkippedTestNodeStateProperty("inconclusive test"));

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Execution!.Status.Should().Be(TestResultStatus.Skipped);
            result.Message.Should().Be("inconclusive test");
        }

        [Fact]
        public async Task ConsumeAsync_WithInProgressState_DoesNotCallAddResult()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var mockReporter = new Mock<ICoreReporter>();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("InProgressTest", new InProgressTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Never);
        }

        [Fact]
        public async Task ConsumeAsync_WithDiscoveredState_DoesNotCallAddResult()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var mockReporter = new Mock<ICoreReporter>();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("DiscoveredTest", new DiscoveredTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Never);
        }

        [Fact]
        public async Task ConsumeAsync_SetsDisplayNameAsTitle()
        {
            // Arrange
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("MyTestMethod", new PassedTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Title.Should().Be("MyTestMethod");
        }

        [Fact]
        public async Task ConsumeAsync_WhenReporterNull_DoesNotThrow()
        {
            // Arrange - do NOT inject reporter, _reporter stays null
            var extension = CreateExtensionWithConfig(Mode.TestOps);

            var message = CreateTestMessage("TestWithNullReporter", new PassedTestNodeStateProperty());

            // Act & Assert - should not throw NullReferenceException
            var act = () => extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }

        // =====================================================================
        // Attribute extraction, ContextManager, signature, and Ignore tests
        // =====================================================================

        /// <summary>
        /// Creates a TestMethodIdentifierProperty pointing to a method on a fixture class.
        /// </summary>
        private static TestMethodIdentifierProperty CreateMethodIdentifier(
            string namespaceName, string typeName, string methodName)
        {
            return new TestMethodIdentifierProperty(
                AssemblyFullName: "Qase.MSTest.Reporter.Tests, Version=1.0.0.0",
                Namespace: namespaceName,
                TypeName: typeName,
                MethodName: methodName,
                ParameterTypeFullNames: Array.Empty<string>(),
                ReturnTypeFullName: "System.Void"
            );
        }

        /// <summary>
        /// Creates a TestMethodIdentifierProperty with parameter type full names for parameterized tests.
        /// </summary>
        private static TestMethodIdentifierProperty CreateMethodIdentifier(
            string namespaceName, string typeName, string methodName, string[] parameterTypeFullNames)
        {
            return new TestMethodIdentifierProperty(
                AssemblyFullName: "Qase.MSTest.Reporter.Tests, Version=1.0.0.0",
                Namespace: namespaceName,
                TypeName: typeName,
                MethodName: methodName,
                ParameterTypeFullNames: parameterTypeFullNames,
                ReturnTypeFullName: "System.Void"
            );
        }

        [Fact]
        public async Task ConsumeAsync_WithQaseIdsAttribute_SetsTestopsIds()
        {
            // Arrange (QASE-01)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "AnnotatedTestMethod");
            var message = CreateTestMessage("AnnotatedTestMethod",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.TestopsIds.Should().NotBeNull();
            result.TestopsIds.Should().ContainInOrder(42L, 100L);
        }

        [Fact]
        public async Task ConsumeAsync_WithTitleAttribute_OverridesDisplayName()
        {
            // Arrange (QASE-02)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "AnnotatedTestMethod");
            var message = CreateTestMessage("OriginalDisplayName",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Title.Should().Be("Custom Login Test");
        }

        [Fact]
        public async Task ConsumeAsync_WithFieldsAttributes_AccumulatesKeyValuePairs()
        {
            // Arrange (QASE-03)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "AnnotatedTestMethod");
            var message = CreateTestMessage("FieldsTest",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Fields.Should().ContainKey("priority").WhoseValue.Should().Be("high");
            result.Fields.Should().ContainKey("severity").WhoseValue.Should().Be("critical");
        }

        [Fact]
        public async Task ConsumeAsync_WithSuitesAttribute_OverridesDefaultSuiteHierarchy()
        {
            // Arrange (QASE-04)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "AnnotatedTestMethod");
            var message = CreateTestMessage("SuitesTest",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Relations.Should().NotBeNull();
            result.Relations!.Suite.Data.Should().HaveCount(2);
            result.Relations.Suite.Data.Select(d => d.Title).Should().ContainInOrder("Auth", "Login");
        }

        [Fact]
        public async Task ConsumeAsync_WithoutAttributes_UsesDefaultSuiteFromNamespaceAndClass()
        {
            // Arrange (QASE-10)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "UnannotatedTestMethod");
            var message = CreateTestMessage("UnannotatedTestMethod",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Relations.Should().NotBeNull();
            var suiteTitles = result.Relations!.Suite.Data.Select(d => d.Title).ToList();
            suiteTitles.Should().ContainInOrder(
                "Qase", "MSTest", "Reporter", "Tests", "QaseAnnotatedTestFixture");
        }

        [Fact]
        public async Task ConsumeAsync_WithIgnoreAttribute_DoesNotCallAddResult()
        {
            // Arrange (QASE-05)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var mockReporter = new Mock<ICoreReporter>();
            mockReporter
                .Setup(r => r.addResult(It.IsAny<TestResult>()))
                .Returns(Task.CompletedTask);
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "IgnoredTestMethod");
            var message = CreateTestMessage("IgnoredTestMethod",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Never);
        }

        [Fact]
        public async Task ConsumeAsync_WithQaseIdsAndSuites_GeneratesSignature()
        {
            // Arrange (QASE-09)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "AnnotatedTestMethod");
            var message = CreateTestMessage("SignatureTest",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Signature.Should().NotBeNullOrEmpty();
            // Signature.Generate joins ids and suite titles with "::"
            // IDs 42 and 100 produce "42-100", suites "Auth" and "Login" produce "auth" and "login"
            result.Signature.Should().Contain("42-100");
            result.Signature.Should().Contain("auth");
            result.Signature.Should().Contain("login");
        }

        [Fact]
        public async Task ConsumeAsync_WithContextManagerSteps_RetrievesSteps()
        {
            // Arrange (QASE-06)
            ContextManager.Clear();
            var displayName = "Qase.MSTest.Reporter.Tests.QaseAnnotatedTestFixture.UnannotatedTestMethod";
            ContextManager.SetTestCaseName(displayName);
            ContextManager.StartStep("Step 1");
            ContextManager.PassStep();

            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "UnannotatedTestMethod");
            var message = CreateTestMessage("UnannotatedTestMethod",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Steps.Should().HaveCount(1);
            result.Steps[0].Data!.Action.Should().Be("Step 1");

            // Cleanup
            ContextManager.Clear();
        }

        [Fact]
        public async Task ConsumeAsync_WithContextManagerComments_RetrievesComments()
        {
            // Arrange (QASE-08)
            ContextManager.Clear();
            var displayName = "Qase.MSTest.Reporter.Tests.QaseAnnotatedTestFixture.MethodWithTitleOnly";
            ContextManager.SetTestCaseName(displayName);
            Metadata.Comment("Test comment");

            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "MethodWithTitleOnly");
            var message = CreateTestMessage("MethodWithTitleOnly",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Message.Should().Contain("Test comment");

            // Cleanup
            ContextManager.Clear();
        }

        [Fact]
        public async Task ConsumeAsync_WithContextManagerAttachments_RetrievesAttachments()
        {
            // Arrange (QASE-07)
            ContextManager.Clear();
            var displayName = "Qase.MSTest.Reporter.Tests.QaseAnnotatedTestFixture.UnannotatedTestMethod";
            ContextManager.SetTestCaseName(displayName);
            Metadata.Attach("/tmp/test-screenshot.png");

            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "QaseAnnotatedTestFixture", "UnannotatedTestMethod");
            var message = CreateTestMessage("UnannotatedTestMethod",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Attachments.Should().HaveCount(1);
            result.Attachments[0].FilePath.Should().Be("/tmp/test-screenshot.png");
            result.Attachments[0].FileName.Should().Be("test-screenshot.png");

            // Cleanup
            ContextManager.Clear();
        }

        [Fact]
        public async Task ConsumeAsync_WithoutTestMethodIdentifierProperty_FallsBackGracefully()
        {
            // Arrange - no TestMethodIdentifierProperty in the message
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var message = CreateTestMessage("FallbackDisplayName", new PassedTestNodeStateProperty());

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            mockReporter.Verify(r => r.addResult(It.IsAny<TestResult>()), Times.Once);
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Title.Should().Be("FallbackDisplayName");
            // Without TestMethodIdentifierProperty, signature falls back to title-based value
            result.Signature.Should().Be("fallbackdisplayname");
            // Relations default from constructor (empty suite data)
            result.Relations.Should().NotBeNull();
            result.Relations!.Suite.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task ConsumeAsync_WithClassLevelSuitesAttribute_UsesSuitesFromClass()
        {
            // Arrange (QASE-10 + QASE-04)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ClassLevelSuiteFixture", "SomeTest");
            var message = CreateTestMessage("SomeTest",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Relations.Should().NotBeNull();
            result.Relations!.Suite.Data.Should().HaveCount(1);
            result.Relations.Suite.Data[0].Title.Should().Be("ClassLevel Suite");
        }

        // =====================================================================
        // Parameterized test parameter extraction tests (PARAM-01, PARAM-02, PARAM-03)
        // =====================================================================

        [Fact]
        public async Task ConsumeAsync_DataRowIntegerParams_ExtractsParamsCorrectly()
        {
            // Arrange (PARAM-01: DataRow with integer parameters)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "AddNumbers",
                new[] { "System.Int32", "System.Int32" });
            var message = CreateTestMessage("AddNumbers (3,5)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(2);
            result.Params.Should().ContainKey("a").WhoseValue.Should().Be("3");
            result.Params.Should().ContainKey("b").WhoseValue.Should().Be("5");
            result.Title.Should().Be("AddNumbers");
        }

        [Fact]
        public async Task ConsumeAsync_DataRowStringParams_ExtractsParamsCorrectly()
        {
            // Arrange (PARAM-01: DataRow with string parameters)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "LoginUser",
                new[] { "System.String", "System.String" });
            var message = CreateTestMessage("LoginUser (admin,secret123)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(2);
            result.Params.Should().ContainKey("username").WhoseValue.Should().Be("admin");
            result.Params.Should().ContainKey("password").WhoseValue.Should().Be("secret123");
        }

        [Fact]
        public async Task ConsumeAsync_DynamicDataMixedTypes_ExtractsParamsCorrectly()
        {
            // Arrange (PARAM-02: DynamicData with mixed type parameters)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "MixedParams",
                new[] { "System.Int32", "System.String", "System.Boolean" });
            var message = CreateTestMessage("MixedParams (42,TestUser,True)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(3);
            result.Params.Should().ContainKey("id").WhoseValue.Should().Be("42");
            result.Params.Should().ContainKey("name").WhoseValue.Should().Be("TestUser");
            result.Params.Should().ContainKey("active").WhoseValue.Should().Be("True");
        }

        [Fact]
        public async Task ConsumeAsync_QuotedStringWithInnerComma_ParsedAsSingleValue()
        {
            // Arrange (PARAM-03: Quoted string parameters with commas)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "SingleParam",
                new[] { "System.String" });
            var message = CreateTestMessage("SingleParam (\"hello, world\")",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(1);
            result.Params.Should().ContainKey("value").WhoseValue.Should().Be("hello, world");
        }

        [Fact]
        public async Task ConsumeAsync_NullParameterValue_PreservedAsLiteralString()
        {
            // Arrange (PARAM-03: Null literal parameter values)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "LoginUser",
                new[] { "System.String", "System.String" });
            var message = CreateTestMessage("LoginUser (null,secret123)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(2);
            result.Params.Should().ContainKey("username").WhoseValue.Should().Be("null");
            result.Params.Should().ContainKey("password").WhoseValue.Should().Be("secret123");
        }

        [Fact]
        public async Task ConsumeAsync_ParameterizedWithTitleAttribute_KeepsCustomTitle()
        {
            // Arrange (PARAM-03: Title attribute overrides even for parameterized tests)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "TitledParameterized",
                new[] { "System.Int32" });
            var message = CreateTestMessage("TitledParameterized (7)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Title.Should().Be("Custom Parameterized Title");
            result.Params.Should().HaveCount(1);
            result.Params.Should().ContainKey("x").WhoseValue.Should().Be("7");
        }

        [Fact]
        public async Task ConsumeAsync_ParameterizedSignature_IncludesParameterValues()
        {
            // Arrange (PARAM-03: Signature includes parameter values)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "AddNumbers",
                new[] { "System.Int32", "System.Int32" });
            var message = CreateTestMessage("AddNumbers (3,5)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Signature.Should().NotBeNullOrEmpty();
            // Signature.Generate includes params as "key":"value" joined with "::"
            result.Signature.Should().Contain("\"a\":\"3\"");
            result.Signature.Should().Contain("\"b\":\"5\"");
        }

        [Fact]
        public async Task ConsumeAsync_ParameterizedContextManager_RetrievesStepsViaParamDisplayName()
        {
            // Arrange (PARAM-03: ContextManager display name includes parameters for step association)
            ContextManager.Clear();
            var contextDisplayName = "Qase.MSTest.Reporter.Tests.ParameterizedTestFixture.AddNumbers(a: 10, b: 20)";
            ContextManager.SetTestCaseName(contextDisplayName);
            ContextManager.StartStep("Parameterized Step");
            ContextManager.PassStep();

            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "AddNumbers",
                new[] { "System.Int32", "System.Int32" });
            var message = CreateTestMessage("AddNumbers (10,20)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Steps.Should().HaveCount(1);

            // Cleanup
            ContextManager.Clear();
        }

        [Fact]
        public async Task ConsumeAsync_CustomDisplayNameWithoutParams_FallsBackToEmptyParams()
        {
            // Arrange (PARAM-03: Custom DisplayName without parenthesized params)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "ParameterizedTestFixture", "AddNumbers",
                new[] { "System.Int32", "System.Int32" });
            var message = CreateTestMessage("Custom DataRow Display Name",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().BeEmpty();
            result.Title.Should().Be("Custom DataRow Display Name");
        }

        [Fact]
        public async Task ConsumeAsync_OverloadedMethodResolvedViaParameterTypeFullNames()
        {
            // Arrange (PARAM-03: Overloaded method resolved via ParameterTypeFullNames)
            var extension = CreateExtensionWithConfig(Mode.TestOps);
            var (mockReporter, getCaptured) = CreateCapturingReporter();
            InjectReporter(extension, mockReporter.Object);

            var methodId = CreateMethodIdentifier(
                "Qase.MSTest.Reporter.Tests", "OverloadedTestFixture", "Calculate",
                new[] { "System.String" });
            var message = CreateTestMessage("Calculate (test-expression)",
                new PassedTestNodeStateProperty(), methodId);

            // Act
            await extension.ConsumeAsync(Mock.Of<IDataProducer>(), message, CancellationToken.None);

            // Assert
            var result = getCaptured();
            result.Should().NotBeNull();
            result!.Params.Should().HaveCount(1);
            result.Params.Should().ContainKey("expression").WhoseValue.Should().Be("test-expression");
        }
    }
}
