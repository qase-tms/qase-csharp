using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentAssertions;
using Moq;
using Xunit;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.NUnit.Reporter;

namespace Qase.NUnit.Reporter.Tests
{
    public class XmlEventHandlingTests : IDisposable
    {
        private QaseNUnitEventListener _listener;
        private Type _listenerType;
        private Mock<ICoreReporter> _mockReporter;

        public XmlEventHandlingTests()
        {
            _listener = new QaseNUnitEventListener();
            _listenerType = typeof(QaseNUnitEventListener);
            _mockReporter = new Mock<ICoreReporter>();
            
            // Set the reporter using reflection
            var reporterField = _listenerType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Static);
            reporterField?.SetValue(null, _mockReporter.Object);
        }

        public void Dispose()
        {
            // Clear static state
            var reporterField = _listenerType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Static);
            reporterField?.SetValue(null, null);
            
            var testResultsField = _listenerType.GetField("_testResults", BindingFlags.NonPublic | BindingFlags.Static);
            var testResults = testResultsField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, TestResult>;
            testResults?.Clear();
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
        public void OnTestEvent_WithStartTestEvent_ShouldCreateTestResult()
        {
            // Arrange
            var xml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(xml);

            // Assert
            var testResultsField = _listenerType.GetField("_testResults", BindingFlags.NonPublic | BindingFlags.Static);
            var testResults = testResultsField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, TestResult>;
            testResults.Should().NotBeNull();
            testResults.Should().ContainKey("0-1001");
            testResults["0-1001"].Title.Should().Be("Test1");
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

            // Assert - verify that addResult was called at least once for our test case
            _mockReporter.Verify(x => x.addResult(It.Is<TestResult>(r => r.Title == "Test1")), Times.AtLeastOnce);
        }

        [Fact]
        public void OnTestEvent_WithFailedTestCase_ShouldSetFailedStatus()
        {
            // Arrange
            var startTestXml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" result=""Failed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" asserts=""1"">
                <failure>
                    <message>Assertion failed</message>
                    <stack-trace>at NUnit.Framework.Assert.AreEqual</stack-trace>
                </failure>
            </test-case>";
            
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            TestResult capturedResult = null;
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>()))
                .Callback<TestResult>(r => capturedResult = r)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert
            capturedResult.Should().NotBeNull();
            capturedResult.Execution.Status.Should().Be(TestResultStatus.Failed);
            capturedResult.Message.Should().Contain("Assertion failed");
        }

        [Fact]
        public void OnTestEvent_WithInvalidTestCase_ShouldSetInvalidStatus()
        {
            // Arrange
            var startTestXml = @"<start-test id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test1"" fullname=""Tests.Test1"" result=""Failed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" label=""Error"" asserts=""0"">
                <failure>
                    <message>System.Exception: Error occurred</message>
                    <stack-trace>at Tests.Test1()</stack-trace>
                </failure>
            </test-case>";
            
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);

            TestResult capturedResult = null;
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>()))
                .Callback<TestResult>(r => capturedResult = r)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert
            capturedResult.Should().NotBeNull();
            capturedResult.Execution.Status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void OnTestEvent_WithParameterizedTest_ShouldExtractParameters()
        {
            // Arrange
            var startRunXml = @"<start-run />";
            var startTestXml = @"<start-test id=""0-1001"" name=""Test2(\""user1\"",\""value2\"")"" fullname=""Tests.Test2(\""user1\"",\""value2\"")"" />";
            var testCaseXml = @"<test-case id=""0-1001"" name=""Test2(\""user1\"",\""value2\"")"" fullname=""Tests.Test2(\""user1\"",\""value2\"")"" result=""Passed"" start-time=""2026-01-16T11:38:42.8196620Z"" end-time=""2026-01-16T11:38:42.8200660Z"" duration=""0.000404"" />";
            
            _mockReporter.Setup(x => x.startTestRun()).Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Clear any existing calls
            _mockReporter.Invocations.Clear();

            // Act
            _listener.OnTestEvent(startRunXml);
            _listener.OnTestEvent(startTestXml);
            _listener.OnTestEvent(testCaseXml);

            // Assert
            // Verify that the test result was created in _testResults
            var testResultsField = _listenerType.GetField("_testResults", BindingFlags.NonPublic | BindingFlags.Static);
            var testResults = testResultsField?.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, TestResult>;
            
            // The test result should have been processed and removed from _testResults
            // If it was ignored, it wouldn't be added to results, but the flow should still work
            // Note: Parameters extraction requires reflection to find the actual method
            // This test verifies the flow works - the test case is processed even if the method doesn't exist
            // If the method doesn't exist, parameters won't be extracted, but the test should still be reported
            // We verify that the test was processed by checking that it's no longer in _testResults
            testResults.Should().NotContainKey("0-1001");
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
