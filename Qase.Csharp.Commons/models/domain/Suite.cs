using System.Collections.Generic;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a test suite
    /// </summary>
    public class Suite
    {
        /// <summary>
        /// Gets or sets the suite data
        /// </summary>
        public List<SuiteData> Data { get; set; } = new();
    }
} 
