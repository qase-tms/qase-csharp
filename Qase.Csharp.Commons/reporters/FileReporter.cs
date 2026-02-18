using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Serialization;
using Qase.Csharp.Commons.Writers;

namespace Qase.Csharp.Commons.Reporters
{
    /// <summary>
    /// Reporter that writes test results to a file
    /// </summary>
    public class FileReporter : IInternalReporter
    {
        private readonly ILogger<FileReporter> _logger;
        private readonly QaseConfig _config;
        private readonly FileWriter _writer;
        private readonly List<TestResult> _results;
        
        /// <summary>
        /// Initializes a new instance of the FileReporter class
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="config">The configuration for the reporter</param>
        /// <param name="writer">The writer to use</param>
        public FileReporter(
            ILogger<FileReporter> logger,
            QaseConfig config,
            FileWriter writer)
        {
            _logger = logger;
            _config = config;
            _writer = writer;
            _results = new List<TestResult>();
        }
        
        /// <inheritdoc />
        public Task startTestRun()
        {
            return Task.CompletedTask;
        }
        
        /// <inheritdoc />
        public Task completeTestRun()
        {
            return Task.CompletedTask;
        }
        
        /// <inheritdoc />
        public Task addResult(TestResult result)
        {
            _results.Add(result);
            return Task.CompletedTask;
        }
        
        /// <inheritdoc />
        public async Task uploadResults()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                Converters =
                {
                    new LowercaseEnumConverter<TestResultStatus>(),
                    new LowercaseEnumConverter<StepResultStatus>()
                }
            };

            var json = JsonSerializer.Serialize(_results, options);

            await _writer.WriteLineAsync(json);
            await _writer.FlushAsync();
            _results.Clear();
        }
        
        /// <inheritdoc />
        public Task<List<TestResult>> getResults()
        {
            return Task.FromResult(_results);
        }
        
        /// <inheritdoc />
        public Task setResults(List<TestResult> results)
        {
            _results.Clear();
            _results.AddRange(results);
            return Task.CompletedTask;
        }
    }
} 
