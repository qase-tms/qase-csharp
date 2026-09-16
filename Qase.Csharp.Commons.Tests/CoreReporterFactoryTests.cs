using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    [Collection("Config")]
    public class CoreReporterFactoryTests : IDisposable
    {
        private const string ConfigFileName = "qase.config.json";

        public CoreReporterFactoryTests()
        {
            Cleanup();
        }

        public void Dispose()
        {
            Cleanup();
        }

        private static void Cleanup()
        {
            Environment.SetEnvironmentVariable("QASE_MODE", null);

            if (File.Exists(ConfigFileName))
            {
                File.Delete(ConfigFileName);
            }

            CoreReporterFactory.Reset();
        }

        [Fact]
        public void IsReportingEnabled_ShouldBeFalse_WhenModeIsOff()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "off");

            // Act & Assert
            CoreReporterFactory.IsReportingEnabled().Should().BeFalse();
            CoreReporterFactory.GetConfig().Mode.Should().Be(Mode.Off);
        }

        [Fact]
        public void IsReportingEnabled_ShouldBeTrue_WhenModeIsReport()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "report");

            // Act & Assert
            CoreReporterFactory.IsReportingEnabled().Should().BeTrue();
            CoreReporterFactory.GetConfig().Mode.Should().Be(Mode.Report);
        }

        [Fact]
        public void GetConfig_ShouldLoadOnce_AndReturnSameInstance()
        {
            // Act
            var first = CoreReporterFactory.GetConfig();
            var second = CoreReporterFactory.GetConfig();

            // Assert
            second.Should().BeSameAs(first);
        }

        [Fact]
        public async Task GetInstance_WithOffMode_ShouldReturnSilentNoOpReporter()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "off");

            // Act
            var reporter = CoreReporterFactory.GetInstance();

            // Assert
            reporter.Should().NotBeNull();
            reporter.Should().BeOfType<CoreReporter>();
            CoreReporterFactory.GetInstance().Should().BeSameAs(reporter);

            await reporter.startTestRun();
            await reporter.addResult(new TestResult());
            await reporter.uploadResults();
            await reporter.completeTestRun();
        }

        [Fact]
        public void GetInstance_WithOffMode_ShouldNotBuildServiceProvider()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "off");

            // Act
            CoreReporterFactory.GetInstance();

            // Assert: no container means no Serilog sink and no Qase output
            ServiceProviderField().GetValue(null).Should().BeNull();
        }

        [Fact]
        public void GetInstance_WithReportMode_ShouldBuildServiceProvider()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "report");

            // Act
            CoreReporterFactory.GetInstance();

            // Assert
            ServiceProviderField().GetValue(null).Should().NotBeNull();
        }

        private static FieldInfo ServiceProviderField()
        {
            var field = typeof(CoreReporterFactory)
                .GetField("_serviceProvider", BindingFlags.NonPublic | BindingFlags.Static);
            field.Should().NotBeNull();
            return field!;
        }
    }
}
