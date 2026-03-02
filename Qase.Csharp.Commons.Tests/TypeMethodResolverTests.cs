using Qase.Csharp.Commons.Utils;
using Qase.Csharp.Commons.Tests.Fixtures;

namespace Qase.Csharp.Commons.Tests
{
    public class TypeMethodResolverTests : IDisposable
    {
        public TypeMethodResolverTests()
        {
            TypeMethodResolver.ClearCache();
        }

        public void Dispose()
        {
            TypeMethodResolver.ClearCache();
        }

        // === RESOLVE-01: ResolveType ===

        [Fact]
        public void ResolveType_WithValidFullName_ReturnsCorrectType()
        {
            // Arrange & Act
            var result = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.SampleTestClass");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("SampleTestClass");
        }

        [Fact]
        public void ResolveType_WithAnotherClass_ReturnsCorrectType()
        {
            // Arrange & Act
            var result = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.AnotherTestClass");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("AnotherTestClass");
        }

        [Fact]
        public void ResolveType_WithNonExistentName_ReturnsNull()
        {
            // Arrange & Act
            var result = TypeMethodResolver.ResolveType("NonExistent.Class.Name");

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ResolveType_WithNullOrEmpty_ReturnsNull(string? input)
        {
            // Arrange & Act
            var result = TypeMethodResolver.ResolveType(input);

            // Assert
            result.Should().BeNull();
        }

        // === RESOLVE-02: ResolveMethod ===

        [Fact]
        public void ResolveMethod_WithSimpleName_ReturnsMethod()
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act
            var result = TypeMethodResolver.ResolveMethod(type, "SimpleMethod");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("SimpleMethod");
        }

        [Fact]
        public void ResolveMethod_WithOverloadAndParamTypes_ResolvesCorrectOverload()
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act
            var result = TypeMethodResolver.ResolveMethod(
                type, "OverloadedMethod",
                new[] { "System.String", "System.Int32" });

            // Assert
            result.Should().NotBeNull();
            result!.GetParameters().Should().HaveCount(2);
        }

        [Fact]
        public void ResolveMethod_WithSingleParamOverload_ResolvesCorrectOverload()
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act
            var result = TypeMethodResolver.ResolveMethod(
                type, "OverloadedMethod",
                new[] { "System.Int32" });

            // Assert
            result.Should().NotBeNull();
            result!.GetParameters().Should().HaveCount(1);
            result.GetParameters()[0].ParameterType.Should().Be(typeof(int));
        }

        [Fact]
        public void ResolveMethod_WithOverloadAndNoParamTypes_DoesNotThrow()
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act -- should not throw AmbiguousMatchException
            var result = TypeMethodResolver.ResolveMethod(type, "OverloadedMethod");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("OverloadedMethod");
        }

        [Fact]
        public void ResolveMethod_WithNonExistentMethod_ReturnsNull()
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act
            var result = TypeMethodResolver.ResolveMethod(type, "NonExistentMethod");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ResolveMethod_WithNullType_ReturnsNull()
        {
            // Act
            var result = TypeMethodResolver.ResolveMethod(null!, "Method");

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ResolveMethod_WithNullOrEmptyMethodName_ReturnsNull(string? methodName)
        {
            // Arrange
            var type = typeof(SampleTestClass);

            // Act
            var result = TypeMethodResolver.ResolveMethod(type, methodName!);

            // Assert
            result.Should().BeNull();
        }

        // === RESOLVE-03: Caching ===

        [Fact]
        public void ResolveType_CalledTwice_ReturnsSameReference()
        {
            // Arrange & Act
            var first = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.SampleTestClass");
            var second = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.SampleTestClass");

            // Assert
            first.Should().NotBeNull();
            first.Should().BeSameAs(second);
        }

        [Fact]
        public void ResolveType_WithNonExistentCalledTwice_DoesNotThrow()
        {
            // Arrange & Act
            var first = TypeMethodResolver.ResolveType("NonExistent.Class");
            var second = TypeMethodResolver.ResolveType("NonExistent.Class");

            // Assert
            first.Should().BeNull();
            second.Should().BeNull();
        }

        // === RESOLVE-04: ReflectionTypeLoadException safety ===

        [Fact]
        public void ResolveType_WithRealAssemblies_DoesNotThrow()
        {
            // Act -- exercises assembly scanning against all loaded assemblies
            // Some assemblies may throw ReflectionTypeLoadException on GetTypes()
            var result = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.SampleTestClass");

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void ResolveType_SucceedsEvenWithManyAssemblies()
        {
            // Act -- scanning all assemblies in the AppDomain should work
            // despite some assemblies potentially throwing on GetTypes()
            var result = TypeMethodResolver.ResolveType(
                "Qase.Csharp.Commons.Tests.Fixtures.AnotherTestClass");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("AnotherTestClass");
        }
    }
}
