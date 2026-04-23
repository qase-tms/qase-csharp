using System;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Qase.NUnit.Reporter;

namespace Qase.NUnit.Reporter.Tests
{
    public class QaseNUnitEventListenerTests
    {
        private QaseNUnitEventListener _listener;
        private Type _listenerType;

        public QaseNUnitEventListenerTests()
        {
            _listener = new QaseNUnitEventListener();
            _listenerType = typeof(QaseNUnitEventListener);
        }

        #region MapResultStatus Tests

        [Theory]
        [InlineData("Passed", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Passed)]
        [InlineData("Failed", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Failed)]
        [InlineData("Skipped", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("Inconclusive", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("Unknown", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        [InlineData("", Qase.Csharp.Commons.Models.Domain.TestResultStatus.Skipped)]
        public void MapResultStatus_ShouldMapCorrectly(string result, Qase.Csharp.Commons.Models.Domain.TestResultStatus expected)
        {
            // Arrange - MapResultStatus is still private static on QaseNUnitEventListener
            var method = _listenerType.GetMethod("MapResultStatus",
                BindingFlags.NonPublic | BindingFlags.Static);

            method.Should().NotBeNull("MapResultStatus should exist as private static");

            // Act
            var status = method!.Invoke(null, new object[] { result });

            // Assert
            status.Should().Be(expected);
        }

        #endregion
    }
}
