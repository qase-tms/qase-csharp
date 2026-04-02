using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reqnroll;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;

namespace Qase.Reqnroll.Reporter
{
    /// <summary>
    /// Reqnroll binding hooks that capture test lifecycle events
    /// and report results to Qase TMS.
    /// </summary>
    [Binding]
    public class QaseReqnrollBindings
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly ICoreReporter _reporter;

        private const string TestResultKey = "QaseTestResult";
        private const string StepStartTimeKey = "QaseStepStartTime";
        private const string StepsKey = "QaseSteps";
        private const string StepFailedKey = "QaseStepFailed";

        public QaseReqnrollBindings(
            ScenarioContext scenarioContext,
            FeatureContext featureContext,
            ICoreReporter reporter)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _reporter = reporter;
        }

        [BeforeTestRun(Order = 0)]
        public static void BeforeTestRun()
        {
            try
            {
                var reporter = CoreReporterFactory.GetInstance();
                reporter.startTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed to start test run: {ex.Message}");
            }
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            try
            {
                var result = new TestResult();
                var scenarioInfo = _scenarioContext.ScenarioInfo;

                // Apply tags from feature + scenario
                ReqnrollTagParser.ApplyTags(result, scenarioInfo.CombinedTags);

                // Skip ignored scenarios
                if (result.Ignore)
                {
                    _scenarioContext[TestResultKey] = result;
                    return;
                }

                // Set title from scenario name if not overridden by tag
                if (string.IsNullOrEmpty(result.Title))
                {
                    result.Title = scenarioInfo.Title;
                }

                // Set suite from feature title if not overridden by tag
                if (result.Relations?.Suite.Data == null || result.Relations.Suite.Data.Count == 0)
                {
                    result.Relations ??= new Relations();
                    result.Relations.Suite.Data = new List<SuiteData>
                    {
                        new SuiteData { Title = _featureContext.FeatureInfo.Title }
                    };
                }

                // Extract Scenario Outline parameters
                if (scenarioInfo.Arguments != null && scenarioInfo.Arguments.Count > 0)
                {
                    foreach (DictionaryEntry entry in scenarioInfo.Arguments)
                    {
                        var keyStr = entry.Key?.ToString();
                        var valStr = entry.Value?.ToString();
                        if (keyStr != null)
                        {
                            result.Params[keyStr] = valStr ?? string.Empty;
                        }
                    }
                }

                // Set execution timing
                result.Execution!.StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                result.Execution.Thread = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();

                // Set display name for ContextManager
                var displayName = GenerateDisplayName(scenarioInfo.Title, result.Params);
                ContextManager.SetTestCaseName(displayName);

                _scenarioContext[TestResultKey] = result;
                _scenarioContext[StepsKey] = new List<StepResult>();
                _scenarioContext[StepFailedKey] = false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed in BeforeScenario: {ex.Message}");
            }
        }

        [BeforeStep(Order = 0)]
        public void BeforeStep()
        {
            try
            {
                if (!_scenarioContext.ContainsKey(TestResultKey))
                    return;

                var testResult = (TestResult)_scenarioContext[TestResultKey];
                if (testResult.Ignore)
                    return;

                _scenarioContext[StepStartTimeKey] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed in BeforeStep: {ex.Message}");
            }
        }

        [AfterStep(Order = int.MaxValue)]
        public void AfterStep()
        {
            try
            {
                if (!_scenarioContext.ContainsKey(TestResultKey))
                    return;

                var testResult = (TestResult)_scenarioContext[TestResultKey];
                if (testResult.Ignore)
                    return;

                var stepContext = _scenarioContext.StepContext;
                var stepInfo = stepContext.StepInfo;

                var stepTitle = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";

                var startTime = _scenarioContext.ContainsKey(StepStartTimeKey)
                    ? (long)_scenarioContext[StepStartTimeKey]
                    : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var previousStepFailed = (bool)_scenarioContext[StepFailedKey];

                StepResultStatus stepStatus;
                if (previousStepFailed)
                {
                    stepStatus = StepResultStatus.Skipped;
                }
                else if (stepContext.Status == ScenarioExecutionStatus.TestError)
                {
                    stepStatus = StepResultStatus.Failed;
                    _scenarioContext[StepFailedKey] = true;
                }
                else if (stepContext.Status == ScenarioExecutionStatus.OK)
                {
                    stepStatus = StepResultStatus.Passed;
                }
                else
                {
                    stepStatus = StepResultStatus.Skipped;
                    _scenarioContext[StepFailedKey] = true;
                }

                var stepResult = new StepResult
                {
                    Data = new Data { Action = stepTitle },
                    Execution = new StepExecution
                    {
                        Status = stepStatus,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = endTime - startTime
                    }
                };

                var steps = (List<StepResult>)_scenarioContext[StepsKey];
                steps.Add(stepResult);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed in AfterStep: {ex.Message}");
            }
        }

        [AfterScenario(Order = int.MaxValue)]
        public void AfterScenario()
        {
            try
            {
                if (!_scenarioContext.ContainsKey(TestResultKey))
                    return;

                var result = (TestResult)_scenarioContext[TestResultKey];
                if (result.Ignore)
                    return;

                // Set execution end time and duration
                var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                result.Execution!.EndTime = endTime;
                if (result.Execution.StartTime.HasValue)
                {
                    result.Execution.Duration = (int)(endTime - result.Execution.StartTime.Value);
                }

                // Map scenario status
                result.Execution.Status = MapStatus(_scenarioContext.ScenarioExecutionStatus);

                // Set error info
                if (_scenarioContext.TestError != null)
                {
                    result.Message = _scenarioContext.TestError.Message;
                    result.Execution.Stacktrace = _scenarioContext.TestError.StackTrace;
                }

                // Collect automatic BDD steps
                if (_scenarioContext.ContainsKey(StepsKey))
                {
                    result.Steps = (List<StepResult>)_scenarioContext[StepsKey];
                }

                // Collect any manual steps from ContextManager
                var displayName = GenerateDisplayName(
                    _scenarioContext.ScenarioInfo.Title, result.Params);
                var manualSteps = ContextManager.GetCompletedSteps(displayName);
                if (manualSteps.Count > 0)
                {
                    result.Steps.AddRange(manualSteps);
                }

                // Collect comments and attachments
                var comments = ContextManager.GetComments(displayName);
                if (!string.IsNullOrEmpty(comments))
                {
                    result.Message = string.IsNullOrEmpty(result.Message)
                        ? comments
                        : $"{result.Message}\n{comments}";
                }

                var attachments = ContextManager.GetAttachments(displayName);
                if (attachments.Count > 0)
                {
                    result.Attachments.AddRange(attachments);
                }

                // Generate signature
                var suites = result.Relations?.Suite.Data
                    .Select(s => s.Title)
                    .ToList();

                result.Signature = Signature.Generate(
                    result.TestopsIds, suites, result.Params);

                // Submit result
                _reporter.addResult(result).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed in AfterScenario: {ex.Message}");
            }
        }

        [AfterTestRun(Order = int.MaxValue)]
        public static void AfterTestRun()
        {
            try
            {
                var reporter = CoreReporterFactory.GetInstance();
                reporter.uploadResults().GetAwaiter().GetResult();
                reporter.completeTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Qase] Failed to complete test run: {ex.Message}");
            }
        }

        internal static TestResultStatus MapStatus(ScenarioExecutionStatus status)
        {
            switch (status)
            {
                case ScenarioExecutionStatus.OK:
                    return TestResultStatus.Passed;
                case ScenarioExecutionStatus.TestError:
                    return TestResultStatus.Failed;
                case ScenarioExecutionStatus.UndefinedStep:
                case ScenarioExecutionStatus.BindingError:
                    return TestResultStatus.Invalid;
                case ScenarioExecutionStatus.StepDefinitionPending:
                case ScenarioExecutionStatus.Skipped:
                    return TestResultStatus.Skipped;
                default:
                    return TestResultStatus.Skipped;
            }
        }

        private static string GenerateDisplayName(string title, Dictionary<string, string>? parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return title;

            var paramStr = string.Join(", ", parameters.Select(p => $"{p.Key}: {p.Value}"));
            return $"{title}({paramStr})";
        }
    }
}
