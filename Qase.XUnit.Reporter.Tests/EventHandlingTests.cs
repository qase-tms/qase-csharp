using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
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
            _sinkType = GetSinkType();
            
            // Create instance using reflection since class is internal
            _sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;
            
            // Set the reporter using reflection
            var reporterField = _sinkType.GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Replace reporter with mock after creation
            reporterField?.SetValue(_sink, _mockReporter.Object);
        }

        private Type GetSinkType()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Qase.XUnit.Reporters");
            
            if (assembly == null)
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dllPath = Path.Combine(currentDir, "..", "Qase.XUnit.Reporter", "bin", "Debug", "net6.0", "Qase.XUnit.Reporters.dll");
                if (File.Exists(dllPath))
                {
                    assembly = Assembly.LoadFrom(dllPath);
                }
            }
            
            if (assembly == null)
            {
                throw new InvalidOperationException("Could not find Qase.XUnit.Reporters assembly");
            }
            
            var sinkType = assembly.GetType("Qase.Xunit.Reporter.QaseMessageSink");
            if (sinkType == null)
            {
                throw new InvalidOperationException("Could not find QaseMessageSink type");
            }
            
            return sinkType;
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
            var mockArgs = new Mock<MessageHandlerArgs<ITestAssemblyExecutionStarting>>();
            var mockMessage = new Mock<ITestAssemblyExecutionStarting>();
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);
            _mockReporter.Setup(x => x.startTestRun()).Returns(Task.CompletedTask);

            // Act
            var method = _sinkType.GetMethod("OnTestAssemblyExecutionStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

            // Assert
            _mockReporter.Verify(x => x.startTestRun(), Times.Once);
        }

        [Fact]
        public void OnTestAssemblyExecutionFinished_ShouldCallUploadAndComplete()
        {
            // Arrange
            var mockArgs = new Mock<MessageHandlerArgs<ITestAssemblyExecutionFinished>>();
            var mockMessage = new Mock<ITestAssemblyExecutionFinished>();
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);
            _mockReporter.Setup(x => x.uploadResults()).Returns(Task.CompletedTask);
            _mockReporter.Setup(x => x.completeTestRun()).Returns(Task.CompletedTask);

            // Act
            var method = _sinkType.GetMethod("OnTestAssemblyExecutionFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            
            var mockArgs = new Mock<MessageHandlerArgs<ITestPassed>>();
            var mockMessage = new Mock<ITestPassed>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockMessage.Setup(x => x.ExecutionTime).Returns(0.5m);
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestPassed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            
            var mockArgs = new Mock<MessageHandlerArgs<ITestFailed>>();
            var mockMessage = new Mock<ITestFailed>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockMessage.Setup(x => x.ExecutionTime).Returns(0.3m);
            mockMessage.Setup(x => x.Messages).Returns(new[] { "Assertion failed" });
            mockMessage.Setup(x => x.StackTraces).Returns(new[] { "at Xunit.Assert.Equal" });
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestFailed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            
            var mockArgs = new Mock<MessageHandlerArgs<ITestFailed>>();
            var mockMessage = new Mock<ITestFailed>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockMessage.Setup(x => x.ExecutionTime).Returns(0.2m);
            mockMessage.Setup(x => x.Messages).Returns(new[] { "System.Exception: Error occurred" });
            mockMessage.Setup(x => x.StackTraces).Returns(new[] { "at System.Exception..ctor()" });
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestFailed", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            
            var mockArgs = new Mock<MessageHandlerArgs<ITestSkipped>>();
            var mockMessage = new Mock<ITestSkipped>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockMessage.Setup(x => x.ExecutionTime).Returns(0.1m);
            mockMessage.Setup(x => x.Reason).Returns("Test was skipped");
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestSkipped", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            
            var mockArgs = new Mock<MessageHandlerArgs<ITestFinished>>();
            var mockMessage = new Mock<ITestFinished>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(Task.CompletedTask);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

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
            var mockTestCase = CreateMockTestCase("IgnoredTestMethod", "TestClassWithAttributes");
            mockTestCase.Setup(x => x.DisplayName).Returns("TestClassWithAttributes.IgnoredTestMethod");
            mockTest.Setup(x => x.TestCase).Returns(mockTestCase.Object);

            var mockArgs = new Mock<MessageHandlerArgs<ITestFinished>>();
            var mockMessage = new Mock<ITestFinished>();
            mockMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockArgs.Setup(x => x.Message).Returns(mockMessage.Object);

            _mockReporter.Setup(x => x.addResult(It.IsAny<TestResult>())).Returns(Task.CompletedTask);

            // First call OnTestStarting to create the test result
            var onTestStartingMethod = _sinkType.GetMethod("OnTestStarting", BindingFlags.NonPublic | BindingFlags.Instance);
            var mockStartingArgs = new Mock<MessageHandlerArgs<ITestStarting>>();
            var mockStartingMessage = new Mock<ITestStarting>();
            mockStartingMessage.Setup(x => x.Test).Returns(mockTest.Object);
            mockStartingArgs.Setup(x => x.Message).Returns(mockStartingMessage.Object);
            onTestStartingMethod?.Invoke(_sink, new object[] { mockStartingArgs.Object });

            // Act
            var method = _sinkType.GetMethod("OnTestFinished", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_sink, new object[] { mockArgs.Object });

            // Assert
            _mockReporter.Verify(x => x.addResult(It.IsAny<TestResult>()), Times.Never);
        }

        private Mock<ITestCase> CreateMockTestCase(string methodName, string className)
        {
            var mockTestCase = new Mock<ITestCase>();
            var mockMethod = new Mock<IMethodInfo>();
            mockMethod.Setup(x => x.Name).Returns(methodName);
            mockMethod.Setup(x => x.GetParameters()).Returns(Array.Empty<IParameterInfo>());
            mockMethod.Setup(x => x.GetCustomAttributes(It.IsAny<Type>())).Returns(Array.Empty<IAttributeInfo>());

            var mockTypeInfo = new Mock<ITypeInfo>();
            mockTypeInfo.Setup(x => x.Name).Returns(className);
            mockTypeInfo.Setup(x => x.GetCustomAttributes(It.IsAny<Type>())).Returns(Array.Empty<IAttributeInfo>());

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
