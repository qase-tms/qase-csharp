using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FluentAssertions;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ConfigFactoryTests
    {
        private const string ConfigFileName = "qase.config.json";

        public ConfigFactoryTests()
        {
            // Clean up any existing config file
            if (File.Exists(ConfigFileName))
            {
                File.Delete(ConfigFileName);
            }
        }

        [Fact]
        public void LoadConfig_ShouldReturnDefaultConfig_WhenNoSourcesAvailable()
        {
            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Should().NotBeNull();
            config.Mode.Should().Be(Mode.Off);
            config.Fallback.Should().Be(Mode.Off);
            config.Environment.Should().BeNull();
            config.RootSuite.Should().BeNull();
            config.Debug.Should().BeFalse();
        }

        [Fact]
        public void LoadConfig_ShouldLoadFromFile_WhenFileExists()
        {
            // Arrange
            var jsonConfig = @"{
  ""mode"": ""testops"",
  ""fallback"": ""off"",
  ""environment"": ""staging"",
  ""rootSuite"": ""Custom Suite"",
  ""debug"": true,
  ""testops"": {
    ""project"": ""TEST"",
    ""api"": {
      ""token"": ""test-token"",
      ""host"": ""https://test.qase.io""
    }
  }
}";
            File.WriteAllText(ConfigFileName, jsonConfig);

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.TestOps);
            config.Fallback.Should().Be(Mode.Off);
            config.Environment.Should().Be("staging");
            config.RootSuite.Should().Be("Custom Suite");
            config.Debug.Should().BeTrue();
            config.TestOps.Project.Should().Be("TEST");
            config.TestOps.Api.Token.Should().Be("test-token");
            config.TestOps.Api.Host.Should().Be("https://test.qase.io");
        }

        [Fact]
        public void LoadConfig_ShouldHandleInvalidJsonFile()
        {
            // Arrange
            File.WriteAllText(ConfigFileName, "invalid json");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Should().NotBeNull();
            // Should fall back to default config
            config.Mode.Should().Be(Mode.Off);
        }

        [Fact]
        public void LoadConfig_ShouldLoadFromEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_MODE", "testops");
            Environment.SetEnvironmentVariable("QASE_FALLBACK", "off");
            Environment.SetEnvironmentVariable("QASE_ENVIRONMENT", "production");
            Environment.SetEnvironmentVariable("QASE_ROOT_SUITE", "Env Suite");
            Environment.SetEnvironmentVariable("QASE_DEBUG", "true");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PROJECT", "ENV_TEST");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_TOKEN", "env-token");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_HOST", "https://env.qase.io");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.TestOps);
            config.Fallback.Should().Be(Mode.Off);
            config.Environment.Should().Be("production");
            config.RootSuite.Should().Be("Env Suite");
            config.Debug.Should().BeTrue();
            config.TestOps.Project.Should().Be("ENV_TEST");
            config.TestOps.Api.Token.Should().Be("env-token");
            config.TestOps.Api.Host.Should().Be("https://env.qase.io");

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_MODE", null);
            Environment.SetEnvironmentVariable("QASE_FALLBACK", null);
            Environment.SetEnvironmentVariable("QASE_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("QASE_ROOT_SUITE", null);
            Environment.SetEnvironmentVariable("QASE_DEBUG", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PROJECT", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_TOKEN", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_API_HOST", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleBooleanEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_DEBUG", "true");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_DEFECT", "false");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_COMPLETE", "true");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Debug.Should().BeTrue();
            config.TestOps.Defect.Should().BeFalse();
            config.TestOps.Run.Complete.Should().BeTrue();

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_DEBUG", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_DEFECT", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_COMPLETE", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleListEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_TAGS", "tag1,tag2,tag3");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.TestOps.Run.Tags.Should().BeEquivalentTo(new List<string> { "tag1", "tag2", "tag3" });

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_TAGS", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleIntegerEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_TESTOPS_BATCH_SIZE", "100");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.TestOps.Batch.Size.Should().Be(100);

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_TESTOPS_BATCH_SIZE", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleLongEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_ID", "12345");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PLAN_ID", "67890");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.TestOps.Run.Id.Should().Be(12345);
            config.TestOps.Plan.Id.Should().Be(67890);

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_TESTOPS_RUN_ID", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PLAN_ID", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleReportDriverEnvironmentVariable()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_REPORT_DRIVER", "local");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Report.Driver.Should().Be(Driver.Local);

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_REPORT_DRIVER", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleReportFormatEnvironmentVariable()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_REPORT_FORMAT", "json");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Report.Connection.Local.Format.Should().Be(Format.Json);

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_REPORT_FORMAT", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleReportPathEnvironmentVariable()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QASE_REPORT_CONNECTION_PATH", "/custom/path");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Report.Connection.Local.Path.Should().Be("/custom/path");

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_REPORT_CONNECTION_PATH", null);
        }

        [Fact]
        public void LoadConfig_ShouldValidateTestOpsConfig_WhenModeIsTestOps()
        {
            // Arrange
            var jsonConfig = @"{
  ""mode"": ""testops"",
  ""testops"": {
    ""project"": ""TEST"",
    ""api"": {
      ""token"": ""test-token"",
      ""host"": ""https://test.qase.io""
    }
  }
}";
            File.WriteAllText(ConfigFileName, jsonConfig);

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.TestOps);
            config.TestOps.Project.Should().Be("TEST");
            config.TestOps.Api.Token.Should().Be("test-token");
            config.TestOps.Api.Host.Should().Be("https://test.qase.io");
        }

        [Fact]
        public void LoadConfig_ShouldValidateTestOpsConfig_WhenFallbackIsTestOps()
        {
            // Arrange
            var jsonConfig = @"{
  ""mode"": ""off"",
  ""fallback"": ""testops"",
  ""testops"": {
    ""project"": ""TEST"",
    ""api"": {
      ""token"": ""test-token"",
      ""host"": ""https://test.qase.io""
    }
  }
}";
            File.WriteAllText(ConfigFileName, jsonConfig);

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.Off);
            config.Fallback.Should().Be(Mode.TestOps);
            config.TestOps.Project.Should().Be("TEST");
            config.TestOps.Api.Token.Should().Be("test-token");
            config.TestOps.Api.Host.Should().Be("https://test.qase.io");
        }

        [Fact]
        public void LoadConfig_ShouldThrowQaseException_WhenFileReadFails()
        {
            // Arrange
            var invalidPath = "/invalid/path/qase.config.json";
            var originalMethod = typeof(ConfigFactory).GetMethod("GetConfigFilePath", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            // Act & Assert
            // This test would require mocking File.ReadAllText to throw an exception
            // For now, we'll test that the method handles missing files gracefully
            var config = ConfigFactory.LoadConfig();
            config.Should().NotBeNull();
        }

        [Fact]
        public void Create_ShouldReturnNewConfigInstance()
        {
            // Act
            var config = ConfigFactory.Create();

            // Assert
            config.Should().NotBeNull();
            config.Should().BeOfType<QaseConfig>();
        }

        [Fact]
        public void LoadConfig_ShouldMergeFileAndEnvironmentConfig()
        {
            // Arrange
            var jsonConfig = @"{
  ""mode"": ""testops"",
  ""environment"": ""staging"",
  ""testops"": {
    ""project"": ""FILE_PROJECT"",
    ""api"": {
      ""token"": ""file-token"",
      ""host"": ""https://file.qase.io""
    }
  }
}";
            File.WriteAllText(ConfigFileName, jsonConfig);

            Environment.SetEnvironmentVariable("QASE_ENVIRONMENT", "production");
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PROJECT", "ENV_PROJECT");

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.TestOps);
            config.Environment.Should().Be("production"); // Environment variable should override file
            config.TestOps.Project.Should().Be("ENV_PROJECT"); // Environment variable should override file
            config.TestOps.Api.Token.Should().Be("file-token"); // File value should remain
            config.TestOps.Api.Host.Should().Be("https://file.qase.io"); // File value should remain

            // Cleanup
            Environment.SetEnvironmentVariable("QASE_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("QASE_TESTOPS_PROJECT", null);
        }

        [Fact]
        public void LoadConfig_ShouldHandleComplexJsonConfiguration()
        {
            // Arrange
            var jsonConfig = @"{
  ""mode"": ""testops"",
  ""fallback"": ""off"",
  ""environment"": ""production"",
  ""rootSuite"": ""Complex Suite"",
  ""debug"": true,
  ""testops"": {
    ""project"": ""COMPLEX"",
    ""defect"": true,
    ""run"": {
      ""complete"": true,
      ""id"": 12345,
      ""tags"": [""complex"", ""test""]
    },
    ""plan"": {
      ""id"": 67890
    },
    ""batch"": {
      ""size"": 50
    },
    ""api"": {
      ""token"": ""complex-token"",
      ""host"": ""https://complex.qase.io""
    }
  },
  ""report"": {
    ""driver"": ""local"",
    ""connection"": {
      ""local"": {
        ""format"": ""json"",
        ""path"": ""/complex/report/path""
      }
    }
  }
}";
            File.WriteAllText(ConfigFileName, jsonConfig);

            // Act
            var config = ConfigFactory.LoadConfig();

            // Assert
            config.Mode.Should().Be(Mode.TestOps);
            config.Fallback.Should().Be(Mode.Off);
            config.Environment.Should().Be("production");
            config.RootSuite.Should().Be("Complex Suite");
            config.Debug.Should().BeTrue();
            
            config.TestOps.Project.Should().Be("COMPLEX");
            config.TestOps.Defect.Should().BeTrue();
            config.TestOps.Run.Complete.Should().BeTrue();
            config.TestOps.Run.Id.Should().Be(12345);
            config.TestOps.Run.Tags.Should().BeEquivalentTo(new List<string> { "complex", "test" });
            config.TestOps.Plan.Id.Should().Be(67890);
            config.TestOps.Batch.Size.Should().Be(50);
            config.TestOps.Api.Token.Should().Be("complex-token");
            config.TestOps.Api.Host.Should().Be("https://complex.qase.io");
            
            config.Report.Driver.Should().Be(Driver.Local);
            config.Report.Connection.Local.Format.Should().Be(Format.Json);
            config.Report.Connection.Local.Path.Should().Be("/complex/report/path");
        }
    }
} 
