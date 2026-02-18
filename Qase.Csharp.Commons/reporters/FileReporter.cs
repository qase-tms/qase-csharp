using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Models.Report;
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
        private long _startTime;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new LowercaseEnumConverter<TestResultStatus>(),
                new LowercaseEnumConverter<StepResultStatus>()
            }
        };

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
            _startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _writer.Prepare();
            return Task.CompletedTask;
        }
        
        /// <inheritdoc />
        public async Task completeTestRun()
        {
            var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Write each result as individual file
            foreach (var result in _results)
            {
                var json = JsonSerializer.Serialize(result, _options);
                await _writer.WriteResultAsync(result.Id, json);
            }

            // Build the Run model
            var run = new Run(
                _config.TestOps?.Run?.Title ?? "Test run",
                _startTime,
                endTime,
                _config.Environment
            );

            // Populate run with result summaries
            foreach (var result in _results)
            {
                var status = result.Execution?.Status.ToString().ToLowerInvariant() ?? "invalid";
                var duration = result.Execution?.Duration ?? 0;
                var thread = result.Execution?.Thread;
                run.AddResult(result.Id, result.Title ?? "", status, duration, thread, result.Muted);
            }
            run.ComputeThreads();

            // Serialize and write run.json
            var runJson = JsonSerializer.Serialize(run, _options);
            await _writer.WriteRunAsync(runJson);

            // Clear results
            _results.Clear();
        }
        
        /// <inheritdoc />
        public Task addResult(TestResult result)
        {
            _results.Add(result);
            return Task.CompletedTask;
        }
        
        /// <inheritdoc />
        public Task uploadResults()
        {
            return Task.CompletedTask;
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
