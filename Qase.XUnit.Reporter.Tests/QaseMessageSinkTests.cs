using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Moq;

namespace Qase.XUnit.Reporter.Tests
{
    public class QaseMessageSinkTests
    {
        #region IsAssertionFailure Tests

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
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeTrue();
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
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeFalse();
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
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("at Xunit.Assert.True")]
        [InlineData("at Xunit.Assert.False")]
        [InlineData("at Xunit.Assert.Null")]
        [InlineData("at Xunit.Assert.NotNull")]
        [InlineData("at Xunit.Assert.Empty")]
        [InlineData("at Xunit.Assert.NotEmpty")]
        [InlineData("at Xunit.Assert.Contains")]
        [InlineData("at Xunit.Assert.DoesNotContain")]
        [InlineData("at Xunit.Assert.InRange")]
        [InlineData("at Xunit.Assert.NotInRange")]
        [InlineData("at Xunit.Assert.Single")]
        [InlineData("at Xunit.Assert.Collection")]
        [InlineData("at Xunit.Assert.Throws")]
        [InlineData("at Xunit.Assert.DoesNotThrow")]
        public void IsAssertionFailure_WithVariousAssertMethods_ShouldReturnTrue(string assertMethod)
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            var stackTraces = new List<string>
            {
                $"   {assertMethod}(...) in /_/src/xunit.assert/Asserts/SomeAsserts.cs:line 1",
                "   at TestClass.TestMethod() in TestClass.cs:line 10"
            };
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAssertionFailure_WithEmptyStackTrace_ShouldReturnFalse()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            mockTestFailed.Setup(x => x.StackTraces).Returns(Array.Empty<string>());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsAssertionFailure_WithNullStackTrace_ShouldReturnFalse()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            mockTestFailed.Setup(x => x.StackTraces).Returns((string[]?)null);

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsAssertionFailure_WithSystemExceptionInStackTrace_ShouldReturnFalse()
        {
            // Arrange
            var mockTestFailed = new Mock<ITestFailed>();
            var stackTraces = new List<string>
            {
                "   at System.Exception..ctor()",
                "   at TestClass.TestMethod() in TestClass.cs:line 10"
            };
            mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

            // Act
            var result = InvokeIsAssertionFailure(mockTestFailed.Object);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        private bool InvokeIsAssertionFailure(ITestFailed testFailed)
        {
            // Get type from assembly since class is internal
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Qase.XUnit.Reporter");
            
            if (assembly == null)
            {
                // Try to load from project reference
                var currentDir = Directory.GetCurrentDirectory();
                var dllPath = Path.Combine(currentDir, "..", "Qase.XUnit.Reporter", "bin", "Debug", "net6.0", "Qase.XUnit.Reporter.dll");
                if (File.Exists(dllPath))
                {
                    assembly = Assembly.LoadFrom(dllPath);
                }
            }
            
            if (assembly == null)
            {
                throw new InvalidOperationException("Could not find Qase.XUnit.Reporter assembly");
            }
            
            var sinkType = assembly.GetType("Qase.Xunit.Reporter.QaseMessageSink");
            if (sinkType == null)
            {
                throw new InvalidOperationException("Could not find QaseMessageSink type");
            }
            
            var method = sinkType.GetMethod("IsAssertionFailure", BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find IsAssertionFailure method");
            }
            
            return (bool)method.Invoke(null, new object[] { testFailed })!;
        }
    }
}
