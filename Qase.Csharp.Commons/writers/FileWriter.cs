using System.IO;
using System.Text;
using System.Threading.Tasks;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Writers
{
    /// <summary>
    /// Writes test results to a directory-based structure
    /// </summary>
    public class FileWriter
    {
        private readonly string _rootPath;
        private readonly string _resultsPath;
        private readonly string _attachmentsPath;

        /// <summary>
        /// Initializes a new instance of the FileWriter class
        /// </summary>
        /// <param name="rootPath">The root directory path for the report</param>
        public FileWriter(string rootPath)
        {
            _rootPath = rootPath;
            _resultsPath = Path.Combine(rootPath, "results");
            _attachmentsPath = Path.Combine(rootPath, "attachments");
        }

        /// <summary>
        /// Prepares the directory structure for writing
        /// </summary>
        public void Prepare()
        {
            if (Directory.Exists(_resultsPath))
            {
                Directory.Delete(_resultsPath, true);
            }

            if (Directory.Exists(_attachmentsPath))
            {
                Directory.Delete(_attachmentsPath, true);
            }

            Directory.CreateDirectory(_rootPath);
            Directory.CreateDirectory(_resultsPath);
            Directory.CreateDirectory(_attachmentsPath);
        }

        /// <summary>
        /// Writes an attachment file to the attachments directory.
        /// Copies from FilePath, writes Content string, or writes ContentBytes.
        /// Updates the attachment's FilePath to the destination path.
        /// </summary>
        /// <param name="attachment">The attachment to write</param>
        public void WriteAttachment(Attachment attachment)
        {
            if (attachment == null) return;

            var fileName = attachment.FileName ?? attachment.Id;
            var destName = $"{attachment.Id}-{fileName}";
            var destPath = Path.Combine(_attachmentsPath, destName);

            if (string.IsNullOrEmpty(attachment.MimeType))
            {
                attachment.MimeType = MimeTypes.GuessFromFileName(fileName);
            }

            if (attachment.FilePath != null && File.Exists(attachment.FilePath))
            {
                File.Copy(attachment.FilePath, destPath, true);
                attachment.FilePath = Path.GetFullPath(destPath);
                attachment.Content = null;
                attachment.ContentBytes = null;
            }
            else if (attachment.Content != null)
            {
                File.WriteAllText(destPath, attachment.Content, Encoding.UTF8);
                attachment.FilePath = Path.GetFullPath(destPath);
                attachment.Content = null;
                attachment.ContentBytes = null;
            }
            else if (attachment.ContentBytes != null)
            {
                File.WriteAllBytes(destPath, attachment.ContentBytes);
                attachment.FilePath = Path.GetFullPath(destPath);
                attachment.Content = null;
                attachment.ContentBytes = null;
            }
        }

        /// <summary>
        /// Writes the run.json file
        /// </summary>
        /// <param name="json">The JSON content to write</param>
        public async Task WriteRunAsync(string json)
        {
            var path = Path.Combine(_rootPath, "run.json");
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                await writer.WriteAsync(json);
            }
        }

        /// <summary>
        /// Writes an individual result file
        /// </summary>
        /// <param name="id">The result ID</param>
        /// <param name="json">The JSON content to write</param>
        public async Task WriteResultAsync(string id, string json)
        {
            var path = Path.Combine(_resultsPath, $"{id}.json");
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                await writer.WriteAsync(json);
            }
        }
    }
} 
