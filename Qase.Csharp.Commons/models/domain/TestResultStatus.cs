namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents the status of a test result
    /// </summary>
    public enum TestResultStatus
    {
        /// <summary>
        /// Test passed
        /// </summary>
        Passed,

        /// <summary>
        /// Test failed
        /// </summary>
        Failed,

        /// <summary>
        /// Test skipped
        /// </summary>
        Skipped,

        /// <summary>
        /// Test invalid (failed due to non-assertion reasons)
        /// </summary>
        Invalid
    }
} 
