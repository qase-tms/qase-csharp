using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;

namespace Qase.NUnit.Reporter
{
    /// <summary>
    /// NUnit Event Listener for Qase TMS integration
    /// </summary>
    [Extension(Description = "Qase NUnit Event Listener", EngineVersion = "3.4")]
    public class QaseNUnitEventListener : ITestEventListener
    {
        private static readonly object _lockObject = new object();
        private static string? _logFilePath;
        private static ICoreReporter? _reporter;
        private static readonly ConcurrentDictionary<string, TestResult> _testResults = new();

        private static string LogFilePath
        {
            get
            {
                if (_logFilePath == null)
                {
                    var logDirectory = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "logs"
                    );
                    
                    Directory.CreateDirectory(logDirectory);
                    
                    _logFilePath = Path.Combine(
                        logDirectory,
                        $"qase-nunit-{DateTime.Now:yyyyMMdd-HHmmss}.log"
                    );
                }
                
                return _logFilePath;
            }
        }

        private static ICoreReporter Reporter
        {
            get
            {
                if (_reporter == null)
                {
                    _reporter = CoreReporterFactory.GetInstance();
                }
                return _reporter;
            }
        }

        private static void WriteToFile(string message)
        {
            try
            {
                lock (_lockObject)
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var logMessage = $"[{timestamp}] {message}";
                    
                    File.AppendAllText(LogFilePath, logMessage + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                // Fallback to console if file writing fails
                Console.WriteLine($"[Qase] Failed to write to log file: {ex.Message}");
                Console.WriteLine($"[Qase] Original message: {message}");
            }
        }
        /// <summary>
        /// Handles test events from NUnit Engine
        /// </summary>
        /// <param name="report">XML-formatted test event report</param>
        public void OnTestEvent(string report)
        {
            try
            {
                // Log the full XML for debugging (formatted for readability)
                WriteToFile($"[Qase] Raw XML Event:");
                try
                {
                    var xmlDocForLogging = new XmlDocument();
                    xmlDocForLogging.LoadXml(report);
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = new System.Xml.XmlTextWriter(stringWriter)
                        {
                            Formatting = System.Xml.Formatting.Indented,
                            Indentation = 2
                        })
                        {
                            xmlDocForLogging.WriteTo(xmlWriter);
                        }
                        WriteToFile(stringWriter.ToString());
                    }
                }
                catch
                {
                    // If formatting fails, just write raw XML
                    WriteToFile(report);
                }
                WriteToFile("---");

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(report);

                // Determine event type by root element name
                var rootElement = xmlDoc.DocumentElement;
                if (rootElement == null)
                    return;

                var eventType = rootElement.Name;

                switch (eventType)
                {
                    case "start-run":
                        HandleStartRun(rootElement);
                        break;

                    case "test-run":
                        HandleTestRun(rootElement);
                        break;

                    case "start-test":
                        HandleStartTest(rootElement);
                        break;

                    case "test-case":
                        HandleTestCase(rootElement);
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteToFile($"[Qase] Error processing test event: {ex.Message}");
                WriteToFile($"[Qase] XML: {report}");
            }
        }

        private void HandleStartRun(XmlElement element)
        {
            var id = element.GetAttribute("id");
            var testCount = element.GetAttribute("testcasecount");
            var startTime = element.GetAttribute("start-time");

            WriteToFile($"[Qase] Test run started - ID: {id}, Tests: {testCount}, Start: {startTime}");
            
            try
            {
                Reporter.startTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                WriteToFile($"[Qase] Failed to start test run: {ex.Message}");
            }
        }

        private void HandleTestRun(XmlElement element)
        {
            var id = element.GetAttribute("id");
            var result = element.GetAttribute("result");
            var total = element.GetAttribute("total");
            var passed = element.GetAttribute("passed");
            var failed = element.GetAttribute("failed");
            var skipped = element.GetAttribute("skipped");
            var duration = element.GetAttribute("duration");

            WriteToFile($"[Qase] Test run finished - ID: {id}, Result: {result}");
            WriteToFile($"[Qase] Summary - Total: {total}, Passed: {passed}, Failed: {failed}, Skipped: {skipped}, Duration: {duration}s");
            
            try
            {
                Reporter.uploadResults().GetAwaiter().GetResult();
                Reporter.completeTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                WriteToFile($"[Qase] Error completing test run: {ex.Message}");
            }
        }

