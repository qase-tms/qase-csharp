using System;
using System.Collections.Generic;
using System.Reflection;
using AspectInjector.Broker;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Utils;

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
            [Argument(Source.Instance)] object? instance,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.ReturnType)] Type returnType)
        {
            // Check if the method has QaseFeature attribute
            if (metadata.GetCustomAttribute<QaseAttribute>() != null)
            {
                // Generate test case display name using method metadata and arguments
                var testCaseName = GenerateTestCaseDisplayName(metadata, instance, args);

                if (!string.IsNullOrEmpty(testCaseName))
                {
                    ContextManager.BeginTestCase(testCaseName);
                }
            }

            return target(args);
        }

        /// <summary>
        /// Builds the same key the result builder derives from the framework's report:
        /// the runtime class (an inherited test belongs to the derived fixture) plus the arguments.
        /// </summary>
        private static string GenerateTestCaseDisplayName(MethodBase metadata, object? instance, object[] args)
        {
            var classType = instance?.GetType() ?? metadata.DeclaringType;
            var displayName = $"{classType?.FullName ?? classType?.Name ?? ""}.{metadata.Name}";

            if (args == null || args.Length == 0)
                return displayName;

            var parameters = metadata.GetParameters();
            var values = new Dictionary<string, string>();
            for (int i = 0; i < Math.Min(args.Length, parameters.Length); i++)
            {
                values[parameters[i].Name ?? $"param{i}"] = DisplayNameGenerator.FormatArgument(args[i]);
            }

            return DisplayNameGenerator.Generate(displayName, values);
        }
    }
}
