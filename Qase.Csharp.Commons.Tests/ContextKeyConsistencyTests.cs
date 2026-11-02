using System;
using System.Collections.Generic;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    [Collection("ContextManager")]
    public class ContextKeyConsistencyTests
    {
        public enum Color { Red }

        public class Fixture
        {
            public void Search(string query, int expectedMinResults) { }
            public void Plain() { }
            public void Other() { }
        }

        [Theory]
        [InlineData("29.99d", "29.99")]
        [InlineData("10m", "10")]
        [InlineData("5L", "5")]
        [InlineData("\"usb hub\"", "usb hub")]
        [InlineData("'a'", "a")]
        [InlineData("", "empty")]
        [InlineData("\"\"", "empty")]
        [InlineData(null, "null")]
        [InlineData("plain", "plain")]
        [InlineData("29.989999999999998", "29.99")]
        [InlineData("9.9900000000000002", "9.99")]
        public void NormalizeValue_ShouldStripFrameworkDecorations(string? input, string expected)
        {
            DisplayNameGenerator.NormalizeValue(input).Should().Be(expected);
        }

        [Fact]
        public void FormatArgument_ShouldMatchNormalizedDisplayValues()
        {
            DisplayNameGenerator.FormatArgument(29.99m).Should().Be(DisplayNameGenerator.NormalizeValue("29.99m"));
            DisplayNameGenerator.FormatArgument(29.99d).Should().Be(DisplayNameGenerator.NormalizeValue("29.99d"));
            DisplayNameGenerator.FormatArgument(Color.Red).Should().Be("Red");
            DisplayNameGenerator.FormatArgument("").Should().Be("empty");
            DisplayNameGenerator.FormatArgument(null).Should().Be("null");
        }

        [Fact]
        public void ParseAndMap_ShouldStripParameterNames_FromXunitV3DisplayName()
        {
            var method = typeof(Fixture).GetMethod(nameof(Fixture.Search))!;

            var result = ParameterParser.ParseAndMap("Ns.Fixture.Search(query: \"usb hub\", expectedMinResults: 8)", method);

            result.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                ["query"] = "usb hub",
                ["expectedMinResults"] = "8"
            });
        }

        [Fact]
        public void ParseAndMap_ShouldMapEmptyNamedValue_ToEmpty()
        {
            var method = typeof(Fixture).GetMethod(nameof(Fixture.Search))!;

            var result = ParameterParser.ParseAndMap("Search(query: \"\", expectedMinResults: 0)", method);

            result["query"].Should().Be("empty");
        }

        [Fact]
        public void ParseAndMap_ShouldKeepPositionalValues()
        {
            var method = typeof(Fixture).GetMethod(nameof(Fixture.Search))!;

            var result = ParameterParser.ParseAndMap("Search (usb hub,8)", method);

            result["query"].Should().Be("usb hub");
        }

        [Fact]
        public void Build_ShouldUseMethodName_WhenDisplayNameIsFullyQualified()
        {
            var typeName = typeof(Fixture).FullName!;
            var raw = new RawTestData
            {
                DisplayName = $"{typeName}.{nameof(Fixture.Plain)}",
                FullTypeName = typeName,
                MethodName = nameof(Fixture.Plain),
                Status = TestResultStatus.Passed
            };

            var result = new TestResultBuilder().Build(raw);

            result.Title.Should().Be(nameof(Fixture.Plain));
        }

        [Fact]
        public void Build_ShouldGiveDistinctSignatures_ToTestsOfTheSameClass()
        {
            var typeName = typeof(Fixture).FullName!;
            RawTestData Raw(string method) => new RawTestData
            {
                DisplayName = method,
                FullTypeName = typeName,
                MethodName = method,
                Status = TestResultStatus.Passed
            };

            var first = new TestResultBuilder().Build(Raw(nameof(Fixture.Plain)));
            var second = new TestResultBuilder().Build(Raw(nameof(Fixture.Other)));

            first.Signature.Should().NotBe(second.Signature);
            first.Signature.Should().EndWith("::plain");
        }

        [Fact]
        public void Build_ShouldCollectContextData_ForXunitV3Theory()
        {
            var typeName = typeof(Fixture).FullName!;
            // The key QaseAspect builds from the runtime arguments.
            var key = DisplayNameGenerator.Generate($"{typeName}.{nameof(Fixture.Search)}", new Dictionary<string, string>
            {
                ["query"] = DisplayNameGenerator.FormatArgument("usb hub"),
                ["expectedMinResults"] = DisplayNameGenerator.FormatArgument(8)
            });
            ContextManager.BeginTestCase(key);
            ContextManager.AddComment("from the test body");

            var result = new TestResultBuilder().Build(new RawTestData
            {
                DisplayName = $"{typeName}.{nameof(Fixture.Search)}(query: \"usb hub\", expectedMinResults: 8)",
                FullTypeName = typeName,
                MethodName = nameof(Fixture.Search),
                Status = TestResultStatus.Passed
            });

            result.Params["query"].Should().Be("usb hub");
            result.Message.Should().Be("from the test body");
        }

        [Fact]
        public void GetComments_ShouldNotLeakIntoTheNextResultWithTheSameKey()
        {
            var key = $"Leak.Test.{Guid.NewGuid():N}";
            ContextManager.BeginTestCase(key);
            ContextManager.AddComment("first attempt");
            ContextManager.AddAttachment(new byte[] { 1 }, "a.bin");

            ContextManager.GetComments(key).Should().Be("first attempt");
            ContextManager.GetAttachments(key).Should().HaveCount(1);

            ContextManager.GetComments(key).Should().BeEmpty();
            ContextManager.GetAttachments(key).Should().BeEmpty();
        }

        [Fact]
        public void BeginTestCase_ShouldDropDataOfAnEarlierAttempt()
        {
            var key = $"Retry.Test.{Guid.NewGuid():N}";
            ContextManager.BeginTestCase(key);
            ContextManager.StartStep("attempt 1");
            ContextManager.FailStep();

            ContextManager.BeginTestCase(key);
            ContextManager.StartStep("attempt 2");
            ContextManager.PassStep();

            var steps = ContextManager.GetCompletedSteps(key);
            steps.Should().ContainSingle().Which.Data!.Action.Should().Be("attempt 2");
        }
    }
}
