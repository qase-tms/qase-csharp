using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Qase.Csharp.Commons.Writers
{
    /// <summary>
    /// Writes test results to a file
    /// </summary>
    public class FileWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        
        /// <summary>
        /// Initializes a new instance of the FileWriter class
        /// </summary>
        /// <param name="filePath">The path to the file to write to</param>
        public FileWriter(string filePath)
        {
            _writer = new StreamWriter(filePath, false, Encoding.UTF8);
        }
        
        /// <summary>
        /// Writes a line to the file
        /// </summary>
        /// <param name="line">The line to write</param>
        public async Task WriteLineAsync(string line)
        {
            await _writer.WriteLineAsync(line);
        }
        
        /// <summary>
        /// Flushes the writer
        /// </summary>
        public async Task FlushAsync()
        {
            await _writer.FlushAsync();
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
} 
