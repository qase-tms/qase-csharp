using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Qase.Csharp.Commons.Attributes;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class AttributesTests
    {
        [Fact]
        public void QaseIdsAttribute_Constructor_ShouldSetIds()
        {
            // Arrange
            var ids = new long[] { 1, 2, 3 };

            // Act
            var attribute = new QaseIdsAttribute(ids);

            // Assert
            attribute.Ids.Should().HaveCount(3);
            attribute.Ids.Should().Contain(1);
            attribute.Ids.Should().Contain(2);
            attribute.Ids.Should().Contain(3);
        }

        [Fact]
        public void QaseIdsAttribute_Constructor_WithNoIds_ShouldCreateEmptyList()
        {
            // Act
            var attribute = new QaseIdsAttribute();

            // Assert
            attribute.Ids.Should().BeEmpty();
        }

        [Fact]
        public void QaseIdsAttribute_ShouldImplementIQaseAttribute()
        {
            // Act
            var attribute = new QaseIdsAttribute(1, 2, 3);

            // Assert
            attribute.Should().BeAssignableTo<IQaseAttribute>();
        }

        [Fact]
        public void TitleAttribute_Constructor_ShouldSetTitle()
        {
            // Arrange
            var title = "Test Title";

            // Act
            var attribute = new TitleAttribute(title);

            // Assert
            attribute.Title.Should().Be(title);
        }

        [Fact]
        public void TitleAttribute_ShouldImplementIQaseAttribute()
        {
            // Act
            var attribute = new TitleAttribute("Test Title");

            // Assert
            attribute.Should().BeAssignableTo<IQaseAttribute>();
        }

        [Fact]
        public void TitleAttribute_WithNullTitle_ShouldSetNullTitle()
        {
            // Act
            var attribute = new TitleAttribute(null!);

            // Assert
            attribute.Title.Should().BeNull();
        }

        [Fact]
        public void TitleAttribute_WithEmptyTitle_ShouldSetEmptyTitle()
        {
            // Act
            var attribute = new TitleAttribute("");

            // Assert
            attribute.Title.Should().Be("");
        }

        [Fact]
        public void FieldsAttribute_ShouldHaveCorrectAttributeUsage()
        {
            // Arrange & Act
            var attributeUsage = typeof(FieldsAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage!.ValidOn.Should().Be(AttributeTargets.Method | AttributeTargets.Class);
            attributeUsage.AllowMultiple.Should().BeTrue();
        }

        [Fact]
        public void FieldsAttribute_ShouldImplementIQaseAttribute()
        {
            // Arrange & Act
            var interfaces = typeof(FieldsAttribute).GetInterfaces();

            // Assert
            interfaces.Should().Contain(typeof(IQaseAttribute));
        }

        [Fact]
        public void FieldsAttribute_Constructor_ShouldSetProperties()
        {
            // Arrange
            var key = "test_key";
            var value = "test_value";

            // Act
            var attribute = new FieldsAttribute(key, value);

            // Assert
            attribute.Key.Should().Be(key);
            attribute.Value.Should().Be(value);
        }

        [Fact]
        public void FieldsAttribute_Constructor_ShouldHandleNullValues()
        {
            // Arrange
            string? key = null;
            string? value = null;

            // Act
            var attribute = new FieldsAttribute(key!, value!);

            // Assert
            attribute.Key.Should().BeNull();
            attribute.Value.Should().BeNull();
        }

        [Fact]
        public void FieldsAttribute_Constructor_ShouldHandleEmptyStrings()
        {
            // Arrange
            var key = "";
            var value = "";

            // Act
            var attribute = new FieldsAttribute(key, value);

            // Assert
            attribute.Key.Should().Be("");
            attribute.Value.Should().Be("");
        }

        [Fact]
        public void FieldsAttribute_Constructor_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var key = "key with spaces and symbols: @#$%";
            var value = "value with \"quotes\" and 'apostrophes'";

            // Act
            var attribute = new FieldsAttribute(key, value);

            // Assert
            attribute.Key.Should().Be(key);
            attribute.Value.Should().Be(value);
        }

        [Fact]
        public void SuitesAttribute_ShouldHaveCorrectAttributeUsage()
        {
            // Arrange & Act
            var attributeUsage = typeof(SuitesAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage!.ValidOn.Should().Be(AttributeTargets.Method | AttributeTargets.Class);
            attributeUsage.AllowMultiple.Should().BeFalse();
        }

        [Fact]
        public void SuitesAttribute_ShouldImplementIQaseAttribute()
        {
            // Arrange & Act
            var interfaces = typeof(SuitesAttribute).GetInterfaces();

            // Assert
            interfaces.Should().Contain(typeof(IQaseAttribute));
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldSetProperties()
        {
            // Arrange
            var suites = new[] { "suite1", "suite2", "suite3" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleEmptyArray()
        {
            // Arrange
            var suites = new string[0];

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().BeEmpty();
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleNullArray()
        {
            // Arrange
            string[]? suites = null;

            // Act
            var attribute = new SuitesAttribute(suites!);

            // Assert
            attribute.Suites.Should().BeEmpty();
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleSuite()
        {
            // Arrange
            var suite = "single_suite";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleMultipleSuites()
        {
            // Arrange
            var suites = new[] { "suite1", "suite2", "suite3", "suite4" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleNullValuesInArray()
        {
            // Arrange
            var suites = new[] { "suite1", null, "suite3" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(3);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleEmptyStringsInArray()
        {
            // Arrange
            var suites = new[] { "suite1", "", "suite3" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(3);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var suites = new[] { "suite with spaces", "suite-with-dashes", "suite_with_underscores", "suite.with.dots" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleUnicodeCharacters()
        {
            // Arrange
            var suites = new[] { "suite_русский", "suite_中文", "suite_日本語", "suite_한국어" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleVeryLongSuiteNames()
        {
            // Arrange
            var longSuite = new string('a', 1000);
            var suites = new[] { longSuite };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(longSuite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleMixedCase()
        {
            // Arrange
            var suites = new[] { "Suite1", "SUITE2", "suite3", "Suite_4" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleDuplicateSuites()
        {
            // Arrange
            var suites = new[] { "suite1", "suite1", "suite2", "suite2" };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleWhitespaceOnly()
        {
            // Arrange
            var suites = new[] { "   ", "\t", "\n", "  \t  " };

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(4);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleVeryLargeArray()
        {
            // Arrange
            var suites = Enumerable.Range(1, 1000).Select(i => $"suite{i}").ToArray();

            // Act
            var attribute = new SuitesAttribute(suites);

            // Assert
            attribute.Suites.Should().HaveCount(1000);
            attribute.Suites.Should().BeEquivalentTo(suites);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleParamsKeyword()
        {
            // Arrange
            var suite1 = "suite1";
            var suite2 = "suite2";
            var suite3 = "suite3";

            // Act
            var attribute = new SuitesAttribute(suite1, suite2, suite3);

            // Assert
            attribute.Suites.Should().HaveCount(3);
            attribute.Suites.Should().BeEquivalentTo(new[] { suite1, suite2, suite3 });
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleNoParameters()
        {
            // Act
            var attribute = new SuitesAttribute();

            // Assert
            attribute.Suites.Should().BeEmpty();
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleNullParameter()
        {
            // Arrange
            string? suite = null;

            // Act
            var attribute = new SuitesAttribute(suite!);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().BeNull();
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleEmptyParameter()
        {
            // Arrange
            var suite = "";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be("");
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleWhitespaceParameter()
        {
            // Arrange
            var suite = "   ";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be("   ");
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleSpecialCharacterParameter()
        {
            // Arrange
            var suite = "suite@#$%^&*()";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleUnicodeParameter()
        {
            // Arrange
            var suite = "suite_русский_中文_日本語_한국어";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleVeryLongParameter()
        {
            // Arrange
            var suite = new string('a', 10000);

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleMixedCaseParameter()
        {
            // Arrange
            var suite = "Suite_With_Mixed_Case_123";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleNumberParameter()
        {
            // Arrange
            var suite = "123";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleSymbolParameter()
        {
            // Arrange
            var suite = "@#$%^&*()";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }

        [Fact]
        public void SuitesAttribute_Constructor_ShouldHandleSingleEmojiParameter()
        {
            // Arrange
            var suite = "suite_😀🎉🚀";

            // Act
            var attribute = new SuitesAttribute(suite);

            // Assert
            attribute.Suites.Should().HaveCount(1);
            attribute.Suites.First().Should().Be(suite);
        }
    }
} 