        private void HandleStartTest(XmlElement element)
        {
            var id = element.GetAttribute("id");
            var name = element.GetAttribute("name");
            var fullName = element.GetAttribute("fullname");

            WriteToFile($"[Qase] Test started - ID: {id}, Name: {name}, FullName: {fullName}");
            
            // Create base test result
            var testResult = CreateBaseTestResult(fullName, name);
            _testResults[id] = testResult;
            
            // Generate display name in QaseAspect format for ContextManager
            var displayName = GenerateDisplayName(fullName, testResult.Params);
            ContextManager.SetTestCaseName(displayName);
        }

        private void HandleTestCase(XmlElement element)
        {
            var id = element.GetAttribute("id");
            var name = element.GetAttribute("name");
            var fullName = element.GetAttribute("fullname");
            var result = element.GetAttribute("result");
            var duration = element.GetAttribute("duration");
            var startTime = element.GetAttribute("start-time");
            var endTime = element.GetAttribute("end-time");

            WriteToFile($"[Qase] Test case finished - ID: {id}, Name: {name}, FullName: {fullName}");
            WriteToFile($"[Qase]   Result: {result}, Duration: {duration}s, Start: {startTime}, End: {endTime}");

            // Get or create test result
            if (!_testResults.TryGetValue(id, out var testResult))
            {
                testResult = CreateBaseTestResult(fullName, name);
            }

            // Update test result with execution data
            UpdateTestResultFromXml(testResult, element, result, duration, startTime, endTime);

            // Generate display name in QaseAspect format for ContextManager
            var displayName = GenerateDisplayName(fullName, testResult.Params);
            
            // Get steps, comments, attachments from ContextManager
            testResult.Steps = ContextManager.GetCompletedSteps(displayName);
            testResult.Message = string.Join("\n", 
                testResult.Message, 
                ContextManager.GetComments(displayName));
            testResult.Attachments = ContextManager.GetAttachments(displayName);

            // Send to Qase if not ignored
            if (!testResult.Ignore)
            {
                try
                {
                    Reporter.addResult(testResult).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    WriteToFile($"[Qase] Failed to add result: {ex.Message}");
                }
            }

            _testResults.TryRemove(id, out _);
        }

        private TestResult CreateBaseTestResult(string fullName, string name)
        {
            // Extract method name without parameters for reflection and title
            var methodNameWithoutParams = ExtractMethodNameWithoutParameters(name);
            
            var result = new TestResult
            {
                // Use method name without parameters as title
                Title = methodNameWithoutParams,
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Skipped,
                    Thread = System.Threading.Thread.CurrentThread.Name ??
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()
                },
                Relations = new Relations
                {
                    Suite = new Suite
                    {
                        Data = ParseSuiteFromFullName(fullName)
                    }
                }
            };

            // Extract attributes from method using reflection
            ExtractAttributesFromFullName(fullName, result);

            // Extract parameters from test name (for parameterized tests)
            // This will populate result.Params with parameter names and values
            ExtractParametersFromTestName(fullName, methodNameWithoutParams, result);

            // Generate signature
            result.Signature = Signature.Generate(
                result.TestopsIds,
                result.Relations?.Suite?.Data?.Select(suite => suite.Title),
                result.Params
            );

