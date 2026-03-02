using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Xunit.Reporter;
using Xunit;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter.Tests
{
    public class EventHandlingTests : IDisposable
    {
        private object _sink;
        private Type _sinkType;
        private Mock<ICoreReporter> _mockReporter;
        private Mock<IRunnerLogger> _mockLogger;

        public EventHandlingTests()
        {
            _mockLogger = new Mock<IRunnerLogger>();
            _mockReporter = new Mock<ICoreReporter>();
            _sinkType = typeof(QaseMessageSink);

            // Create instance - accessible via InternalsVisibleTo
            _sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Set the reporter using reflection (private field)
            var reporterField = _sinkType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Instance);
            reporterField?.SetValue(_sink, _mockReporter.Object);
        }

        public void Dispose()
        {
            // Clear static state
            var currentSinkField = _sinkType.GetProperty("CurrentSink", BindingFlags.Public | BindingFlags.Static);
            currentSinkField?.SetValue(null, null);
        }

        [Fact]
        public void OnTestAssemblyExecutionStarting_ShouldCallStartTestRun()
        {
            // Arrange
            var mockMessage = new Mock<ITestAssemblyExecutionStarting>();
            var args = new MessageHandlerArgs<ITestAssemblyExecutionStarting>(mockMessage.Object);
            _mockReporter.Setup(x => x.startTestRun()).Returns(Task.CompletedTask);

            // Act
            var method = _sinkType.GetMethod("OnTestAssemblyExecutionStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { args });

            // Assert
            _mockReporter.Verify(x => x.startTestRun(), Times.Once);
        }

        [Fact]
        public void OnTestAssemblyExecutionFinished_ShouldCallUploadAndComplete()
        {
            // Arrange
            var mockMessage = new Mock<ITestAssemblyExecutionFinished>();
            var args = new MessageHandlerArgs<ITestAssemblyExecutionFinished>(mockMessage.Object);
            _mockReporter.Setup(x => x.uploadResults()).Returns(Task.CompletedTask);
            _mockReporter.Setup(x => x.completeTestRun()).Returns(Task.CompletedTask);

            // Act
            var method = _sinkType.GetMethod("OnTestAssemblyExecutionFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { args });

            // Assert
            _mockReporter.Verify(x => x.uploadResults(), Times.Once);
            _mockReporter.Verify(x => x.completeTestRun(), Times.Once);
        }

        [Fact]
        public void OnTestPassed_ShouldSetPassedStatus()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockPassedMessage = new Mock<ITestPassed>();
            mockPassedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockPassedMessage.Setup(x => x.ExecutionTime).Returns(0.5m);
            var passedArgs = new MessageHandlerArgs<ITestPassed>(mockPassedMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestPassed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { passedArgs });

            // Assert
            var qaseTestDataField = _sinkType.GetField("qaseTestData", BindingFlags.NonPublic | BindingFlags.Instance);
            var qaseTestData = qaseTestDataField?.GetValue(_sink) as System.Collections.Concurrent.ConcurrentDictionary<ITest, TestResult>;
            qaseTestData.Should().NotBeNull();
            qaseTestData.Should().ContainKey(mockTest.Object);
            qaseTestData[mockTest.Object].Execution.Status.Should().Be(TestResultStatus.Passed);
            qaseTestData[mockTest.Object].Execution.Duration.Should().Be(500); // 0.5 seconds = 500 milliseconds
        }

        [Fact]
        public void OnTestFailed_WithAssertionFailure_ShouldSetFailedStatus()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockFailedMessage = new Mock<ITestFailed>();
            mockFailedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockFailedMessage.Setup(x => x.ExecutionTime).Returns(0.3m);
            mockFailedMessage.Setup(x => x.Messages).Returns(new[] { "Assertion failed" });
            mockFailedMessage.Setup(x => x.StackTraces).Returns(new[] { "at Xunit.Assert.Equal" });
            var failedArgs = new MessageHandlerArgs<ITestFailed>(mockFailedMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestFailed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { failedArgs });

            // Assert
            var qaseTestDataField = _sinkType.GetField("qaseTestData", BindingFlags.NonPublic | BindingFlags.Instance);
            var qaseTestData = qaseTestDataField?.GetValue(_sink) as System.Collections.Concurrent.ConcurrentDictionary<ITest, TestResult>;
            qaseTestData.Should().NotBeNull();
            qaseTestData.Should().ContainKey(mockTest.Object);
            qaseTestData[mockTest.Object].Execution.Status.Should().Be(TestResultStatus.Failed);
            qaseTestData[mockTest.Object].Message.Should().Contain("Assertion failed");
        }

        [Fact]
        public void OnTestFailed_WithExceptionFailure_ShouldSetInvalidStatus()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockFailedMessage = new Mock<ITestFailed>();
            mockFailedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockFailedMessage.Setup(x => x.ExecutionTime).Returns(0.2m);
            mockFailedMessage.Setup(x => x.Messages).Returns(new[] { "System.Exception: Error occurred" });
            mockFailedMessage.Setup(x => x.StackTraces).Returns(new[] { "at System.Exception..ctor()" });
            var failedArgs = new MessageHandlerArgs<ITestFailed>(mockFailedMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestFailed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { failedArgs });

            // Assert
            var qaseTestDataField = _sinkType.GetField("qaseTestData", BindingFlags.NonPublic | BindingFlags.Instance);
            var qaseTestData = qaseTestDataField?.GetValue(_sink) as System.Collections.Concurrent.ConcurrentDictionary<ITest, TestResult>;
            qaseTestData.Should().NotBeNull();
            qaseTestData.Should().ContainKey(mockTest.Object);
            qaseTestData[mockTest.Object].Execution.Status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void OnTestSkipped_ShouldSetSkippedStatus()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockSkippedMessage = new Mock<ITestSkipped>();
            mockSkippedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockSkippedMessage.Setup(x => x.ExecutionTime).Returns(0.1m);
            mockSkippedMessage.Setup(x => x.Reason).Returns("Test was skipped");
            var skippedArgs = new MessageHandlerArgs<ITestSkipped>(mockSkippedMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestSkipped", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { skippedArgs });

            // Assert
            var qaseTestDataField = _sinkType.GetField("qaseTestData", BindingFlags.NonPublic | BindingFlags.Instance);
            var qaseTestData = qaseTestDataField?.GetValue(_sink) as System.Collections.Concurrent.ConcurrentDictionary<ITest, TestResult>;
            qaseTestData.Should().NotBeNull();
            qaseTestData.Should().ContainKey(mockTest.Object);
            qaseTestData[mockTest.Object].Execution.Status.Should().Be(TestResultStatus.Skipped);
            qaseTestData[mockTest.Object].Message.Should().Be("Test was skipped");
        }

        [Fact]
        public void OnTestFinished_ShouldCallAddResult()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass");
            mockTestCase.Setup(x => x.DisplayName).Returns("TestClass.TestMethod");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockFinishedMessage = new Mock<ITestFinished>();
            mockFinishedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var finishedArgs = new MessageHandlerArgs<ITestFinished>(mockFinishedMessage.Object);

            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(Task.CompletedTask);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { finishedArgs });

            // Assert
            _mockReporter.Verify(x => x.addResult(It.IsAny<TestResult>()), Times.Once);

            // Verify test result was removed from dictionary
            var qaseTestDataField = _sinkType.GetField("qaseTestData", BindingFlags.NonPublic | BindingFlags.Instance);
            var qaseTestData = qaseTestDataField?.GetValue(_sink) as System.Collections.Concurrent.ConcurrentDictionary<ITest, TestResult>;
            qaseTestData.Should().NotContainKey(mockTest.Object);
        }

        [Fact]
        public void OnTestFinished_WithIgnoredTest_ShouldNotCallAddResult()
        {
            // Arrange
            var mockTest = new Mock<ITest>();
            var mockTestCase = CreateMockTestCase("IgnoredTestMethod", "TestClassWithAttributes",
                new Qase.Csharp.Commons.Attributes.IgnoreAttribute());
            mockTestCase.Setup(x => x.DisplayName).Returns("TestClassWithAttributes.IgnoredTestMethod");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockFinishedMessage = new Mock<ITestFinished>();
            mockFinishedMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var finishedArgs = new MessageHandlerArgs<ITestFinished>(mockFinishedMessage.Object);

            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(Task.CompletedTask);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            var startingArgs = new MessageHandlerArgs<ITestStarting>(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { startingArgs });

            // Act
            var method = _sinkType.GetMethod("OnTestFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { finishedArgs });

            // Assert
            _mockReporter.Verify(x => x.addResult(It.IsAny<TestResult>()), Times.Never);
        }

        private Mock<ITestCase> CreateMockTestCase(string methodName, string className, params Attribute[] methodAttributes)
        {
            var mockTestCase = new Mock<ITestCase>();
            var mockMethod = new Mock<IMethodInfo>();
            mockMethod.Setup(x => x.Name).Returns(methodName);
            mockMethod.Setup(x => x.GetParameters()).Returns(Array.Empty<IParameterInfo>());

            var attrInfos = methodAttributes.Select(attr =>
            {
                var mock = new Mock<IReflectionAttributeInfo>();
                mock.Setup(x => x.Attribute).Returns(attr);
                return (IAttributeInfo)mock.Object;
            }).ToList();

            mockMethod.Setup(x => x.GetCustomAttributes(It.IsAny<string>()))
                .Returns(attrInfos);

            var mockTypeInfo = new Mock<ITypeInfo>();
            mockTypeInfo.Setup(x => x.Name).Returns(className);
            mockTypeInfo.Setup(x => x.GetCustomAttributes(It.IsAny<string>()))
                .Returns(Array.Empty<IAttributeInfo>());

            var mockTestClass = new Mock<ITestClass>();
            mockTestClass.Setup(x => x.Class).Returns(mockTypeInfo.Object);

            var mockTestMethod = new Mock<ITestMethod>();
            mockTestMethod.Setup(x => x.Method).Returns(mockMethod.Object);
            mockTestMethod.Setup(x => x.TestClass).Returns(mockTestClass.Object);

            mockTestCase.Setup(x => x.TestMethod).Returns(mockTestMethod.Object);
            mockTestCase.Setup(x => x.TestMethodArguments).Returns(Array.Empty<object>());
            mockTestCase.Setup(x => x.DisplayName).Returns($"{className}.{methodName}");

            return mockTestCase;
        }
    }
}
