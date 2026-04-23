#nullable enable

using System.Collections.Generic;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Intermediate structure holding raw test data from a framework reporter.
    /// Each reporter fills the fields available to it; the TestResultBuilder
    /// assembles a complete TestResult from this data.
    /// </summary>
    public class RawTestData
    {
        // Method identification (for reflection-based attribute extraction)
        public string? FullTypeName { get; set; }
        public string? MethodName { get; set; }
        public string[]? ParameterTypeFullNames { get; set; }

        // Test identity
        public string DisplayName { get; set; } = "";
        public string? FullTestName { get; set; }

        // Status and timing
        public TestResultStatus Status { get; set; }
        public long? StartTime { get; set; }
        public long? EndTime { get; set; }
        public int? Duration { get; set; }
        public string? Thread { get; set; }

        // Error details
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }

        // Failure classification hints
        public string? FailureLabel { get; set; }
        public int? AssertsCount { get; set; }
        public string[]? ExceptionTypes { get; set; }

        // Parameters (pre-parsed by framework, if available)
        public Dictionary<string, string>? PreParsedParams { get; set; }

        // Steps (pre-collected by framework, e.g. Reqnroll BDD steps)
        public List<StepResult>? PreCollectedSteps { get; set; }

        // ContextManager display name base override
        public string? ContextDisplayNameBase { get; set; }
    }
}
