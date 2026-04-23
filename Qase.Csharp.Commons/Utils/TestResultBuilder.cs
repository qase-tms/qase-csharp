#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Assembles a TestResult from raw test data provided by a reporter.
    /// Orchestrates the full pipeline: type resolution, attributes, params,
    /// suite hierarchy, failure classification, ContextManager, signature.
    /// </summary>
    public class TestResultBuilder : ITestResultBuilder
    {
        /// <summary>
        /// Builds a complete TestResult from raw test data.
        /// </summary>
        public TestResult Build(RawTestData raw)
        {
            var result = new TestResult
            {
                Title = raw.DisplayName,
                Execution = new TestResultExecution
                {
                    Status = raw.Status,
                    StartTime = raw.StartTime,
                    EndTime = raw.EndTime,
                    Duration = raw.Duration,
                    Thread = raw.Thread
                },
                Message = raw.ErrorMessage
            };

            if (raw.StackTrace != null)
            {
                result.Execution.Stacktrace = raw.StackTrace;
            }

            // Resolve suite hierarchy
            if (raw.FullTypeName != null)
            {
                result.Relations = new Relations
                {
                    Suite = new Suite
                    {
                        Data = SuiteParser.FromTypeName(raw.FullTypeName)
                    }
                };
            }
            else if (raw.FullTestName != null)
            {
                result.Relations = new Relations
                {
                    Suite = new Suite
                    {
                        Data = SuiteParser.FromFullTestName(raw.FullTestName)
                    }
                };
            }

            // Resolve type and method for attribute extraction and parameter mapping
            Type? type = null;
            MethodInfo? method = null;

            if (raw.FullTypeName != null && raw.MethodName != null)
            {
                type = TypeMethodResolver.ResolveType(raw.FullTypeName);
                if (type != null)
                {
                    method = TypeMethodResolver.ResolveMethod(type, raw.MethodName, raw.ParameterTypeFullNames);
                }
            }
            else if (raw.FullTestName != null)
            {
                (type, method) = ResolveFromFullTestName(raw.FullTestName);
            }

            // Extract Qase attributes
            if (type != null)
            {
                var classAttributes = type.GetCustomAttributes(typeof(IQaseAttribute), false).Cast<Attribute>();
                var methodAttributes = method != null
                    ? method.GetCustomAttributes(typeof(IQaseAttribute), false).Cast<Attribute>()
                    : Enumerable.Empty<Attribute>();
                AttributeExtractor.Apply(classAttributes, methodAttributes, result);
            }

            // Set parameters
            if (raw.PreParsedParams != null && raw.PreParsedParams.Count > 0)
            {
                foreach (var kvp in raw.PreParsedParams)
                {
                    result.Params[kvp.Key] = string.IsNullOrEmpty(kvp.Value) ? "empty" : kvp.Value;
                }
            }
            else if (raw.FullTestName != null && method != null)
            {
                var values = ParameterParser.ParseValues(raw.FullTestName);
                if (values.Count > 0)
                {
                    var methodParams = method.GetParameters();
                    for (int i = 0; i < Math.Min(values.Count, methodParams.Length); i++)
                    {
                        var paramName = methodParams[i].Name ?? $"param{i}";
                        var paramValue = values[i];
                        if (paramValue == "null")
                            result.Params[paramName] = "null";
                        else if (string.IsNullOrEmpty(paramValue))
                            result.Params[paramName] = "empty";
                        else
                            result.Params[paramName] = paramValue;
                    }
                }
            }
            else if (method != null)
            {
                var parsedParams = ParameterParser.ParseAndMap(raw.DisplayName, method);
                foreach (var kvp in parsedParams)
                {
                    result.Params[kvp.Key] = kvp.Value;
                }
            }

            // If Title was not overridden by attribute and params exist, strip params from title
            if (result.Title == raw.DisplayName && result.Params.Count > 0 && raw.MethodName != null)
            {
                result.Title = raw.MethodName;
            }
            else if (result.Title == raw.DisplayName && result.Params.Count > 0 && raw.FullTestName != null)
            {
                result.Title = ExtractMethodNameFromFullTestName(raw.FullTestName);
            }

            // Classify failure (only for Failed status — Passed/Skipped/Invalid stay as-is)
            if (raw.Status == TestResultStatus.Failed)
            {
                result.Execution.Status = FailureClassifier.Classify(
                    raw.FailureLabel,
                    raw.AssertsCount,
                    raw.ExceptionTypes,
                    raw.StackTrace);
            }

            // Collect ContextManager data
            var contextBaseName = raw.ContextDisplayNameBase;
            if (contextBaseName == null && raw.FullTypeName != null && raw.MethodName != null)
            {
                contextBaseName = $"{raw.FullTypeName}.{raw.MethodName}";
            }
            else if (contextBaseName == null && raw.FullTestName != null)
            {
                var openParen = raw.FullTestName.IndexOf('(');
                contextBaseName = openParen > 0
                    ? raw.FullTestName.Substring(0, openParen)
                    : raw.FullTestName;
            }
            else if (contextBaseName == null)
            {
                contextBaseName = raw.DisplayName;
            }

            var contextDisplayName = DisplayNameGenerator.Generate(contextBaseName, result.Params);
            result.Steps = ContextManager.GetCompletedSteps(contextDisplayName);

            var comments = ContextManager.GetComments(contextDisplayName);
            if (!string.IsNullOrEmpty(comments))
            {
                result.Message = string.IsNullOrEmpty(result.Message)
                    ? comments
                    : string.Join("\n", result.Message, comments);
            }

            result.Attachments = ContextManager.GetAttachments(contextDisplayName);

            // Merge pre-collected steps (e.g. Reqnroll BDD steps)
            if (raw.PreCollectedSteps != null && raw.PreCollectedSteps.Count > 0)
            {
                result.Steps.AddRange(raw.PreCollectedSteps);
            }

            // Generate signature
            result.Signature = Signature.Generate(
                result.TestopsIds,
                result.Relations?.Suite?.Data?.Select(s => s.Title),
                result.Params);

            if (string.IsNullOrEmpty(result.Signature))
            {
                result.Signature = result.Title?.ToLower().Trim().Replace(" ", "-") ?? "unknown";
            }

            return result;
        }

        private static (Type? type, MethodInfo? method) ResolveFromFullTestName(string fullTestName)
        {
            var openParenIndex = fullTestName.IndexOf('(');
            var nameWithoutParams = openParenIndex > 0
                ? fullTestName.Substring(0, openParenIndex)
                : fullTestName;

            var parts = nameWithoutParams.Split('.');
            if (parts.Length < 2)
                return (null, null);

            var methodName = parts[parts.Length - 1];
            var className = parts[parts.Length - 2];
            var namespaceName = string.Join(".", parts.Take(parts.Length - 2));
            var fullClassName = string.IsNullOrEmpty(namespaceName)
                ? className
                : $"{namespaceName}.{className}";

            var type = TypeMethodResolver.ResolveType(fullClassName);
            if (type == null)
                return (null, null);

            var method = TypeMethodResolver.ResolveMethod(type, methodName);
            return (type, method);
        }

        private static string ExtractMethodNameFromFullTestName(string fullTestName)
        {
            var openParenIndex = fullTestName.IndexOf('(');
            var nameWithoutParams = openParenIndex > 0
                ? fullTestName.Substring(0, openParenIndex)
                : fullTestName;

            var parts = nameWithoutParams.Split('.');
            return parts.Length > 0 ? parts[parts.Length - 1] : fullTestName;
        }
    }
}
