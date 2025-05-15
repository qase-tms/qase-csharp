using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.ApiClient.V2.Api;
using Qase.ApiClient.V2.Client;
using Qase.ApiClient.V2.Extensions;
using Qase.ApiClient.V2.Model;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Clients
{
    /// <summary>
    /// Implementation of the Qase API client using V2 API
    /// </summary>
    public class ApiClientV2 : IClient
    {
        private readonly QaseConfig _config;
        private readonly ApiClientV1 _apiClientV1;
        private readonly ILogger<ApiClientV2> _logger;
        private readonly IResultsApi _resultsApi;
        
        /// <summary>
        /// Initializes a new instance of the ApiClientV2 class
        /// </summary>
        /// <param name="config">The configuration for the client</param>
        public ApiClientV2(QaseConfig config)
        {
            _config = config;
            _apiClientV1 = new ApiClientV1(config);
            _logger = NullLogger<ApiClientV2>.Instance;
            
            var services = new ServiceCollection();
            
            var baseUrl = config.TestOps.Api.Host == "qase.io" ? 
                "https://api.qase.io/v2" : 
                $"https://api-{config.TestOps.Api.Host}/v2";
            
            services.AddApi(options =>
            {
                ApiKeyToken token = new(config.TestOps.Api.Token!, ClientUtils.ApiKeyHeader.Token, "");
                options.AddTokens(token);
                options.AddApiHttpClients(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                });
            });
            
            var serviceProvider = services.BuildServiceProvider();
            _resultsApi = serviceProvider.GetRequiredService<IResultsApi>();
            
            _logger.LogDebug("ApiClientV2 initialized with base URL: {BaseUrl}", baseUrl);
        }
        
        /// <inheritdoc />
        public Task<long> CreateTestRunAsync()
        {
            return _apiClientV1.CreateTestRunAsync();
        }
        
        /// <inheritdoc />
        public Task CompleteTestRunAsync(long runId)
        {
            return _apiClientV1.CompleteTestRunAsync(runId);
        }
        
        /// <inheritdoc />
        public async Task UploadResultsAsync(long runId, List<TestResult> results)
        {
            _logger.LogDebug("Uploading {Count} test results for run {RunId}", results.Count, runId);
            
            var convertedResults = results.Select(ConvertResult).ToList();
            
            var model = new CreateResultsRequestV2
            {
                Results = convertedResults
            };
            
            try
            {
                _logger.LogDebug("Sending request to upload results with data: {@Results}", model);
                var response = await _resultsApi.CreateResultsV2Async(_config.TestOps.Project!, runId, model);
                
                if (!response.IsAccepted)
                {
                    _logger.LogError(
                        "Failed to upload test results. StatusCode={StatusCode}, Reason={Reason}, Response={@Response}",
                        response.StatusCode,
                        response.ReasonPhrase,
                        response.RawContent
                    );
                    throw new QaseException($"Failed to upload test results: {response.RawContent}");
                }
                
                _logger.LogDebug("Test results uploaded successfully");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "API error while uploading test results");
                throw new QaseException($"Failed to upload test results: {ex.Message}", ex);
            }
        }
        
        /// <inheritdoc />
        public Task<List<long>> GetTestCaseIdsForExecutionAsync()
        {
            return _apiClientV1.GetTestCaseIdsForExecutionAsync();
        }
        
        private ResultCreate ConvertResult(TestResult result)
        {
            _logger.LogDebug("Converting test result: {@TestResult}", result);
            
            var attachments = result.Attachments
                .Select(async a => await _apiClientV1.UploadAttachmentAsync(a))
                .Select(t => t.Result)
                .Where(attachment => !string.IsNullOrEmpty(attachment))
                .ToList();
            
            var steps = new List<ResultStep>();
            if (result.Steps != null && result.Steps.Count > 0)
            {
                steps = result.Steps.Select(ConvertStepResult).ToList();
            }
            
            var execution = new ResultExecution(
                status: MapStatus(result.Execution?.Status),
                startTime: result.Execution?.StartTime.HasValue == true ? 
                    result.Execution.StartTime.Value / 1000.0 : null,
                endTime: result.Execution?.EndTime.HasValue == true ? 
                    result.Execution.EndTime.Value / 1000.0 : null,
                duration: result.Execution?.Duration,
                stacktrace: result.Execution?.Stacktrace,
                thread: result.Execution?.Thread
            );
            
            var relations = new ResultRelations();
            if (result.Relations?.Suite?.Data != null)
            {
                var data = new List<RelationSuiteItem>();
                foreach (var suiteData in result.Relations.Suite.Data)
                {
                    var item = new RelationSuiteItem(
                        title: suiteData.Title!
                    );
                    data.Add(item);
                }
                
                relations.Suite = new RelationSuite(data);
            }
            
            var fields = new ResultCreateFields();
            if (result.Fields != null && result.Fields.Count > 0)
            {
                foreach (var key in result.Fields.Keys)
                {
                    switch (key)
                    {
                        case "author":
                            fields.Author = result.Fields[key];
                            break;
                        case "description":
                            fields.Description = result.Fields[key];
                            break;
                        case "preconditions":
                            fields.Preconditions = result.Fields[key];
                            break;
                        case "postconditions":
                            fields.Postconditions = result.Fields[key];
                            break;
                        case "layer":
                            fields.Layer = result.Fields[key];
                            break;
                        case "severity":
                            fields.Severity = result.Fields[key];
                            break;
                        case "priority":
                            fields.Priority = result.Fields[key];
                            break;
                        case "behavior":
                            fields.Behavior = result.Fields[key];
                            break;
                        case "type":
                            fields.Type = result.Fields[key];
                            break;
                        case "muted":
                            fields.Muted = result.Fields[key];
                            break;
                        case "isFlaky":
                            fields.IsFlaky = result.Fields[key];
                            break;
                        default:
                            fields.AdditionalProperties[key] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(result.Fields[key]));
                            break;
                    }
                }
            }
            
            var paramGroups = result.ParamGroups?.Select(g => g.Select(p => p.ToString()).ToList()).ToList();
            
            var convertedResult = new ResultCreate(
                title: result.Title!,
                execution: execution,
                id: result.Id,
                signature: result.Signature,
                testopsIds: result.TestopsIds,
                fields: fields,
                attachments: attachments,
                steps: steps,
                stepsType: ResultStepsType.Classic,
                @params: result.Params,
                paramGroups: paramGroups,
                relations: relations,
                message: result.Message,
                defect: _config.TestOps.Defect
            );
            
            _logger.LogDebug("Converted test result: {@ConvertedResult}", convertedResult);
            return convertedResult;
        }
        
        private ResultStep ConvertStepResult(StepResult step)
        {
            _logger.LogDebug("Converting step result: {@StepResult}", step);
            
            var data = new ResultStepData(
                action: step.Data?.Action ?? string.Empty,
                expectedResult: step.Data?.ExpectedResult,
                inputData: step.Data?.InputData
            );
            
            var attachments = step.Execution?.Attachments
                .Select(async a => await _apiClientV1.UploadAttachmentAsync(a))
                .Select(t => t.Result)
                .Where(attachment => !string.IsNullOrEmpty(attachment))
                .ToList() ?? new List<string>();
            
            var execution = new ResultStepExecution(
                status: MapStepStatus(step.Execution?.Status),
                startTime: step.Execution?.StartTime / 1000.0,
                endTime: step.Execution?.EndTime / 1000.0,
                duration: step.Execution?.Duration,
                attachments: attachments
            );
            
            var convertedStep = new ResultStep(
                data: data,
                execution: execution,
                steps: step.Steps.Select(ConvertStepResult).ToList()
            );
            
            _logger.LogDebug("Converted step result: {@ConvertedStep}", convertedStep);
            return convertedStep;
        }
        
        private string MapStatus(TestResultStatus? status)
        {
            if (status == null)
            {
                return "failed";
            }
            
            switch (status)
            {
                case TestResultStatus.Passed:
                    return "passed";
                case TestResultStatus.Failed:
                    return "failed";
                case TestResultStatus.Skipped:
                    return "skipped";
                default:
                    return "failed";
            }
        }
        
        private ResultStepStatus MapStepStatus(StepResultStatus? status)
        {
            if (status == null)
            {
                return ResultStepStatus.Skipped;
            }
            
            switch (status)
            {
                case StepResultStatus.Passed:
                    return ResultStepStatus.Passed;
                case StepResultStatus.Failed:
                    return ResultStepStatus.Failed;
                case StepResultStatus.Skipped:
                    return ResultStepStatus.Skipped;
                case StepResultStatus.Blocked:
                    return ResultStepStatus.Blocked;
                default:
                    return ResultStepStatus.Failed;
            }
        }
    }
} 

