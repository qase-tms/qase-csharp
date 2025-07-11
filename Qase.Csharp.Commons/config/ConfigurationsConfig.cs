using System.Collections.Generic;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configurations block for TestOps config
    /// </summary>
    public class ConfigurationsConfig
    {
        /// <summary>
        /// List of configuration items (group+value)
        /// </summary>
        public List<ConfigurationItem> Values { get; set; } = new();

        /// <summary>
        /// Whether to create group/config if not exists
        /// </summary>
        public bool CreateIfNotExists { get; set; } = false;
    }
} 
