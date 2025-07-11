namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Represents a configuration item with name and value
    /// </summary>
    public class ConfigurationItem
    {
        /// <summary>
        /// Gets or sets the configuration name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the configuration value
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
} 
