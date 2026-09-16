using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using Qase.Csharp.Commons.Reporters;
using Qase.Xunit.Reporter;
using Xunit;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter.Tests
{
    /// <summary>
    /// Shares the collection with EventHandlingTests: both construct a sink, which
    /// reads the process-wide config cache in CoreReporterFactory.
    /// </summary>
    [Collection("Sink")]
    public class OffModeTests : IDisposable
    {
        private const string ConfigFileName = "qase.config.json";

        private readonly string? _savedConfig;

        public OffModeTests()
        {
            _savedConfig = File.Exists(ConfigFileName) ? File.ReadAllText(ConfigFileName) : null;
            File.Delete(ConfigFileName);
            Environment.SetEnvironmentVariable("QASE_MODE", "off");
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

        [Fact]
        public void Constructor_WithOffMode_ShouldNotBuildReporter()
        {
            // Act
            var sink = new QaseMessageSink(new Mock<IRunnerLogger>().Object);

            // Assert
            var reporterField = typeof(QaseMessageSink)
                .GetField("_reporter", BindingFlags.NonPublic | BindingFlags.Instance);
            reporterField!.GetValue(sink).Should().BeNull();
        }

        [Theory]
        [InlineData("TestStartingEvent")]
        [InlineData("TestFailedEvent")]
        [InlineData("TestPassedEvent")]
        [InlineData("TestSkippedEvent")]
        [InlineData("TestFinishedEvent")]
        public void Constructor_WithOffMode_ShouldNotSubscribeToExecutionEvents(string eventName)
        {
            // Act
            var sink = new QaseMessageSink(new Mock<IRunnerLogger>().Object);

            // Assert
            QaseHandlersOf(sink.Execution, eventName).Should().BeEmpty();
        }

        [Theory]
        [InlineData("TestAssemblyExecutionStartingEvent")]
        [InlineData("TestAssemblyExecutionFinishedEvent")]
        public void Constructor_WithOffMode_ShouldNotSubscribeToRunnerEvents(string eventName)
        {
            // Act
            var sink = new QaseMessageSink(new Mock<IRunnerLogger>().Object);

            // Assert: the base reporter keeps its own handlers, only ours must be absent
            QaseHandlersOf(sink.Runner, eventName).Should().BeEmpty();
        }

        /// <summary>
        /// Returns the handlers declared by QaseMessageSink itself on a field-like
        /// event, read through its backing field: an event cannot be inspected from
        /// outside the declaring type.
        /// </summary>
        private static Delegate[] QaseHandlersOf(object target, string eventName)
        {
            var field = target.GetType()
                .GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.Should().NotBeNull($"event '{eventName}' should exist on {target.GetType().Name}");

            var handlers = (Delegate?)field!.GetValue(target);
            if (handlers == null)
            {
                return Array.Empty<Delegate>();
            }

            return handlers.GetInvocationList()
                .Where(h => h.Method.DeclaringType == typeof(QaseMessageSink))
                .ToArray();
        }
    }
}
