using Qase.ApiClient.V1.Client;

namespace Qase.Csharp.Commons.Core
{
    /// <summary>
    /// Provides MIME type detection based on file extension
    /// </summary>
    internal static class MimeTypes
    {
        /// <summary>
        /// Guesses the MIME type from a file name based on its extension.
        /// Returns "application/octet-stream" if the type cannot be determined.
        /// </summary>
        public static string GuessFromFileName(string fileName) => FileContentTypes.GuessFromFileName(fileName);
    }
}
