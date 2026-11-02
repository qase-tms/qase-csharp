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
            // Third-party assertion libraries throw their own exceptions, which NUnit labels "Error"
            // and counts as zero asserts; the assertion evidence has to win over both.
            if (ContainsAssertionException(exceptionTypes) ||
                (!string.IsNullOrEmpty(stackTrace) && ContainsAssertionPattern(stackTrace!)))
                return TestResultStatus.Failed;

            // Explicit "Error" label → Invalid (NUnit)
            if (!string.IsNullOrEmpty(failureLabel) && failureLabel == "Error")
                return TestResultStatus.Invalid;

            // assertsCount == 0 → Invalid (NUnit)
            if (assertsCount.HasValue && assertsCount.Value == 0)
                return TestResultStatus.Invalid;

            // Default: Failed (safer than Invalid — assume assertion failure)
            return TestResultStatus.Failed;
        }

        private static readonly string[] AssertionNamespaces =
        {
            "NUnit.Framework.",
            "Xunit.Sdk.",
            "Microsoft.VisualStudio.TestTools.UnitTesting.",
            "FluentAssertions.",
            "AwesomeAssertions.",
            "Shouldly."
        };

        private static bool ContainsAssertionException(string[]? exceptionTypes)
        {
            if (exceptionTypes == null || exceptionTypes.Length == 0)
                return false;

            // xUnit reports the whole chain; the outermost exception decides.
            var type = exceptionTypes[0];
            foreach (var ns in AssertionNamespaces)
            {
                if (type.StartsWith(ns, System.StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        private static bool ContainsAssertionPattern(string stackTrace)
        {
            foreach (var ns in AssertionNamespaces)
            {
                if (stackTrace.Contains("at " + ns))
                    return true;
            }

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
