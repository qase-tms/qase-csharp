using System;
using FluentAssertions;
using Qase.Csharp.Commons.Config;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class QaseConfigTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new QaseConfig();

            // Assert
            config.Mode.Should().Be(Mode.Off);
            config.Fallback.Should().Be(Mode.Off);
            config.Environment.Should().BeNull();
            config.RootSuite.Should().BeNull();
            config.Debug.Should().BeFalse();
            config.TestOps.Should().NotBeNull();
            config.Report.Should().NotBeNull();
            config.Logging.Should().NotBeNull();
            config.Logging.Console.Should().BeTrue();
            config.Logging.File.Should().BeFalse();
        }

        [Theory]
        [InlineData(null, Mode.Off)]
        [InlineData("", Mode.Off)]
        [InlineData("off", Mode.Off)]
        [InlineData("testops", Mode.TestOps)]
        [InlineData("report", Mode.Report)]
        public void SetMode_WithValidValues_ShouldSetMode(string? mode, Mode expectedMode)
        {
            // Arrange
            var config = new QaseConfig();

            // Act
            config.SetMode(mode);

            // Assert
            config.Mode.Should().Be(expectedMode);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("unknown")]
        public void SetMode_WithInvalidValues_ShouldThrowArgumentException(string? mode)
        {
            // Arrange
            var config = new QaseConfig();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => config.SetMode(mode));
        }

        [Theory]
        [InlineData(null, Mode.Off)]
        [InlineData("", Mode.Off)]
        [InlineData("off", Mode.Off)]
        [InlineData("testops", Mode.TestOps)]
        [InlineData("report", Mode.Report)]
        public void SetFallback_WithValidValues_ShouldSetFallback(string? fallback, Mode expectedFallback)
        {
            // Arrange
            var config = new QaseConfig();

            // Act
            config.SetFallback(fallback);

            // Assert
            config.Fallback.Should().Be(expectedFallback);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("unknown")]
        public void SetFallback_WithInvalidValues_ShouldThrowArgumentException(string? fallback)
        {
            // Arrange
            var config = new QaseConfig();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => config.SetFallback(fallback));
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            // Arrange
            var config = new QaseConfig();
            var environment = "staging";
            var rootSuite = "Test Suite";
            var debug = true;

            // Act
            config.Environment = environment;
            config.RootSuite = rootSuite;
            config.Debug = debug;
            config.Mode = Mode.TestOps;
            config.Fallback = Mode.Report;

            // Assert
            config.Environment.Should().Be(environment);
            config.RootSuite.Should().Be(rootSuite);
            config.Debug.Should().Be(debug);
            config.Mode.Should().Be(Mode.TestOps);
            config.Fallback.Should().Be(Mode.Report);
        }

        [Fact]
        public void LoggingProperties_ShouldBeSettable()
        {
            // Arrange
            var config = new QaseConfig();
            var console = false;
            var file = true;

            // Act
            config.Logging.Console = console;
            config.Logging.File = file;

            // Assert
            config.Logging.Console.Should().Be(console);
            config.Logging.File.Should().Be(file);
        }

        [Fact]
        public void TestOpsConfig_ShowPublicReportLink_ShouldHaveDefaultValue()
        {
            // Act
            var config = new QaseConfig();

            // Assert
            config.TestOps.Should().NotBeNull();
            config.TestOps.ShowPublicReportLink.Should().BeFalse();
        }

        [Fact]
        public void TestOpsConfig_ShowPublicReportLink_ShouldBeSettable()
        {
            // Arrange
            var config = new QaseConfig();

            // Act
            config.TestOps.ShowPublicReportLink = true;

            // Assert
            config.TestOps.ShowPublicReportLink.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOpsConfig_ShowPublicReportLink_ShouldAcceptBooleanValues(bool value)
        {
            // Arrange
            var config = new QaseConfig();

            // Act
            config.TestOps.ShowPublicReportLink = value;

            // Assert
            config.TestOps.ShowPublicReportLink.Should().Be(value);
        }
    }
} 
