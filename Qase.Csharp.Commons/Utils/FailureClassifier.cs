#nullable enable

using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Classifies test failures as assertion failures (Failed) or runtime errors (Invalid).
    /// Consolidates logic from NUnit DetermineFailureType and xUnit IsAssertionFailure.
    /// </summary>
    public static class FailureClassifier
    {
        /// <summary>
        /// Determines whether a test failure is an assertion failure (Failed)
        /// or a runtime error (Invalid).
        /// </summary>
        public static TestResultStatus Classify(
            string? failureLabel,
            int? assertsCount,
            string[]? exceptionTypes,
            string? stackTrace)
        {
            // Priority 1: explicit "Error" label → Invalid (NUnit)
            if (!string.IsNullOrEmpty(failureLabel) && failureLabel == "Error")
                return TestResultStatus.Invalid;

            // Priority 2: assertsCount == 0 → Invalid (NUnit)
            if (assertsCount.HasValue && assertsCount.Value == 0)
                return TestResultStatus.Invalid;

            // Priority 3: assertsCount > 0 → Failed (NUnit)
            if (assertsCount.HasValue && assertsCount.Value > 0)
                return TestResultStatus.Failed;

            // Priority 4: check stack trace for assertion framework patterns
            if (!string.IsNullOrEmpty(stackTrace) && ContainsAssertionPattern(stackTrace))
                return TestResultStatus.Failed;

            // Default: Failed (safer than Invalid — assume assertion failure)
            return TestResultStatus.Failed;
        }

        private static bool ContainsAssertionPattern(string stackTrace)
        {
            // NUnit assertion patterns
            if (stackTrace.Contains("at NUnit.Framework.Assert") ||
                stackTrace.Contains("at NUnit.Framework.Constraints"))
                return true;

            // xUnit assertion patterns
            if (stackTrace.Contains("at Xunit.Assert."))
                return true;

            // Generic assertion patterns (FluentAssertions, etc.)
            if (stackTrace.Contains("Assert.That") ||
                stackTrace.Contains("Assert.AreEqual") ||
                stackTrace.Contains("Assert.IsTrue") ||
                stackTrace.Contains("Assert.IsFalse") ||
                stackTrace.Contains("Assert.AreNotEqual") ||
                stackTrace.Contains("Assert.IsNull") ||
                stackTrace.Contains("Assert.IsNotNull"))
                return true;

            return false;
        }
    }
}
