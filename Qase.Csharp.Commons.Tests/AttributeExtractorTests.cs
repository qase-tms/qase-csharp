using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;
using Qase.Csharp.Commons.Tests.Fixtures;

namespace Qase.Csharp.Commons.Tests
{
    public class AttributeExtractorTests
    {
        /// <summary>
        /// Returns class-level Qase attributes from the given type.
        /// </summary>
        private static IEnumerable<Attribute> GetClassQaseAttributes(Type type)
        {
            return type.GetCustomAttributes(typeof(IQaseAttribute), false).Cast<Attribute>();
        }

        /// <summary>
        /// Returns method-level Qase attributes from the given type and method name.
        /// </summary>
        private static IEnumerable<Attribute> GetMethodQaseAttributes(Type type, string methodName)
        {
            return type.GetMethod(methodName)!
                .GetCustomAttributes(typeof(IQaseAttribute), false)
                .Cast<Attribute>();
        }

        // === ATTR-01: Apply extracts all five attribute types ===

        [Fact]
        public void Apply_WithQaseIds_PopulatesTestopsIds()
        {
            // Arrange
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(Enumerable.Empty<Attribute>(), methodAttrs, result);

            // Assert
            result.TestopsIds.Should().NotBeNull();
            result.TestopsIds.Should().BeEquivalentTo(new List<long> { 1, 2 });
        }

        [Fact]
        public void Apply_WithTitle_PopulatesTitle()
        {
            // Arrange
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(Enumerable.Empty<Attribute>(), methodAttrs, result);

            // Assert
            result.Title.Should().Be("Custom Title");
        }

        [Fact]
        public void Apply_WithFields_PopulatesFields()
        {
            // Arrange
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(Enumerable.Empty<Attribute>(), methodAttrs, result);

            // Assert
            result.Fields.Should().ContainKey("env");
            result.Fields["env"].Should().Be("staging");
        }

        [Fact]
        public void Apply_WithSuites_PopulatesSuiteData()
        {
            // Arrange
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(Enumerable.Empty<Attribute>(), methodAttrs, result);

            // Assert
            result.Relations!.Suite.Data.Should().HaveCount(2);
            result.Relations.Suite.Data[0].Title.Should().Be("Login");
            result.Relations.Suite.Data[1].Title.Should().Be("Auth");
        }

        [Fact]
        public void Apply_WithIgnore_SetsIgnoreTrue()
        {
            // Arrange
            var classAttrs = GetClassQaseAttributes(typeof(IgnoredClassFixture));
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, Enumerable.Empty<Attribute>(), result);

            // Assert
            result.Ignore.Should().BeTrue();
        }

        [Fact]
        public void Apply_WithAllAttributes_PopulatesAllProperties()
        {
            // Arrange -- FullyAnnotatedFixture has all five attribute types on method
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(Enumerable.Empty<Attribute>(), methodAttrs, result);

            // Assert
            result.TestopsIds.Should().BeEquivalentTo(new List<long> { 1, 2 });
            result.Title.Should().Be("Custom Title");
            result.Fields.Should().ContainKey("env");
            result.Fields["env"].Should().Be("staging");
            result.Relations!.Suite.Data.Should().HaveCount(2);
            // Note: FullyAnnotatedFixture does not have [Ignore], so Ignore stays false
            result.Ignore.Should().BeFalse();
        }

        [Fact]
        public void Apply_WithEmptyCollections_LeavesTestResultUnchanged()
        {
            // Arrange
            var result = new TestResult();
            var originalTitle = result.Title;
            var originalIgnore = result.Ignore;

            // Act
            AttributeExtractor.Apply(
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<Attribute>(),
                result);

            // Assert
            result.Title.Should().Be(originalTitle);
            result.TestopsIds.Should().BeNull();
            result.Fields.Should().BeEmpty();
            result.Relations!.Suite.Data.Should().BeEmpty();
            result.Ignore.Should().Be(originalIgnore);
        }

        [Fact]
        public void Apply_WithNullClassAttributes_DoesNotThrow()
        {
            // Arrange
            var methodAttrs = GetMethodQaseAttributes(typeof(FullyAnnotatedFixture), "AnnotatedMethod");
            var result = new TestResult();

            // Act
            var act = () => AttributeExtractor.Apply(null, methodAttrs, result);

            // Assert
            act.Should().NotThrow();
            result.TestopsIds.Should().BeEquivalentTo(new List<long> { 1, 2 });
        }

