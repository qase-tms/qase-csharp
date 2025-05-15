namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents test relations
    /// </summary>
    public class Relations
    {
        /// <summary>
        /// Gets or sets the suite
        /// </summary>
        public Suite Suite { get; set; }

        /// <summary>
        /// Initializes a new instance of the Relations class
        /// </summary>
        public Relations()
        {
            Suite = new Suite();
        }
    }
} 
