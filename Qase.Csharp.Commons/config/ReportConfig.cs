using System;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for report settings
    /// </summary>
    public class ReportConfig
    {
        /// <summary>
        /// Gets or sets the driver
        /// </summary>
        public Driver Driver { get; set; } = Driver.Local;

        /// <summary>
        /// Gets or sets the connection config
        /// </summary>
        public ConnectionConfig Connection { get; set; } = new ConnectionConfig();

        public void SetDriver(string? driver)
        {
            if (string.IsNullOrEmpty(driver) || driver == "local")
            {
                Driver = Driver.Local;
            }
            else
            {
                throw new ArgumentException("Invalid driver");
            }
        }
    }
}
