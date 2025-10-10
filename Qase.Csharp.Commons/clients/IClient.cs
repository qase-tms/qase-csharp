using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Clients
{
    /// <summary>
    /// Interface for API client operations
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Creates a new test run
        /// </summary>
        /// <returns>The ID of the created test run</returns>
        /// <exception cref="QaseException">Thrown when an error occurs during API communication</exception>
        Task<long> CreateTestRunAsync();

        /// <summary>
        /// Completes a test run
        /// </summary>
        /// <param name="runId">The ID of the test run to complete</param>
        /// <exception cref="QaseException">Thrown when an error occurs during API communication</exception>
        Task CompleteTestRunAsync(long runId);

        /// <summary>
        /// Uploads test results for a specific test run
        /// </summary>
        /// <param name="runId">The ID of the test run</param>
        /// <param name="results">List of test results to upload</param>
        /// <exception cref="QaseException">Thrown when an error occurs during API communication</exception>
        Task UploadResultsAsync(long runId, List<TestResult> results);
        
        /// <summary>
        /// Gets test case IDs for execution
        /// </summary>
        /// <returns>List of test case IDs</returns>
        /// <exception cref="QaseException">Thrown when an error occurs during API communication</exception>
        Task<List<long>> GetTestCaseIdsForExecutionAsync();

        /// <summary>
        /// Enables public report for a specific test run
        /// </summary>
        /// <param name="runId">The ID of the test run</param>
        /// <returns>The public report URL</returns>
        /// <exception cref="QaseException">Thrown when an error occurs during API communication</exception>
        Task<string> EnablePublicReportAsync(long runId);
    }
}
