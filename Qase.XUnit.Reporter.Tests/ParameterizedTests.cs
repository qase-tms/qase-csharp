using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter.Tests
{
    public class ParameterizedTests
    {
        private object _sink;
        private Type _sinkType;
        private Mock<IRunnerLogger> _mockLogger;

        public ParameterizedTests()
        {
            _mockLogger = new Mock<IRunnerLogger>();
            _sinkType = GetSinkType();
            // Don't create instance in constructor - create it only when needed in tests
        }

        [Fact]
        public void CreateBaseTestResult_WithStringParameters_ShouldExtractParameters()
        {
            // Arrange
            var mockParam1 = new Mock<IParameterInfo>();
            mockParam1.Setup(x => x.Name).Returns("param1");
            var mockParam2 = new Mock<IParameterInfo>();
            mockParam2.Setup(x => x.Name).Returns("param2");
            var parameters = new[] { mockParam1.Object, mockParam2.Object };
            var arguments = new object[] { "value1", "value2" };

            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass", parameters, arguments);
            var method = _sinkType.GetMethod("CreateBaseTestResult", BindingFlags.NonPublic | BindingFlags.Instance);
            // Create instance only when needed
            var sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Act
            var result = method?.Invoke(sink, new object[] { mockTestCase.Object }) as TestResult;

            // Assert
            result.Should().NotBeNull();
            result.Params.Should().HaveCount(2);
            result.Params.Should().ContainKey("param1");
            result.Params.Should().ContainKey("param2");
            result.Params["param1"].Should().Be("value1");
            result.Params["param2"].Should().Be("value2");
        }

        [Fact]
        public void CreateBaseTestResult_WithMixedParameterTypes_ShouldExtractParameters()
        {
            // Arrange
            var mockParam1 = new Mock<IParameterInfo>();
            mockParam1.Setup(x => x.Name).Returns("stringParam");
            var mockParam2 = new Mock<IParameterInfo>();
            mockParam2.Setup(x => x.Name).Returns("intParam");
            var mockParam3 = new Mock<IParameterInfo>();
            mockParam3.Setup(x => x.Name).Returns("boolParam");
            var parameters = new[] { mockParam1.Object, mockParam2.Object, mockParam3.Object };
            var arguments = new object[] { "test", 42, true };

            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass", parameters, arguments);
            var method = _sinkType.GetMethod("CreateBaseTestResult", BindingFlags.NonPublic | BindingFlags.Instance);
            // Create instance only when needed
            var sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Act
            var result = method?.Invoke(sink, new object[] { mockTestCase.Object }) as TestResult;

            // Assert
            result.Should().NotBeNull();
            result.Params.Should().HaveCount(3);
            result.Params["stringParam"].Should().Be("test");
            result.Params["intParam"].Should().Be("42");
            result.Params["boolParam"].Should().Be("True");
        }

        [Fact]
        public void CreateBaseTestResult_WithNullParameter_ShouldHandleNull()
        {
            // Arrange
            var mockParam = new Mock<IParameterInfo>();
            mockParam.Setup(x => x.Name).Returns("param1");
            var parameters = new[] { mockParam.Object };
            var arguments = new object[] { null };

            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass", parameters, arguments);
            var method = _sinkType.GetMethod("CreateBaseTestResult", BindingFlags.NonPublic | BindingFlags.Instance);
            // Create instance only when needed
            var sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Act
            var result = method?.Invoke(sink, new object[] { mockTestCase.Object }) as TestResult;

            // Assert
            result.Should().NotBeNull();
            result.Params.Should().HaveCount(1);
            result.Params["param1"].Should().Be("null");
        }

        [Fact]
        public void CreateBaseTestResult_WithEmptyStringParameter_ShouldHandleEmpty()
        {
            // Arrange
            var mockParam = new Mock<IParameterInfo>();
            mockParam.Setup(x => x.Name).Returns("param1");
            var parameters = new[] { mockParam.Object };
            var arguments = new object[] { "" };

            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass", parameters, arguments);
            var method = _sinkType.GetMethod("CreateBaseTestResult", BindingFlags.NonPublic | BindingFlags.Instance);
            // Create instance only when needed
            var sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Act
            var result = method?.Invoke(sink, new object[] { mockTestCase.Object }) as TestResult;

            // Assert
            result.Should().NotBeNull();
            result.Params.Should().HaveCount(1);
            result.Params["param1"].Should().Be("empty");
        }

        [Fact]
        public void CreateBaseTestResult_WithNoParameters_ShouldHaveEmptyParams()
        {
            // Arrange
            var mockTestCase = CreateMockTestCase("TestMethod", "TestClass", Array.Empty<IParameterInfo>(), Array.Empty<object>());
            var method = _sinkType.GetMethod("CreateBaseTestResult", BindingFlags.NonPublic | BindingFlags.Instance);
            // Create instance only when needed
            var sink = Activator.CreateInstance(_sinkType, _mockLogger.Object)!;

            // Act
            var result = method?.Invoke(sink, new object[] { mockTestCase.Object }) as TestResult;

            // Assert
            result.Should().NotBeNull();
            result.Params.Should().BeEmpty();
        }

        private Type GetSinkType()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Qase.XUnit.Reporters");
            
            if (assembly == null)
            {
                var currentDir = System.IO.Directory.GetCurrentDirectory();
                var dllPath = System.IO.Path.Combine(currentDir, "..", "Qase.XUnit.Reporter", "bin", "Debug", "net6.0", "Qase.XUnit.Reporters.dll");
                if (System.IO.File.Exists(dllPath))
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

        private Mock<ITestCase> CreateMockTestCase(string methodName, string className, IParameterInfo[] parameters, object[] arguments)
        {
            var mockTestCase = new Mock<ITestCase>();
            var mockMethod = new Mock<IMethodInfo>();
            mockMethod.Setup(x => x.Name).Returns(methodName);
            mockMethod.Setup(x => x.GetParameters()).Returns(parameters);
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
            mockTestCase.Setup(x => x.TestMethodArguments).Returns(arguments);
            mockTestCase.Setup(x => x.DisplayName).Returns($"{className}.{methodName}");

            return mockTestCase;
        }
    }
}