        [Fact]
        public void Apply_WithNullMethodAttributes_DoesNotThrow()
        {
            // Arrange
            var classAttrs = GetClassQaseAttributes(typeof(IgnoredClassFixture));
            var result = new TestResult();

            // Act
            var act = () => AttributeExtractor.Apply(classAttrs, null, result);

            // Assert
            act.Should().NotThrow();
            result.Ignore.Should().BeTrue();
        }

        [Fact]
        public void Apply_WithBothNull_DoesNotThrow()
        {
            // Arrange
            var result = new TestResult();

            // Act
            var act = () => AttributeExtractor.Apply(null, null, result);

            // Assert
            act.Should().NotThrow();
            result.TestopsIds.Should().BeNull();
            result.Fields.Should().BeEmpty();
            result.Ignore.Should().BeFalse();
        }

        // === ATTR-02: Indexer semantics for Fields (duplicate key handling) ===

        [Fact]
        public void Apply_WithDuplicateFieldKeys_MethodOverwritesClass()
        {
            // Arrange
            var classAttrs = GetClassQaseAttributes(typeof(ClassAndMethodAttributeFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(ClassAndMethodAttributeFixture), "MethodWithOverrides");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert -- method-level "env"="production" overwrites class-level "env"="staging"
            result.Fields["env"].Should().Be("production");
        }

        [Fact]
        public void Apply_WithDistinctFieldKeys_BothPreserved()
        {
            // Arrange
            var classAttrs = GetClassQaseAttributes(typeof(ClassAndMethodAttributeFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(ClassAndMethodAttributeFixture), "MethodWithOverrides");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert -- class-level "priority" and method-level "browser" both present
            result.Fields.Should().ContainKey("priority");
            result.Fields["priority"].Should().Be("high");
            result.Fields.Should().ContainKey("browser");
            result.Fields["browser"].Should().Be("chrome");
        }

        [Fact]
        public void Apply_WithDuplicateFieldKeys_DoesNotThrow()
        {
            // Arrange -- class has [Fields("env", "staging")], method has [Fields("env", "production")]
            var classAttrs = GetClassQaseAttributes(typeof(ClassAndMethodAttributeFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(ClassAndMethodAttributeFixture), "MethodWithOverrides");
            var result = new TestResult();

            // Act -- must NOT throw ArgumentException (this is the Fields.Add bug fix)
            var act = () => AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert
            act.Should().NotThrow();
        }

        // === ATTR-03: Class-first, method-second ordering ===

        [Fact]
        public void Apply_MethodSuitesOverwriteClassSuites()
        {
            // Arrange
            var classAttrs = GetClassQaseAttributes(typeof(ClassAndMethodAttributeFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(ClassAndMethodAttributeFixture), "MethodWithOverrides");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert -- method-level [Suites("MethodSuite")] overwrites class-level [Suites("ClassSuite")]
            result.Relations!.Suite.Data.Should().HaveCount(1);
            result.Relations.Suite.Data[0].Title.Should().Be("MethodSuite");
        }

        [Fact]
        public void Apply_ClassIgnorePreservedWhenNoMethodIgnore()
        {
            // Arrange -- IgnoredClassFixture has [Ignore] on class, TestMethod has no attributes
            var classAttrs = GetClassQaseAttributes(typeof(IgnoredClassFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(IgnoredClassFixture), "TestMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert -- class-level [Ignore] is preserved
            result.Ignore.Should().BeTrue();
        }

        [Fact]
        public void Apply_ClassAttributesProcessedFirst()
        {
            // Arrange -- PlainMethod has no method-level attributes
            var classAttrs = GetClassQaseAttributes(typeof(ClassAndMethodAttributeFixture));
            var methodAttrs = GetMethodQaseAttributes(typeof(ClassAndMethodAttributeFixture), "PlainMethod");
            var result = new TestResult();

            // Act
            AttributeExtractor.Apply(classAttrs, methodAttrs, result);

            // Assert -- class-level attributes applied: Fields, Suites, Ignore
            result.Fields.Should().ContainKey("env");
            result.Fields["env"].Should().Be("staging");
            result.Fields.Should().ContainKey("priority");
            result.Fields["priority"].Should().Be("high");
            result.Relations!.Suite.Data.Should().HaveCount(1);
            result.Relations.Suite.Data[0].Title.Should().Be("ClassSuite");
            result.Ignore.Should().BeTrue();
        }
    }
}
