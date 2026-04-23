using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentAssertions;
using Moq;
using Xunit;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;
using Qase.NUnit.Reporter;

namespace Qase.NUnit.Reporter.Tests
{
    public class XmlEventHandlingTests : IDisposable
    {
        private QaseNUnitEventListener _listener;
        private Type _listenerType;
        private Mock<ICoreReporter> _mockReporter;
        private Mock<ITestResultBuilder> _mockBuilder;

        public XmlEventHandlingTests()
        {
            _listener = new QaseNUnitEventListener();
            _listenerType = typeof(QaseNUnitEventListener);
            _mockReporter = new Mock<ICoreReporter>();
            _mockBuilder = new Mock<ITestResultBuilder>();

            // Set the reporter using reflection
            var reporterField = _listenerType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Static);
            reporterField?.SetValue(null, _mockReporter.Object);

            // Set the builder using reflection (static field)
            var builderField = _listenerType.GetField("_builder", BindingFlags.NonPublic | BindingFlags.Static);
            builderField?.SetValue(null, _mockBuilder.Object);

            // Default: mock builder returns a non-ignored TestResult
            _mockBuilder.Setup(b => b.Build(It.IsAny<RawTestData>()))
                .Returns(new TestResult
                {
                    Title = "Test1",
                    Execution = new TestResultExecution { Status = TestResultStatus.Passed }
                });
        }

        public void Dispose()
        {
            // Clear static state
            var reporterField = _listenerType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Static);
            reporterField?.SetValue(null, null);

            // Restore default builder
            var builderField = _listenerType.GetField("_builder", BindingFlags.NonPublic | BindingFlags.Static);
            builderField?.SetValue(null, new TestResultBuilder());

