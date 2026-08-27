using System;
using System.IO;
using FluentAssertions;
using Qase.Csharp.Commons.Config;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    [Collection("Config")]
    public class ApiConnectionConfigTests : IDisposable
    {
        private const string ConfigFileName = "qase.config.json";

        private static readonly string[] EnvVars =
        {
            "QASE_TESTOPS_API_TIMEOUT",
            "QASE_TESTOPS_API_RETRIES",
            "QASE_TESTOPS_API_RETRY_BACKOFF"
        };

        public ApiConnectionConfigTests() => Cleanup();

        public void Dispose() => Cleanup();

        private static void Cleanup()
        {
            if (File.Exists(ConfigFileName))
            {
                File.Delete(ConfigFileName);
            }

            foreach (var name in EnvVars)
            {
                Environment.SetEnvironmentVariable(name, null);
            }
        }

        [Fact]
        public void ApiConfig_ShouldDefaultToThirtySecondTimeoutAndThreeRetries()
        {
            var config = new ApiConfig();

            config.Timeout.Should().Be(30);
            config.Retries.Should().Be(3);
            config.RetryBackoff.Should().Be(1);
        }

        [Fact]
        public void LoadConfig_ShouldReadConnectionSettingsFromEnvironment()
        {
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_TIMEOUT", "45");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_RETRIES", "5");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_RETRY_BACKOFF", "2.5");

            var config = ConfigFactory.LoadConfig();

            config.TestOps.Api.Timeout.Should().Be(45);
            config.TestOps.Api.Retries.Should().Be(5);
            config.TestOps.Api.RetryBackoff.Should().Be(2.5);
        }

        [Fact]
        public void LoadConfig_ShouldReadConnectionSettingsFromFile()
        {
            File.WriteAllText(ConfigFileName, @"{
  ""testops"": {
    ""api"": {
      ""timeout"": 15,
      ""retries"": 7,
      ""retryBackoff"": 0.5
    }
  }
}");

            var config = ConfigFactory.LoadConfig();

            config.TestOps.Api.Timeout.Should().Be(15);
            config.TestOps.Api.Retries.Should().Be(7);
            config.TestOps.Api.RetryBackoff.Should().Be(0.5);
        }

        [Fact]
        public void LoadConfig_ShouldLetEnvironmentOverrideTheFile()
        {
            File.WriteAllText(ConfigFileName, @"{
  ""testops"": { ""api"": { ""timeout"": 15, ""retries"": 7 } }
}");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_TIMEOUT", "45");

            var config = ConfigFactory.LoadConfig();

            config.TestOps.Api.Timeout.Should().Be(45);
            config.TestOps.Api.Retries.Should().Be(7);
        }

        [Fact]
        public void LoadConfig_ShouldKeepDefaultsWhenTheEnvironmentValueIsNotANumber()
        {
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_TIMEOUT", "soon");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_RETRY_BACKOFF", "later");

            var config = ConfigFactory.LoadConfig();

            config.TestOps.Api.Timeout.Should().Be(30);
            config.TestOps.Api.RetryBackoff.Should().Be(1);
        }
    }
}
