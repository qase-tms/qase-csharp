using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Reqnroll.Configuration;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;
using Qase.Csharp.Commons.Reporters;
using Qase.Reqnroll.Reporter;
using Xunit;

namespace Qase.Reqnroll.Reporter.Tests
{
    [Collection("Config")]
    public class PluginRegistrationTests : IDisposable
    {
        private const string ConfigFileName = "qase.config.json";

        private readonly string? _savedConfig;

        public PluginRegistrationTests()
        {
            _savedConfig = File.Exists(ConfigFileName) ? File.ReadAllText(ConfigFileName) : null;
            File.Delete(ConfigFileName);
            ResetFactory();
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("QASE_MODE", null);

            if (_savedConfig != null)
            {
                File.WriteAllText(ConfigFileName, _savedConfig);
            }

            ResetFactory();
        }

        private static void ResetFactory()
        {
            typeof(CoreReporterFactory)
                .GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Static)!
                .Invoke(null, null);
        }

        private static ReqnrollConfiguration RaiseConfigurationDefaults()
        {
            var events = new RuntimePluginEvents();
            new QaseReqnrollPlugin().Initialize(
                events,
                new RuntimePluginParameters(),
                new UnitTestProviderConfiguration());

            var configuration = ConfigurationLoader.GetDefault();
            events.RaiseConfigurationDefaults(configuration);
            return configuration;
        }

        [Fact]
        public void ConfigurationDefaults_WithOffMode_ShouldNotRegisterBindingAssembly()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "off");

            // Act
            var configuration = RaiseConfigurationDefaults();

            // Assert: without the assembly registered, none of the Qase hooks run
            configuration.AdditionalStepAssemblies
                .Should().NotContain(Assembly.GetAssembly(typeof(QaseReqnrollPlugin))!.FullName);
        }

        [Fact]
        public void ConfigurationDefaults_WithReportMode_ShouldRegisterBindingAssembly()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "report");

            // Act
            var configuration = RaiseConfigurationDefaults();

            // Assert
            configuration.AdditionalStepAssemblies
                .Should().Contain(Assembly.GetAssembly(typeof(QaseReqnrollPlugin))!.FullName);
        }
    }
}