            var rawTestDataField = _listenerType.GetField("_rawTestData", BindingFlags.NonPublic | BindingFlags.Static);
            var rawTestData = rawTestDataField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, Qase.Csharp.Commons.Models.Domain.RawTestData>;
            rawTestData?.Clear();
        }

        [Fact]
        public void OnTestEvent_WithStartRunEvent_ShouldCallStartTestRun()
        {
            // Arrange
            var xml = @"<start-run />";
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(xml);

            // Assert
            _mockReporter.Verify(x => x.startTestRun(), Times.Once);
        }

        [Fact]
        public void OnTestEvent_WithTestRunEvent_ShouldCallUploadAndComplete()
        {
            // Arrange
            var xml = @"<test-run />";
            _mockReporter.Setup(x => x.uploadResults()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.completeTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(xml);

            // Assert
            _mockReporter.Verify(x => x.uploadResults(), Times.Once);
            _mockReporter.Verify(x => x.completeTestRun(), Times.Once);
        }

        [Fact]
        public void OnTestEvent_WithStartTestEvent_ShouldCreateRawTestData()
        {
            // Arrange
            var xml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(xml);

            // Assert
            var rawTestDataField = _listenerType.GetField("_rawTestData", BindingFlags.NonPublic | BindingFlags.Static);
            var rawTestData = rawTestDataField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, Qase.Csharp.Commons.Models.Domain.RawTestData>;
            rawTestData.Should().NotBeNull();
            rawTestData.Should().ContainKey("0-1001");
            rawTestData!["0-1001"].DisplayName.Should().Be("Test1");
        }

        [Fact]
        public void OnTestEvent_WithTestCaseEvent_ShouldAddResult()
        {
            // Arrange
            var startTestXml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" result=""Passed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" />";

            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Clear any existing calls
            _mockReporter.Invocations.Clear();

            // Act
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert - verify builder was called and result was sent to reporter
            _mockBuilder.Verify(b => b.Build(It.Is<RawTestData>(r => r.Status == TestResultStatus.Passed)), Times.AtLeastOnce);
            _mockReporter.Verify(x => x.addResult(It.IsAny<TestResult>()), Times.AtLeastOnce);
        }

        [Fact]
        public void OnTestEvent_WithFailedTestCase_ShouldSetFailedStatus()
        {
            // Arrange
            var failedResult = new TestResult
            {
                Title = "Test1",
                Message = "Assertion failed",
                Execution = new TestResultExecution { Status = TestResultStatus.Failed }
            };
            _mockBuilder.Setup(b => b.Build(It.Is<RawTestData>(r => r.Status == TestResultStatus.Failed)))
                .Returns(failedResult);

            var startTestXml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" result=""Failed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" asserts=""1"">
                <failure>
                    <message>Assertion failed</message>
                    <stack-trace>at NUnit.Framework.Assert.AreEqual</stack-trace>
                </failure>
            </test-case>";

            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert — verify builder received failed raw data and reporter got the result
            _mockBuilder.Verify(b => b.Build(It.Is<RawTestData>(r =>
                r.Status == TestResultStatus.Failed &&
                r.ErrorMessage == "Assertion failed")), Times.AtLeastOnce);
            _mockReporter.Verify(x => x.addResult(failedResult), Times.Once);
        }

        [Fact]
        public void OnTestEvent_WithInvalidTestCase_ShouldSetInvalidStatus()
        {
            // Arrange
            var invalidResult = new TestResult
            {
                Title = "Test1",
                Execution = new TestResultExecution { Status = TestResultStatus.Invalid }
            };
            _mockBuilder.Setup(b => b.Build(It.Is<RawTestData>(r => r.Status == TestResultStatus.Failed)))
                .Returns(invalidResult);

            var startTestXml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" result=""Failed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" label=""Error"" asserts=""0"">
                <failure>
                    <message>System.Exception: Error occurred</message>
                    <stack-trace>at Tests.Test1()</stack-trace>
                </failure>
            </test-case>";

            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert — verify builder received raw data with Error label and reporter got the invalid result
            _mockBuilder.Verify(b => b.Build(It.Is<RawTestData>(r =>
                r.Status == TestResultStatus.Failed &&
                r.FailureLabel == "Error" &&
                r.AssertsCount == 0)), Times.AtLeastOnce);
            _mockReporter.Verify(x => x.addResult(invalidResult), Times.Once);
        }

        [Fact]
        public void OnTestEvent_WithParameterizedTest_ShouldProcessAndRemoveRawData()
        {
            // Arrange
            var startRunXml = @"<start-run />";
            var startTestXml = @"<start-test id=""0-1001"" name=""Test2(&quot;user1&quot;,&quot;value2&quot;)"" fullname=""Tests.Test2(&quot;user1&quot;,&quot;value2&quot;)"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test2(&quot;user1&quot;,&quot;value2&quot;)"" fullname=""Tests.Test2(&quot;user1&quot;,&quot;value2&quot;)"" result=""Passed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" />";

            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Clear any existing calls
            _mockReporter.Invocations.Clear();

            // Act
            _listener.OnTestEvent(startRunXml);
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert — verify builder was called and raw data was cleaned up
            _mockBuilder.Verify(b => b.Build(It.Is<RawTestData>(r => r.Status == TestResultStatus.Passed)), Times.AtLeastOnce);

            var rawTestDataField = _listenerType.GetField("_rawTestData", BindingFlags.NonPublic | BindingFlags.Static);
            var rawTestData = rawTestDataField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, Qase.Csharp.Commons.Models.Domain.RawTestData>;

            // The raw test data should have been processed (test-case event) and removed
            rawTestData.Should().NotContainKey("0-1001");
        }

        [Fact]
        public void OnTestEvent_WithInvalidXml_ShouldNotThrow()
        {
            // Arrange
            var invalidXml = @"<invalid-xml>";

            // Act & Assert
            _listener.OnTestEvent(invalidXml); // Should not throw
        }

        [Fact]
        public void OnTestEvent_WithEmptyXml_ShouldNotThrow()
        {
            // Arrange
            var emptyXml = "";

            // Act & Assert
            _listener.OnTestEvent(emptyXml); // Should not throw
        }
    }
}
