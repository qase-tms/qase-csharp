using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using Qase.Csharp.Commons;
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
        private static ITestResultBuilder _builder = new TestResultBuilder();
        private static readonly ConcurrentDictionary<string, RawTestData> _rawTestData = new();

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

            // Create RawTestData from fullName; builder will resolve type/method/attrs/params
            var raw = new RawTestData
            {
                FullTestName = fullName,
                DisplayName = name,
                Status = TestResultStatus.Skipped,
                Thread = System.Threading.Thread.CurrentThread.Name ??
                    System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()
            };

            _rawTestData[id] = raw;

            // Generate display name for ContextManager WITHOUT calling builder.Build()
            // (Build() consumes ContextManager data, which would lose steps)
            var methodBaseName = ExtractMethodBaseFromFullName(fullName);
            var parameters = ResolveParametersForDisplayName(fullName, name);
            var displayName = DisplayNameGenerator.Generate(methodBaseName, parameters);
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

            // Get or create raw test data
            if (!_rawTestData.TryGetValue(id, out var raw))
            {
                raw = new RawTestData
                {
                    FullTestName = fullName,
                    DisplayName = name,
                    Status = TestResultStatus.Skipped,
                    Thread = System.Threading.Thread.CurrentThread.Name ??
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()
                };
            }

            // Map result status
            raw.Status = MapResultStatus(result);

            // Set timing
            if (DateTimeOffset.TryParse(startTime, out var startOffset))
            {
                raw.StartTime = startOffset.ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] StartTime parsing - raw: '{startTime}', parsed: {startOffset:O}, unix ms: {raw.StartTime}");
            }
            else if (DateTime.TryParse(startTime, out var start))
            {
                raw.StartTime = new DateTimeOffset(start, TimeSpan.Zero).ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] StartTime parsing (fallback) - raw: '{startTime}', parsed: {start:O}, unix ms: {raw.StartTime}");
            }
            else
            {
                WriteToFile($"[Qase] Failed to parse startTime: '{startTime}'");
            }

            if (DateTimeOffset.TryParse(endTime, out var endOffset))
            {
                raw.EndTime = endOffset.ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] EndTime parsing - raw: '{endTime}', parsed: {endOffset:O}, unix ms: {raw.EndTime}");
            }
            else if (DateTime.TryParse(endTime, out var end))
            {
                raw.EndTime = new DateTimeOffset(end, TimeSpan.Zero).ToUnixTimeMilliseconds();
                WriteToFile($"[Qase] EndTime parsing (fallback) - raw: '{endTime}', parsed: {end:O}, unix ms: {raw.EndTime}");
            }
            else
            {
                WriteToFile($"[Qase] Failed to parse endTime: '{endTime}'");
            }

            if (double.TryParse(duration, NumberStyles.Float, CultureInfo.InvariantCulture, out var durationSeconds))
            {
                raw.Duration = (int)Math.Round(durationSeconds * 1000);
                WriteToFile($"[Qase] Duration parsing - raw: '{duration}', parsed seconds: {durationSeconds}, milliseconds: {raw.Duration}");
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

                    raw.ErrorMessage = messageNode?.InnerText ?? "";
                    raw.StackTrace = stackTraceNode?.InnerText ?? "";

                    var label = element.GetAttribute("label");
                    var assertsAttr = element.GetAttribute("asserts");

                    int? assertsCount = null;
                    if (!string.IsNullOrEmpty(assertsAttr) && int.TryParse(assertsAttr, out var parsedCount))
                    {
                        assertsCount = parsedCount;
                    }

                    WriteToFile($"[Qase] Failure analysis - label: '{label}', asserts: '{assertsAttr}' (parsed: {assertsCount?.ToString() ?? "null"})");

                    raw.FailureLabel = label;
                    raw.AssertsCount = assertsCount;
                }
            }
            else if (result == "Skipped" || result == "Inconclusive")
            {
                var reasonNode = element.SelectSingleNode("reason");
                raw.ErrorMessage = reasonNode?.SelectSingleNode("message")?.InnerText ?? "";
            }

            var testResult = _builder.Build(raw);

            WriteToFile($"[Qase] Final status: {testResult.Execution?.Status}");

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

            _rawTestData.TryRemove(id, out _);
        }

        private static TestResultStatus MapResultStatus(string result)
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
        /// Resolves parameter names and values from a NUnit full test name for ContextManager display name.
        /// Uses TypeMethodResolver to get method parameter names and ParameterParser to extract values.
        /// Does NOT call TestResultBuilder.Build() to avoid consuming ContextManager data.
        /// </summary>
        private static Dictionary<string, string>? ResolveParametersForDisplayName(string fullName, string testName)
        {
            // Extract values from fullName
            var values = ParameterParser.ParseValues(fullName);
            if (values.Count == 0)
                return null;

            // Resolve type and method to get parameter names
            var openParen = fullName.IndexOf('(');
            var nameWithoutParams = openParen > 0 ? fullName.Substring(0, openParen) : fullName;
            var parts = nameWithoutParams.Split('.');
            if (parts.Length < 2)
                return null;

            var methodName = parts[parts.Length - 1];
            var className = parts[parts.Length - 2];
            var namespaceName = string.Join(".", parts.Take(parts.Length - 2));
            var fullClassName = string.IsNullOrEmpty(namespaceName) ? className : $"{namespaceName}.{className}";

            var type = TypeMethodResolver.ResolveType(fullClassName);
            if (type == null)
                return null;

            var method = TypeMethodResolver.ResolveMethod(type, methodName);
            if (method == null)
                return null;

            var methodParams = method.GetParameters();
            var result = new Dictionary<string, string>();
            for (int i = 0; i < Math.Min(values.Count, methodParams.Length); i++)
            {
                var paramName = methodParams[i].Name ?? $"param{i}";
                var paramValue = values[i];
                if (paramValue == "null")
                    result[paramName] = "null";
                else if (string.IsNullOrEmpty(paramValue))
                    result[paramName] = "empty";
                else
                    result[paramName] = paramValue;
            }

            return result.Count > 0 ? result : null;
        }

        /// <summary>
        /// Extracts the "Namespace.ClassName.MethodName" portion from a NUnit full test name,
        /// stripping any parameter parentheses. Used as the base for ContextManager display name.
        /// </summary>
        private static string ExtractMethodBaseFromFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;

            var openParenIndex = fullName.IndexOf('(');
            return openParenIndex > 0 ? fullName.Substring(0, openParenIndex) : fullName;
        }
    }
}
