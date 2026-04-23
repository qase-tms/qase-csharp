using FluentAssertions;
using Qase.Csharp.Commons.Utils;
using Xunit;
using System.Collections.Generic;

namespace Qase.Csharp.Commons.Tests
{
    public class DisplayNameGeneratorTests
    {
        [Fact]
        public void Generate_WithBaseNameOnly_ReturnsBaseName()
        {
            var result = DisplayNameGenerator.Generate("MyNamespace.MyClass.MyTest", null);
            result.Should().Be("MyNamespace.MyClass.MyTest");
        }

        [Fact]
        public void Generate_WithEmptyParams_ReturnsBaseName()
        {
            var result = DisplayNameGenerator.Generate("MyNamespace.MyClass.MyTest",
                new Dictionary<string, string>());
            result.Should().Be("MyNamespace.MyClass.MyTest");
        }

        [Fact]
        public void Generate_WithParams_ReturnsBaseNameWithParams()
        {
            var parameters = new Dictionary<string, string>
            {
                { "user", "user1" },
                { "value", "42" }
            };
            var result = DisplayNameGenerator.Generate("MyNamespace.MyClass.MyTest", parameters);
            result.Should().Be("MyNamespace.MyClass.MyTest(user: user1, value: 42)");
        }

        [Fact]
        public void Generate_WithSingleParam_ReturnsCorrectFormat()
        {
            var parameters = new Dictionary<string, string>
            {
                { "name", "test" }
            };
            var result = DisplayNameGenerator.Generate("ScenarioTitle", parameters);
            result.Should().Be("ScenarioTitle(name: test)");
        }
    }
}
