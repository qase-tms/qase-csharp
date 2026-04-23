using FluentAssertions;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class FailureClassifierTests
    {
        [Fact]
        public void Classify_NUnitErrorLabel_ReturnsInvalid()
        {
            var status = FailureClassifier.Classify("Error", null, null, null);
            status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void Classify_ZeroAsserts_ReturnsInvalid()
        {
            var status = FailureClassifier.Classify(null, 0, null, null);
            status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void Classify_PositiveAsserts_ReturnsFailed()
        {
            var status = FailureClassifier.Classify(null, 3, null, null);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_XUnitAssertionInStackTrace_ReturnsFailed()
        {
            var stackTrace = "   at Xunit.Assert.Equal(String expected, String actual)";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_NUnitAssertionInStackTrace_ReturnsFailed()
        {
            var stackTrace = "   at NUnit.Framework.Assert.That(Object actual, IResolveConstraint)";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_NoHints_ReturnsFailed()
        {
            var status = FailureClassifier.Classify(null, null, null, null);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_NonAssertionStackTrace_ReturnsFailed()
        {
            var stackTrace = "   at MyApp.Service.DoWork() in Service.cs:line 42";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_ErrorLabelWithPositiveAsserts_ReturnsInvalid()
        {
            var status = FailureClassifier.Classify("Error", 5, null, null);
            status.Should().Be(TestResultStatus.Invalid);
        }

        [Fact]
        public void Classify_XUnitAssertTrue_ReturnsFailed()
        {
            var stackTrace = "   at Xunit.Assert.True(Boolean condition)";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_XUnitAssertThrows_ReturnsFailed()
        {
            var stackTrace = "   at Xunit.Assert.Throws[T](Func`1 testCode)";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }

        [Fact]
        public void Classify_NUnitConstraints_ReturnsFailed()
        {
            var stackTrace = "   at NUnit.Framework.Constraints.EqualConstraint.ApplyTo[T](T actual)";
            var status = FailureClassifier.Classify(null, null, null, stackTrace);
            status.Should().Be(TestResultStatus.Failed);
        }
    }
}
