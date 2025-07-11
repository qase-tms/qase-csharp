using System.Collections.Generic;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for TestOps settings
    /// </summary>
    public class TestOpsConfig
    {
        /// <summary>
        /// Gets or sets the project
        /// </summary>
        public string? Project { get; set; }

        /// <summary>
        /// Gets or sets whether to create defects in TestOps
        /// </summary>
        public bool Defect { get; set; } = false;

        /// <summary>
        /// Gets or sets the API config
        /// </summary>
        public ApiConfig Api { get; set; } = new ApiConfig();

        /// <summary>
        /// Gets or sets the batch config
        /// </summary>
        public BatchConfig Batch { get; set; } = new BatchConfig();

        /// <summary>
        /// Gets or sets the run config
        /// </summary>
        public RunConfig Run { get; set; } = new RunConfig();

        /// <summary>
        /// Gets or sets the plan config
        /// </summary>
        public PlanConfig Plan { get; set; } = new PlanConfig();

        /// <summary>
        /// Gets or sets the configurations
        /// </summary>
        public ConfigurationsConfig Configurations { get; set; } = new ConfigurationsConfig();
    }
}
