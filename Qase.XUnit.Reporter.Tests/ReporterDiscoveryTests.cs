using System;
using FluentAssertions;
using Qase.Xunit.Reporter;
using Xunit;

namespace Qase.XUnit.Reporter.Tests
{
    /// <summary>
    /// xUnit v2 discovers third-party runner reporters by scanning the test output folder with
    /// <c>Directory.GetFiles(folder, "*reporters*.dll")</c> (see RunnerReporterUtility in
    /// xunit.runner.utility) and then loading each match via
    /// <c>Assembly.Load(new AssemblyName(fileNameWithoutExtension))</c>.
    ///
    /// That glob matches case-insensitively on Windows and macOS but case-sensitively on Linux,
    /// so an assembly named "Qase.XUnit.Reporters" is found on a developer machine and silently
    /// missed on a Linux CI runner, leaving results unreported.
    ///
    /// The assembly name drives the output file name, so it is asserted here rather than the file
    /// on disk: a case-insensitive filesystem keeps the previous file name after a rename and
    /// would report a false pass.
    /// </summary>
    public class ReporterDiscoveryTests
    {
        [Fact]
        public void AssemblyName_ContainsLowercaseReporters_SoLinuxDiscoveryFindsIt()
        {
            var assemblyName = typeof(QaseRunnerReporter).Assembly.GetName().Name;

            assemblyName.Should().NotBeNull();
            assemblyName!.Contains("reporters", StringComparison.Ordinal).Should().BeTrue(
                "xUnit v2 scans for '*reporters*.dll' case-sensitively on Linux, " +
                $"so the assembly name must contain lowercase 'reporters', but was '{assemblyName}'");
        }

        [Fact]
        public void ReporterIsEnvironmentallyEnabled_SoNoRunnerSwitchIsRequired()
        {
            var reporter = new QaseRunnerReporter();

            reporter.IsEnvironmentallyEnabled.Should().BeTrue();
            reporter.RunnerSwitch.Should().Be("qase");
        }
    }
}
