using System;
using System.Collections.Generic;
using System.Reflection;
using AspectInjector.Broker;
using Qase.Csharp.Commons.Attributes;

namespace Qase.Csharp.Commons.Aspects
{
    /// <summary>
    /// Aspect for enabling Qase feature, like steps, attachments, etc.
    /// </summary>
    [Aspect(Scope.Global)]
    public class QaseAspect
    {
        /// <summary>
        /// Advice for enabling Qase feature, like steps, attachments, etc.
        /// </summary>
        [Advice(Kind.Around)]
        public object Around([Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.ReturnType)] Type returnType)
        {
            // Check if the method has QaseFeature attribute
            if (metadata.GetCustomAttribute<QaseAttribute>() != null)
            {
                // Generate test case display name using method metadata and arguments
                var testCaseName = GenerateTestCaseDisplayName(metadata, args);

                if (!string.IsNullOrEmpty(testCaseName))
                {
                    // Set the test case name in the context manager
                    ContextManager.SetTestCaseName(testCaseName);
                }
            }

            return target(args);
        }

        /// <summary>
        /// Generates the test case display name using method metadata and arguments.
        /// This mimics the xUnit display name format including parameters for parameterized tests.
        /// </summary>
        /// <param name="metadata">The method metadata</param>
        /// <param name="args">The method arguments</param>
        /// <returns>The test case display name</returns>
        private static string GenerateTestCaseDisplayName(MethodBase metadata, object[] args)
        {
            var classType = metadata.DeclaringType;
            var className = classType?.Name ?? "";
            var methodName = metadata.Name;

            // Get the full namespace + class name to match xUnit format
            var fullClassName = classType?.FullName ?? className;

            // Base display name with full namespace
            var displayName = $"{fullClassName}.{methodName}";

            // Add parameters if this is a parameterized test
            if (args != null && args.Length > 0)
            {
                var parameters = metadata.GetParameters();
                var parameterValues = new List<string>();

                for (int i = 0; i < Math.Min(args.Length, parameters.Length); i++)
                {
                    var parameterName = parameters[i].Name ?? $"param{i}";
                    var parameterValue = args[i]?.ToString() ?? "null";

                    // For simple types, just use the value
                    // For complex types, use a more descriptive representation
                    if (args[i] != null && !args[i].GetType().IsPrimitive && args[i].GetType() != typeof(string))
                    {
                        parameterValue = $"[{args[i].GetType().Name}]";
                    }

                    parameterValues.Add($"{parameterName}: {parameterValue}");
                }

                if (parameterValues.Count > 0)
                {
                    displayName += $"({string.Join(", ", parameterValues)})";
                }
            }

            return displayName;
        }
    }
}