            return result;
        }

        /// <summary>
        /// Extracts method name without parameters from test name
        /// Example: "Test2(\"user1\",\"value2\")" -> "Test2"
        /// </summary>
        private string ExtractMethodNameWithoutParameters(string testName)
        {
            if (string.IsNullOrEmpty(testName))
                return testName;

            var openParenIndex = testName.IndexOf('(');
            if (openParenIndex > 0)
            {
                return testName.Substring(0, openParenIndex);
            }

            return testName;
        }

        /// <summary>
        /// Generates display name in QaseAspect format for ContextManager
        /// Format: "Namespace.ClassName.MethodName" or "Namespace.ClassName.MethodName(param1: value1, param2: value2)"
        /// Example: "NunitExamples.Tests.Test1" or "NunitExamples.Tests.Test2(user: user1, value: value2)"
        /// </summary>
        private string GenerateDisplayName(string fullName, Dictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;

            // Strip parameters before splitting to avoid splitting on dots inside parameter values (e.g. "50.0")
            var openParenIndex = fullName.IndexOf('(');
            var nameWithoutParams = openParenIndex > 0 ? fullName.Substring(0, openParenIndex) : fullName;

            var parts = nameWithoutParams.Split('.');
            if (parts.Length < 2)
                return fullName;

            var methodName = parts[parts.Length - 1];

            // Build full class name (namespace + class)
            var className = parts[parts.Length - 2];
            var namespaceParts = parts.Take(parts.Length - 2);
            var fullClassName = string.Join(".", namespaceParts);
            
            // Build display name: FullClassName.MethodName
            var displayName = string.IsNullOrEmpty(fullClassName) 
                ? $"{className}.{methodName}"
                : $"{fullClassName}.{className}.{methodName}";

            // Add parameters if present
            if (parameters != null && parameters.Count > 0)
            {
                var parameterStrings = parameters.Select(kvp => $"{kvp.Key}: {kvp.Value}");
                displayName += $"({string.Join(", ", parameterStrings)})";
            }

            return displayName;
        }

        private List<SuiteData> ParseSuiteFromFullName(string fullName)
        {
            // FullName format: "Namespace.ClassName.MethodName" or "Namespace.ClassName.MethodName(params)"
            // Strip parameters first to avoid splitting on dots inside parameter values (e.g. "50.0")
            var openParenIndex = fullName.IndexOf('(');
            var nameWithoutParams = openParenIndex > 0 ? fullName.Substring(0, openParenIndex) : fullName;

            var parts = nameWithoutParams.Split('.');
            if (parts.Length >= 2)
            {
                // Take all parts except the last one (method name)
                return parts.Take(parts.Length - 1)
                    .Select(part => new SuiteData { Title = part })
                    .ToList();
            }

            return new List<SuiteData>();
        }

        private void ExtractAttributesFromFullName(string fullName, TestResult result)
        {
            try
            {
                // Strip parameters before splitting to avoid splitting on dots inside parameter values (e.g. "50.0")
                var openParenIndex = fullName.IndexOf('(');
                var nameWithoutParams = openParenIndex > 0 ? fullName.Substring(0, openParenIndex) : fullName;

                var parts = nameWithoutParams.Split('.');
                if (parts.Length < 2)
                    return;

                var methodName = parts[parts.Length - 1];
                var className = parts[parts.Length - 2];
                var namespaceName = string.Join(".", parts.Take(parts.Length - 2));
                var fullClassName = string.IsNullOrEmpty(namespaceName) 
                    ? className 
                    : $"{namespaceName}.{className}";

                // Find type
                var type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => 
                    {
                        try
                        {
                            return a.GetTypes();
                        }
                        catch
                        {
                            return Array.Empty<Type>();
                        }
                    })
                    .FirstOrDefault(t => t.FullName == fullClassName || t.Name == className);

                if (type == null)
                    return;

                // Find method (use method name without parameters)
                var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (method == null)
                    return;

                // Extract method-level attributes
                var methodAttributes = method.GetCustomAttributes(typeof(IQaseAttribute), false)
                    .Cast<Attribute>();

                // Extract class-level attributes
                var classAttributes = type.GetCustomAttributes(typeof(IQaseAttribute), false)
                    .Cast<Attribute>();

                // Process attributes
                foreach (var attr in classAttributes.Concat(methodAttributes))
                {
                    switch (attr)
                    {
                        case QaseIdsAttribute qaseIds:
                            result.TestopsIds = qaseIds.Ids;
                            break;

                        case TitleAttribute title:
                            result.Title = title.Title;
                            break;

                        case FieldsAttribute fields:
                            result.Fields.Add(fields.Key, fields.Value);
                            break;

                        case SuitesAttribute suites:
                            result.Relations.Suite.Data = suites.Suites
                                .Select(s => new SuiteData { Title = s })
                                .ToList();
                            break;

                        case IgnoreAttribute:
                            result.Ignore = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile($"[Qase] Error extracting attributes: {ex.Message}");
            }
        }

        private void UpdateTestResultFromXml(TestResult testResult, XmlElement element, string result, string duration, string startTime, string endTime)
        {
            // Map result status
            testResult.Execution.Status = MapResultStatus(result);

            // Set timing
            // NUnit uses ISO 8601 format with Z suffix (UTC)
            if (DateTimeOffset.TryParse(startTime, out var startOffset))
            {
                testResult.Execution.StartTime = startOffset.ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] StartTime parsing - raw: '{startTime}', parsed: {startOffset:O}, unix ms: {testResult.Execution.StartTime}");
            }
            else if (DateTime.TryParse(startTime, out var start))
            {
                // Fallback if DateTimeOffset parsing fails
                testResult.Execution.StartTime = new DateTimeOffset(start, TimeSpan.Zero).ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] StartTime parsing (fallback) - raw: '{startTime}', parsed: {start:O}, unix ms: {testResult.Execution.StartTime}");
            }
            else
            {
                WriteToFile($"[Qase] Failed to parse startTime: '{startTime}'");
            }

            if (DateTimeOffset.TryParse(endTime, out var endOffset))
            {
                testResult.Execution.EndTime = endOffset.ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] EndTime parsing - raw: '{endTime}', parsed: {endOffset:O}, unix ms: {testResult.Execution.EndTime}");
            }
            else if (DateTime.TryParse(endTime, out var end))
            {
                // Fallback if DateTimeOffset parsing fails
                testResult.Execution.EndTime = new DateTimeOffset(end, TimeSpan.Zero).ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] EndTime parsing (fallback) - raw: '{endTime}', parsed: {end:O}, unix ms: {testResult.Execution.EndTime}");
            }
            else
            {
                WriteToFile($"[Qase] Failed to parse endTime: '{endTime}'");
            }

            // Set duration in milliseconds
            // Duration is in seconds (e.g., "0.016632" = 16.632 milliseconds)
            if (double.TryParse(duration, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var durationSeconds))
            {
                testResult.Execution.Duration = (int)Math.Round(durationSeconds * 1000);
                WriteToFile($"[Qase] Duration parsing - raw: '{duration}', parsed seconds: {durationSeconds}, milliseconds: {testResult.Execution.Duration}");
            }
            else
            {
                WriteToFile($"[Qase] Failed to parse duration: '{duration}'");
            }

            // Extract failure information
            if (result == "Failed")
            {
                var failureNode = element.SelectSingleNode("failure");
                if (failureNode != null)
                {
                    var messageNode = failureNode.SelectSingleNode("message");
                    var stackTraceNode = failureNode.SelectSingleNode("stack-trace");

                    testResult.Message = messageNode?.InnerText ?? "";
                    testResult.Execution.Stacktrace = stackTraceNode?.InnerText ?? "";

                    // Determine if assertion failure or error
                    // Use label attribute (Error = Invalid), asserts attribute (0 = Invalid, >0 = Failed), and stack trace
                    var label = element.GetAttribute("label");
                    var assertsAttr = element.GetAttribute("asserts");
                    
                    // Check if asserts attribute exists and parse it
                    int? assertsCount = null;
                    if (!string.IsNullOrEmpty(assertsAttr) && int.TryParse(assertsAttr, out var parsedCount))
                    {
                        assertsCount = parsedCount;
                    }
                    
                    WriteToFile($"[Qase] Failure analysis - label: '{label}', asserts: '{assertsAttr}' (parsed: {assertsCount?.ToString() ?? "null"})");
                    
                    var isAssertionFailure = DetermineFailureType(label, assertsCount, testResult.Execution.Stacktrace ?? "");
                    
                    WriteToFile($"[Qase] Failure type determination - isAssertionFailure: {isAssertionFailure}, final status: {(isAssertionFailure ? "Failed" : "Invalid")}");
                    
                    if (!isAssertionFailure)
                    {
                        testResult.Execution.Status = TestResultStatus.Invalid;
                    }
                }
            }
            else if (result == "Skipped" || result == "Inconclusive")
            {
                var reasonNode = element.SelectSingleNode("reason");
                testResult.Message = reasonNode?.SelectSingleNode("message")?.InnerText ?? "";
            }
        }

        /// <summary>
        /// Extracts parameters from test name for parameterized tests
        /// Example: "Test2(\"user1\",\"value2\")" -> {"value1": "user1", "value2": "value2"}
        /// </summary>
        private void ExtractParametersFromTestName(string fullName, string methodName, TestResult testResult)
        {
            try
            {
                // Strip parameters before splitting to avoid splitting on dots inside parameter values (e.g. "50.0")
                var openParenIndex = fullName.IndexOf('(');
                var nameWithoutParams = openParenIndex > 0 ? fullName.Substring(0, openParenIndex) : fullName;

                var parts = nameWithoutParams.Split('.');
                if (parts.Length < 2)
                    return;

                var className = parts[parts.Length - 2];
                var namespaceName = string.Join(".", parts.Take(parts.Length - 2));
                var fullClassName = string.IsNullOrEmpty(namespaceName) 
                    ? className 
                    : $"{namespaceName}.{className}";

                // Find type
                var type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => 
                    {
                        try
                        {
                            return a.GetTypes();
                        }
                        catch
                        {
                            return Array.Empty<Type>();
                        }
                    })
                    .FirstOrDefault(t => t.FullName == fullClassName || t.Name == className);

                if (type == null)
                    return;

                // Find method (use methodName without parameters)
                var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (method == null)
                    return;

                // Extract parameter values from the original fullName (which still has parameters)
                // Format: "Namespace.Class.MethodName(\"value1\",\"value2\")"
                WriteToFile($"[Qase] Extracting parameters from full name: '{fullName}'");
                var parameterValues = ExtractParameterValuesFromName(fullName);
                
                WriteToFile($"[Qase] Extracted parameter values: [{string.Join(", ", parameterValues)}]");
                
                if (parameterValues == null || parameterValues.Count == 0)
                    return;

                // Get method parameters
                var methodParams = method.GetParameters();
                
                // Map parameter values to parameter names
                for (int i = 0; i < Math.Min(parameterValues.Count, methodParams.Length); i++)
                {
                    var paramName = methodParams[i].Name ?? $"param{i}";
                    var paramValue = parameterValues[i];
                    
                    // Parameter values are already extracted without quotes
                    // Handle null values
                    if (paramValue == "null")
                    {
                        testResult.Params[paramName] = "null";
                    }
                    else
                    {
                        testResult.Params[paramName] = paramValue;
                    }
                }
                
                WriteToFile($"[Qase] Extracted parameters from test name - method: {methodName}, params: {string.Join(", ", testResult.Params.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");
            }
            catch (Exception ex)
            {
                WriteToFile($"[Qase] Error extracting parameters from test name: {ex.Message}");
            }
        }

        /// <summary>
        /// Extracts parameter values from test name
        /// Example: "Test2(\"user1\",\"value2\")" -> ["user1", "value2"]
        /// Handles both escaped quotes (\") and regular quotes (")
        /// </summary>
        private List<string> ExtractParameterValuesFromName(string testName)
        {
            var parameters = new List<string>();
            
            if (string.IsNullOrEmpty(testName))
                return parameters;

            var openParenIndex = testName.IndexOf('(');
            var closeParenIndex = testName.LastIndexOf(')');
            
            if (openParenIndex < 0 || closeParenIndex < 0 || closeParenIndex <= openParenIndex)
                return parameters;

            var paramsString = testName.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();
            
            if (string.IsNullOrEmpty(paramsString))
                return parameters;

            // Parse comma-separated values, handling quoted strings
            // XML may have escaped quotes: \" or regular quotes: "
            var currentParam = new System.Text.StringBuilder();
            var inQuotes = false;
            var escapeNext = false;

            for (int i = 0; i < paramsString.Length; i++)
            {
                var ch = paramsString[i];

                if (escapeNext)
                {
                    // Handle escaped characters
                    if (ch == '"' || ch == '\\')
                    {
                        currentParam.Append(ch);
                    }
                    else
                    {
                        currentParam.Append('\\').Append(ch);
                    }
                    escapeNext = false;
                    continue;
                }

                if (ch == '\\')
                {
                    escapeNext = true;
                    continue;
                }

                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    // Don't include quotes in the parameter value
                    continue;
                }

                if (ch == ',' && !inQuotes)
                {
                    var param = currentParam.ToString().Trim();
                    if (!string.IsNullOrEmpty(param))
                    {
                        parameters.Add(param);
                    }
                    currentParam.Clear();
                    continue;
                }

                currentParam.Append(ch);
            }

            // Add last parameter
            var lastParam = currentParam.ToString().Trim();
            if (!string.IsNullOrEmpty(lastParam))
            {
                parameters.Add(lastParam);
            }

            return parameters;
        }

        private TestResultStatus MapResultStatus(string result)
        {
            return result switch
            {
                "Passed" => TestResultStatus.Passed,
                "Failed" => TestResultStatus.Failed,
                "Skipped" => TestResultStatus.Skipped,
                "Inconclusive" => TestResultStatus.Skipped,
                _ => TestResultStatus.Skipped
            };
        }

        /// <summary>
        /// Determines if the failure is due to an assertion failure (Failed) or other error (Invalid)
        /// </summary>
        /// <param name="label">The label attribute from test-case XML (e.g., "Error")</param>
        /// <param name="assertsCount">Number of assertions that ran (null if attribute not present)</param>
        /// <param name="stackTrace">Stack trace from the failure</param>
        /// <returns>True if assertion failure (Failed), false if error (Invalid)</returns>
        private bool DetermineFailureType(string label, int? assertsCount, string stackTrace)
        {
            // Priority 1: If label is "Error", it's an Invalid status
            if (!string.IsNullOrEmpty(label) && label == "Error")
            {
                return false; // Invalid
            }

            // Priority 2: If asserts attribute exists and equals 0, it's an Invalid status
            if (assertsCount.HasValue && assertsCount.Value == 0)
            {
                return false; // Invalid
            }

            // Priority 3: If asserts > 0, it's likely an assertion failure
            if (assertsCount.HasValue && assertsCount.Value > 0)
            {
                // Check stack trace for assertion methods to confirm
                if (!string.IsNullOrEmpty(stackTrace))
                {
                    // If we find assertion methods, it's definitely Failed
                    if (stackTrace.Contains("at NUnit.Framework.Assert") ||
                        stackTrace.Contains("at NUnit.Framework.Constraints") ||
                        stackTrace.Contains("Assert.That") ||
                        stackTrace.Contains("Assert.AreEqual") ||
                        stackTrace.Contains("Assert.IsTrue") ||
                        stackTrace.Contains("Assert.IsFalse") ||
                        stackTrace.Contains("Assert.AreNotEqual") ||
                        stackTrace.Contains("Assert.IsNull") ||
                        stackTrace.Contains("Assert.IsNotNull"))
                    {
                        return true; // Failed - assertion failure
                    }
                }
                
                // If we have assertions (asserts > 0) but can't find assertion methods in stack trace,
                // still assume it's a Failed (assertion failure) since asserts > 0
                return true; // Failed
            }

            // Priority 4: Fallback - check stack trace for assertion methods
            if (!string.IsNullOrEmpty(stackTrace))
            {
                if (stackTrace.Contains("at NUnit.Framework.Assert") ||
                    stackTrace.Contains("at NUnit.Framework.Constraints") ||
                    stackTrace.Contains("Assert.That") ||
                    stackTrace.Contains("Assert.AreEqual") ||
                    stackTrace.Contains("Assert.IsTrue") ||
                    stackTrace.Contains("Assert.IsFalse"))
                {
                    return true; // Failed - assertion failure
                }
            }

            // Priority 5: Default to Failed if we can't determine (assume assertion failure by default)
            // This is safer than defaulting to Invalid
            return true; // Failed
        }
    }
}
