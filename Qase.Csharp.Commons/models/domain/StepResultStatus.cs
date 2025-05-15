namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents the status of a test step
    /// </summary>
    public enum StepResultStatus
    {
        /// <summary>
        /// Step passed
        /// </summary>
        Passed,

        /// <summary>
        /// Step failed
        /// </summary>
        Failed,

        /// <summary>
        /// Step skipped
        /// </summary>
        Skipped,

        /// <summary>
        /// Step blocked
        /// </summary>
        Blocked,

        /// <summary>
        /// Step untested
        /// </summary>
        Untested
    }
} 
