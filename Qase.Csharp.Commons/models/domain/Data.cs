using System.Collections.Generic;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents test step data
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets or sets the action
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Gets or sets the expected result
        /// </summary>
        public string? ExpectedResult { get; set; }

        /// <summary>
        /// Gets or sets the input data
        /// </summary>
        public string? InputData { get; set; }
    }
} 
