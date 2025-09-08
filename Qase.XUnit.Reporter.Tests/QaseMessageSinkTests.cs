using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Qase.Xunit.Reporter;
using Moq;

namespace Qase.XUnit.Reporter.Tests
{
    public class QaseMessageSinkTests
    {
        [Fact]
        public void IsAssertionFailure_WithAssertEqualInStackTrace_ShouldReturnTrue()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            var stackTraces = new List<string>
            {
                "   at Xunit.Assert.Equal[T](T expected, T actual, IEqualityComparer`1 comparer) in /_/src/xunit.assert/Asserts/EqualityAsserts.cs:line 154",
                "   at Xunit.Assert.Equal[T](T expected, T actual) in /_/src/xunit.assert/Asserts/EqualityAsserts.cs:line 89",
                "   at xUnitExamples.FailureTypeTests.TestWithAssertionFailure() in /Users/gda/Documents/github/qase-tms/qase-csharp/examples/xUnitExamples/FailureTypeTests.cs:line 16"
            };
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces);

            // Act
            var result = QaseMessageSink.IsAssertionFailure(mockTestFailed.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAssertionFailure_WithExceptionInStackTrace_ShouldReturnFalse()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            var stackTraces = new List<string>
            {
                "   at xUnitExamples.FailureTypeTests.TestWithExceptionFailure() in /Users/gda/Documents/github/qase-tms/qase-csharp/examples/xUnitExamples/FailureTypeTests.cs:line 25",
                "   at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)",
                "   at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)"
            };
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces);

            // Act
            var result = QaseMessageSink.IsAssertionFailure(mockTestFailed.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAssertionFailure_WithAssertFailInStackTrace_ShouldReturnTrue()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            var stackTraces = new List<string>
            {
                "   at Xunit.Assert.Fail(String message) in /_/src/xunit.assert/Asserts/FailAsserts.cs:line 38",
                "   at xUnitExamples.FailureTypeTests.TestWithMultipleAssertionFailures() in /Users/gda/Documents/github/qase-tms/qase-csharp/examples/xUnitExamples/FailureTypeTests.cs:line 62"
            };
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces);

            // Act
            var result = QaseMessageSink.IsAssertionFailure(mockTestFailed.Object);

            // Assert
            Assert.True(result);
        }
    }
}
