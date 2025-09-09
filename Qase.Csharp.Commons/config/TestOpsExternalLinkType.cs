namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for external link settings
    /// </summary>
    public class TestOpsExternalLinkType
    {
        /// <summary>
        /// Gets or sets the external link type
        /// </summary>
        public ExternalLinkType Type { get; set; }

        /// <summary>
        /// Gets or sets the external link URL or identifier
        /// </summary>
        public string Link { get; set; } = string.Empty;
    }
}
