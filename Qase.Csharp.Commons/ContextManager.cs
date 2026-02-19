using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons
{
    /// <summary>
    /// Context manager for Qase.
    /// </summary>
    public static class ContextManager
    {
        private static readonly AsyncLocal<string> _testCaseName = new();
        private static readonly ConcurrentDictionary<string, Stack<StepResult>> _stepStacks = new();
        private static readonly ConcurrentDictionary<string, List<string>> _messages = new();
        private static readonly ConcurrentDictionary<string, List<Attachment>> _attachments = new();
        private static readonly ConcurrentDictionary<string, List<StepResult>> _completedSteps = new();

        /// <summary>
        /// Set the test case name.
        /// </summary>
        /// <param name="name">The name of the test case.</param>
        public static void SetTestCaseName(string name)
        {
            _testCaseName.Value = name;
            _stepStacks.GetOrAdd(name, _ => new Stack<StepResult>());
        }

        /// <summary>
        /// Start a step.
        /// </summary>
        /// <param name="title">The title of the step.</param>
        /// <param name="configure">The configure action.</param>
        public static void StartStep(string title, Action<StepResult>? configure = null)
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            var step = new StepResult
            {
                Data = new Data
                {
                    Action = title
                },
                Execution = new StepExecution
                {
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                }
            };

            configure?.Invoke(step);

            if (_stepStacks.TryGetValue(_testCaseName.Value, out var stack))
            {
                if (stack.Count > 0)
                {
                    var parentStep = stack.Peek();
                    parentStep.Steps ??= new List<StepResult>();
                    parentStep.Steps.Add(step);
                }

                stack.Push(step);
            }
        }

        /// <summary>
        /// Pass a step.
        /// </summary>
        public static void PassStep()
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            if (!_stepStacks.TryGetValue(_testCaseName.Value, out var stack) || stack.Count == 0)
            {
                throw new InvalidOperationException("No active step to pass");
            }

            var step = stack.Pop();
            step.Execution!.Stop();
            step.Execution!.Status = StepResultStatus.Passed;

            if (stack.Count == 0)
            {
                var steps = _completedSteps.GetOrAdd(_testCaseName.Value, _ => new List<StepResult>());
                steps.Add(step);
            }
        }

        /// <summary>
        /// Fail a step.
        /// </summary>
        public static void FailStep()
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            if (!_stepStacks.TryGetValue(_testCaseName.Value, out var stack) || stack.Count == 0)
            {
                throw new InvalidOperationException("No active step to fail");
            }

            var step = stack.Pop();
            step.Execution!.Stop();
            step.Execution!.Status = StepResultStatus.Failed;

            if (stack.Count == 0)
            {
                var steps = _completedSteps.GetOrAdd(_testCaseName.Value, _ => new List<StepResult>());
                steps.Add(step);
            }
        }

        /// <summary>
        /// Get the completed steps.
        /// </summary>
        /// <param name="testCaseName">The name of the test case.</param>
        /// <returns>The completed steps.</returns>
        public static List<StepResult> GetCompletedSteps(string testCaseName)
        {
            if (_completedSteps.TryRemove(testCaseName, out var steps))
            {
                return steps;
            }

            return new List<StepResult>();
        }

        /// <summary>
        /// Clear the context.
        /// </summary>
        public static void Clear()
        {
            _testCaseName.Value = null;
            _stepStacks.Clear();
            _completedSteps.Clear();
            _messages.Clear();
            _attachments.Clear();
        }

        /// <summary>
        /// Save the steps.
        /// </summary>
        public static void SaveSteps()
        {
            Clear();
        }

        /// <summary>
        /// Add a comment to the test case.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        internal static void AddComment(string comment)
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            var list = _messages.GetOrAdd(_testCaseName.Value, _ => new List<string>());
            list.Add(comment);
        }

        /// <summary>
        /// Get the comments for the test case.
        /// </summary>
        /// <param name="testCaseName">The name of the test case.</param>
        /// <returns>The comments for the test case.</returns>
        public static string GetComments(string testCaseName)
        {
            if (_messages.TryGetValue(testCaseName, out var messages))
            {
                return string.Join("\n", messages);
            }

            return string.Empty;
        }

        /// <summary>
        /// Add attachments to the test case.
        /// </summary>
        /// <param name="paths">The paths to the attachments.</param>
        internal static void AddAttachment(List<string> paths)
        {
            foreach (var path in paths)
            {
                var attachment = new Attachment
                {
                    FilePath = path,
                    FileName = System.IO.Path.GetFileName(path)
                };

                AddAttachment(attachment);
            }
        }

        internal static void AddAttachment(byte[] data, string fileName)
        {
            var attachment = new Attachment
            {
                ContentBytes = data,
                FileName = fileName
            };

            AddAttachment(attachment);
        }

        private static void AddAttachment(Attachment attachment)
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            if (_stepStacks.TryGetValue(_testCaseName.Value, out var stack) && stack.Count > 0)
            {
                var step = stack.Peek();
                step.Execution!.Attachments.Add(attachment);
                return;
            }

            var list = _attachments.GetOrAdd(_testCaseName.Value, _ => new List<Attachment>());
            list.Add(attachment);
        }

        /// <summary>
        /// Get the attachments for the test case.
        /// </summary>
        /// <param name="testCaseName">The name of the test case.</param>
        /// <returns>The attachments for the test case.</returns>
        public static List<Attachment> GetAttachments(string testCaseName)
        {
            if (_attachments.TryGetValue(testCaseName, out var attachments))
            {
                return attachments;
            }

            return new List<Attachment>();
        }
    }
}
