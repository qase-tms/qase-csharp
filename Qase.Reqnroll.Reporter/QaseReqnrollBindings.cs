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
        private readonly ITestResultBuilder _builder = new TestResultBuilder();

        private const string RawTestDataKey = "QaseRawTestData";
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
                var scenarioInfo = _scenarioContext.ScenarioInfo;

                // Check for @QaseIgnore before creating full raw data
                var checkResult = new TestResult();
                ReqnrollTagParser.ApplyTags(checkResult, scenarioInfo.CombinedTags);
                if (checkResult.Ignore)
                {
                    // Store a marker raw that signals ignore; BeforeStep/AfterStep/AfterScenario will skip
                    var ignoredRaw = new RawTestData { DisplayName = scenarioInfo.Title };
                    _scenarioContext[RawTestDataKey] = ignoredRaw;
                    return;
                }

                // Build RawTestData
                var raw = new RawTestData
                {
                    DisplayName = scenarioInfo.Title,
                    ContextDisplayNameBase = scenarioInfo.Title,
                    Status = TestResultStatus.Skipped,
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Thread = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()
                };

                // Extract Scenario Outline parameters
                if (scenarioInfo.Arguments != null && scenarioInfo.Arguments.Count > 0)
                {
                    raw.PreParsedParams = new Dictionary<string, string>();
                    foreach (DictionaryEntry entry in scenarioInfo.Arguments)
                    {
                        var keyStr = entry.Key?.ToString();
                        var valStr = entry.Value?.ToString();
                        if (keyStr != null)
                        {
                            raw.PreParsedParams[keyStr] = valStr ?? string.Empty;
                        }
                    }
                }

                // Set display name for ContextManager (using DisplayNameGenerator)
                var displayName = DisplayNameGenerator.Generate(scenarioInfo.Title, raw.PreParsedParams);
                ContextManager.SetTestCaseName(displayName);

                _scenarioContext[RawTestDataKey] = raw;
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
                if (!_scenarioContext.ContainsKey(RawTestDataKey))
                    return;

                // If StepsKey isn't present, the scenario was ignored — skip step tracking
                if (!_scenarioContext.ContainsKey(StepsKey))
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
                if (!_scenarioContext.ContainsKey(RawTestDataKey))
                    return;

                // If StepsKey isn't present, the scenario was ignored — skip step tracking
                if (!_scenarioContext.ContainsKey(StepsKey))
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
                if (!_scenarioContext.ContainsKey(RawTestDataKey))
                    return;

                var raw = (RawTestData)_scenarioContext[RawTestDataKey];

                // If StepsKey isn't present, the scenario was ignored
                if (!_scenarioContext.ContainsKey(StepsKey))
                    return;

                // Set execution end time and duration
                var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                raw.EndTime = endTime;
                if (raw.StartTime.HasValue)
                {
                    raw.Duration = (int)(endTime - raw.StartTime.Value);
                }

                // Map scenario status
                raw.Status = MapStatus(_scenarioContext.ScenarioExecutionStatus);

                // Set error info
                if (_scenarioContext.TestError != null)
                {
                    raw.ErrorMessage = _scenarioContext.TestError.Message;
                    raw.StackTrace = _scenarioContext.TestError.StackTrace;
                }

                // Collect automatic BDD steps as pre-collected steps for the builder
                if (_scenarioContext.ContainsKey(StepsKey))
                {
                    raw.PreCollectedSteps = (List<StepResult>)_scenarioContext[StepsKey];
                }

                // Build the test result
                var result = _builder.Build(raw);

                // Apply Reqnroll/Gherkin tags (builder can't do this — no reflection on type/method)
                ReqnrollTagParser.ApplyTags(result, _scenarioContext.ScenarioInfo.CombinedTags);

                // Set suite from feature title if tags didn't override it
                if (result.Relations?.Suite.Data == null || result.Relations.Suite.Data.Count == 0)
                {
                    result.Relations ??= new Relations();
                    result.Relations.Suite.Data = new List<SuiteData>
                    {
                        new SuiteData { Title = _featureContext.FeatureInfo.Title }
                    };
                }

                // Set title from scenario name if not overridden by tag
                if (string.IsNullOrEmpty(result.Title))
                {
                    result.Title = _scenarioContext.ScenarioInfo.Title;
                }

                // Regenerate signature now that tags may have updated TestopsIds/suite/params
                result.Signature = Signature.Generate(
                    result.TestopsIds,
                    result.Relations?.Suite?.Data?.Select(s => s.Title),
                    result.Params);

                if (string.IsNullOrEmpty(result.Signature))
                {
                    result.Signature = result.Title?.ToLower().Trim().Replace(" ", "-") ?? "unknown";
                }

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
    }
}
