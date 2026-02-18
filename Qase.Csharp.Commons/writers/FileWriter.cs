using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Qase.Csharp.Commons.Writers
{
    /// <summary>
    /// Writes test results to a directory-based structure
    /// </summary>
    public class FileWriter
    {
        private readonly string _rootPath;
        private readonly string _resultsPath;

        /// <summary>
        /// Initializes a new instance of the FileWriter class
        /// </summary>
        /// <param name="rootPath">The root directory path for the report</param>
        public FileWriter(string rootPath)
        {
            _rootPath = rootPath;
            _resultsPath = Path.Combine(rootPath, "results");
        }

        /// <summary>
        /// Prepares the directory structure for writing
        /// </summary>
        public void Prepare()
        {
            Directory.CreateDirectory(_rootPath);
            Directory.CreateDirectory(_resultsPath);
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
