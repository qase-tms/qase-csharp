using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons
{
    public static class StepManager
    {
        private static readonly Stack<StepResult> _stepStack = new();
        private static List<StepResult> _completedSteps = new();

        public static void StartStep(string title, Action<StepResult>? configure = null)
        {
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

            if (_stepStack.Count > 0)
            {
                var parentStep = _stepStack.Peek();
                parentStep.Steps ??= new List<StepResult>();
                parentStep.Steps.Add(step);
            }

            _stepStack.Push(step);
        }

        public static void PassStep()
        {
            if (_stepStack.Count == 0)
            {
                throw new InvalidOperationException("No active step to pass");
            }

            var step = _stepStack.Pop();
            step.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            step.Execution!.Status = StepResultStatus.Passed;

            if (_stepStack.Count == 0)
            {
                var rootStep = step;
                while (rootStep.Steps?.Count > 0)
                {
                    rootStep = rootStep.Steps[0];
                }
                _completedSteps = new List<StepResult> { rootStep };
            }
        }

        public static void FailStep()
        {
            if (_stepStack.Count == 0)
            {
                throw new InvalidOperationException("No active step to fail");
            }

            var step = _stepStack.Pop();
            step.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            step.Execution!.Status = StepResultStatus.Failed;

            if (_stepStack.Count == 0)
            {
                var rootStep = step;
                while (rootStep.Steps?.Count > 0)
                {
                    rootStep = rootStep.Steps[0];
                }
                _completedSteps = new List<StepResult> { rootStep };
            }
        }

        public static List<StepResult> GetCompletedSteps()
        {
            var steps = _completedSteps.ToList();
            Clear();
            return steps;
        }

        public static void Clear()
        {
            _stepStack.Clear();
            _completedSteps.Clear();
        }
    }
}
