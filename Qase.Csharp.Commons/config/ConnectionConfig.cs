namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for connection settings
    /// </summary>
    public class ConnectionConfig
    {
        /// <summary>
        /// Gets or sets the local config
        /// </summary>
        public LocalConfig Local { get; set; } = new LocalConfig();
    }
}
