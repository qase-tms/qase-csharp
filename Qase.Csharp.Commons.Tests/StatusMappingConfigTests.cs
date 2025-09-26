using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Config;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    /// <summary>
    /// Tests for status mapping configuration
    /// </summary>
    public class StatusMappingConfigTests
    {
        private readonly ILogger _logger = NullLogger.Instance;

        [Fact]
        public void QaseConfig_StatusMapping_InitializedAsEmpty()
        {
            // Act
            var config = new QaseConfig();

            // Assert
            Assert.NotNull(config.StatusMapping);
            Assert.Empty(config.StatusMapping);
        }

        [Fact]
        public void QaseConfig_StatusMapping_CanBeSet()
        {
            // Arrange
            var config = new QaseConfig();
            var expectedMapping = new Dictionary<string, string>
            {
                ["invalid"] = "failed",
                ["skipped"] = "passed"
            };

            // Act
            config.StatusMapping = expectedMapping;

            // Assert
            Assert.Equal(expectedMapping, config.StatusMapping);
        }

        [Fact]
        public void ConfigFactory_LoadFromEnv_StatusMapping_ValidMapping()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", "invalid=failed,skipped=passed");

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Equal(2, config.StatusMapping.Count);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
                Assert.Equal("passed", config.StatusMapping["skipped"]);
            }
            finally
            {
                Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", null);
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromEnv_StatusMapping_EmptyMapping()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", "");

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Empty(config.StatusMapping);
            }
            finally
            {
                Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", null);
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromEnv_StatusMapping_InvalidMapping()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", "invalid=failed,unknown=passed");

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Single(config.StatusMapping);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
            }
            finally
            {
                Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", null);
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_ValidMapping()
        {
            // Arrange
            var jsonConfig = @"{
                ""statusMapping"": {
                    ""invalid"": ""failed"",
                    ""skipped"": ""passed""
                }
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Equal(2, config.StatusMapping.Count);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
                Assert.Equal("passed", config.StatusMapping["skipped"]);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_EmptyMapping()
        {
            // Arrange
            var jsonConfig = @"{
                ""statusMapping"": {}
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Empty(config.StatusMapping);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_InvalidMapping()
        {
            // Arrange
            var jsonConfig = @"{
                ""statusMapping"": {
                    ""invalid"": ""failed"",
                    ""unknown"": ""passed""
                }
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Single(config.StatusMapping);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_MissingProperty()
        {
            // Arrange
            var jsonConfig = @"{
                ""mode"": ""testops""
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Empty(config.StatusMapping);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_CombinedWithEnv()
        {
            // Arrange
            var jsonConfig = @"{
                ""statusMapping"": {
                    ""invalid"": ""failed""
                }
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);
            Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", "skipped=passed");

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Equal(2, config.StatusMapping.Count);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
                Assert.Equal("passed", config.StatusMapping["skipped"]);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
                Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", null);
            }
        }

        [Fact]
        public void ConfigFactory_LoadFromJson_StatusMapping_EnvOverridesJson()
        {
            // Arrange
            var jsonConfig = @"{
                ""statusMapping"": {
                    ""invalid"": ""skipped""
                }
            }";
            var configPath = "qase.config.json";
            File.WriteAllText(configPath, jsonConfig);
            Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", "invalid=failed");

            try
            {
                // Act
                var config = ConfigFactory.LoadConfig();

                // Assert
                Assert.Single(config.StatusMapping);
                Assert.Equal("failed", config.StatusMapping["invalid"]);
            }
            finally
            {
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
                Environment.SetEnvironmentVariable("QASE_STATUS_MAPPING", null);
            }
        }
    }
}
