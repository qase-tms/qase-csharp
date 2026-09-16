using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Moq;
using Qase.Csharp.Commons.Reporters;
using Qase.NUnit.Reporter;
using Xunit;

namespace Qase.NUnit.Reporter.Tests
{
    /// <summary>
    /// Shares the collection with XmlEventHandlingTests: both drive the listener's
    /// static state and the process-wide config cache in CoreReporterFactory.
    /// </summary>
    [Collection("Listener")]
    public class OffModeTests : IDisposable
    {
        private const string ConfigFileName = "qase.config.json";

        private readonly QaseNUnitEventListener _listener = new();
        private readonly Mock<ICoreReporter> _mockReporter = new();
        private readonly string? _savedConfig;

        public OffModeTests()
        {
            _savedConfig = File.Exists(ConfigFileName) ? File.ReadAllText(ConfigFileName) : null;
            File.Delete(ConfigFileName);
            Environment.SetEnvironmentVariable("QASE_MODE", "off");
            ResetFactory();

            SetStaticReporter(_mockReporter.Object);
        }

        public void Dispose()
        {
            SetStaticReporter(null);
            Environment.SetEnvironmentVariable("QASE_MODE", null);

            if (_savedConfig != null)
            {
                File.WriteAllText(ConfigFileName, _savedConfig);
            }

            ResetFactory();
        }

        private static void SetStaticReporter(ICoreReporter? reporter)
        {
            typeof(QaseNUnitEventListener)
                .GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, reporter);
        }

        private static void ResetFactory()
        {
            typeof(CoreReporterFactory)
                .GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Static)!
                .Invoke(null, null);
        }

        [Theory]
        [InlineData("<start-run />")]
        [InlineData("<test-run />")]
        [InlineData("<start-test id=\"1\" name=\"Test1\" fullname=\"Ns.Test1\" />")]
        [InlineData("<test-case id=\"1\" name=\"Test1\" fullname=\"Ns.Test1\" result=\"Passed\" />")]
        public void OnTestEvent_WithOffMode_ShouldNotTouchReporter(string xml)
        {
            // Act
            _listener.OnTestEvent(xml);

            // Assert
            _mockReporter.VerifyNoOtherCalls();
        }

        [Fact]
        public void OnTestEvent_WithOffMode_ShouldNotWriteLogFile()
        {
            // Arrange
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            var before = Directory.Exists(logDirectory)
                ? Directory.GetFiles(logDirectory)
                : Array.Empty<string>();

            // Act
            _listener.OnTestEvent("<start-run />");

            // Assert
            var after = Directory.Exists(logDirectory)
                ? Directory.GetFiles(logDirectory)
                : Array.Empty<string>();
            after.Should().BeEquivalentTo(before);
        }
    }
}
