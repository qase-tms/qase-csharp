using System;
using System.Collections.Generic;
using System.Threading;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons
{
    public static class StepManager
    {
        private static readonly AsyncLocal<string> _testCaseName = new();
        private static readonly Dictionary<string, Stack<StepResult>> _stepStacks = new();
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
            if (!string.IsNullOrEmpty(_testCaseName.Value) && _stepStacks.ContainsKey(_testCaseName.Value))
            {
                _stepStacks[_testCaseName.Value].Clear();
            }
        }

        public static void SaveSteps()
        {
            Clear();
        }
    }
}
