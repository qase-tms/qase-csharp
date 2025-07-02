using System.Collections.Generic;

namespace Qase.Csharp.Commons.Methods
{
    /// <summary>
    /// Metadata methods for Qase.
    /// </summary>
    public static class Metadata
    {
        /// <summary>
        /// Add a comment to the test case.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        public static void Comment(string comment)
        {
            ContextManager.AddComment(comment);
        }

        /// <summary>
        /// Add an attachment to the test case.
        /// </summary>
        /// <param name="path">The path to the attachment.</param>
        public static void Attach(string path)
        {
            ContextManager.AddAttachment(new List<string> { path });
        }

        /// <summary>
        /// Add attachments to the test case.
        /// </summary>
        /// <param name="paths">The paths to the attachments.</param>
        public static void Attach(List<string> paths)
        {
            ContextManager.AddAttachment(paths);
        }

        /// <summary>
        /// Add an attachment to the test case.
        /// </summary>
        /// <param name="data">The data to add.</param>
        /// <param name="fileName">The name of the attachment.</param>
        public static void Attach(byte[] data, string fileName)
        {
            ContextManager.AddAttachment(data, fileName);
        }
    }
}
