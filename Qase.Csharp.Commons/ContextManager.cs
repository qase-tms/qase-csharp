using System;
using System.Collections.Generic;
using System.Threading;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons
{
    public static class ContextManager
    {
        private static readonly AsyncLocal<string> _testCaseName = new();
        private static readonly Dictionary<string, Stack<StepResult>> _stepStacks = new();
        private static readonly Dictionary<string, List<string>> _messages = new();
        private static readonly Dictionary<string, List<Attachment>> _attachments = new();
        private static Dictionary<string, List<StepResult>> _completedSteps = new();

        public static void SetTestCaseName(string name)
        {
            _testCaseName.Value = name;
            if (!_stepStacks.ContainsKey(name))
            {
                _stepStacks[name] = new Stack<StepResult>();
            }
        }

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

            var stack = _stepStacks[_testCaseName.Value];
            if (stack.Count > 0)
            {
                var parentStep = stack.Peek();
                parentStep.Steps ??= new List<StepResult>();
                parentStep.Steps.Add(step);
            }

            stack.Push(step);
        }

        public static void PassStep()
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            var stack = _stepStacks[_testCaseName.Value];
            if (stack.Count == 0)
            {
                throw new InvalidOperationException("No active step to pass");
            }

            var step = stack.Pop();
            step.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            step.Execution!.Status = StepResultStatus.Passed;

            if (stack.Count == 0)
            {
                if (!_completedSteps.ContainsKey(_testCaseName.Value))
                {
                    _completedSteps[_testCaseName.Value] = new List<StepResult>();
                }

                _completedSteps[_testCaseName.Value].Add(step);
            }
        }

        public static void FailStep()
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            var stack = _stepStacks[_testCaseName.Value];
            if (stack.Count == 0)
            {
                throw new InvalidOperationException("No active step to fail");
            }

            var step = stack.Pop();
            step.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            step.Execution!.Status = StepResultStatus.Failed;

            if (stack.Count == 0)
            {
                if (!_completedSteps.ContainsKey(_testCaseName.Value))
                {
                    _completedSteps[_testCaseName.Value] = new List<StepResult>();
                }

                _completedSteps[_testCaseName.Value].Add(step);
            }
        }

        public static List<StepResult> GetCompletedSteps(string testCaseName)
        {
            if (!_completedSteps.ContainsKey(testCaseName))
            {
                return new List<StepResult>();
            }

            var steps = _completedSteps[testCaseName];
            _completedSteps.Remove(testCaseName);
            return steps;
        }

        public static void Clear()
        {
            _testCaseName.Value = null;
            _stepStacks.Clear();
            _completedSteps.Clear();
            _messages.Clear();
            _attachments.Clear();
        }

        public static void SaveSteps()
        {
            Clear();
        }

        internal static void AddComment(string comment)
        {
            if (string.IsNullOrEmpty(_testCaseName.Value))
            {
                return;
            }

            if (!_messages.ContainsKey(_testCaseName.Value))
            {
                _messages[_testCaseName.Value] = new List<string>();
            }

            _messages[_testCaseName.Value].Add(comment);
        }

        public static string GetComments(string testCaseName)
        {
            if (!_messages.ContainsKey(testCaseName))
            {
                return string.Empty;
            }

            return string.Join("\n", _messages[testCaseName]);
        }

        internal static void AddAttachment(List<string> paths)
        {
            foreach (var path in paths)
            {
                var attachment = new Attachment
                {
                    FilePath = path
                };

                AddAttachment(attachment);
            }
            ;
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

            if (_stepStacks.ContainsKey(_testCaseName.Value) && _stepStacks[_testCaseName.Value].Count > 0)
            {
                var step = _stepStacks[_testCaseName.Value].Peek();
                step.Execution!.Attachments.Add(attachment);
                return;
            }

            if (!_attachments.ContainsKey(_testCaseName.Value))
            {
                _attachments[_testCaseName.Value] = new List<Attachment>();
            }

            _attachments[_testCaseName.Value].Add(attachment);
        }

        public static List<Attachment> GetAttachments(string testCaseName)
        {
            if (!_attachments.ContainsKey(testCaseName))
            {
                return new List<Attachment>();
            }

            return _attachments[testCaseName];
        }
    }
}
