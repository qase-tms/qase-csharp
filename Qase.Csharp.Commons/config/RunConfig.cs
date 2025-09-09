using System.Collections.Generic;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for test run settings
    /// </summary>
    public class RunConfig
    {
        /// <summary>
        /// Gets or sets the run ID
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// Gets or sets the run title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the run description
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Gets or sets whether to complete the run after test
        /// </summary>
        public bool Complete { get; set; } = true;

        /// <summary>
        /// Gets or sets the run tags
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the external link configuration
        /// </summary>
        public TestOpsExternalLinkType? ExternalLink { get; set; }
    }
} 
