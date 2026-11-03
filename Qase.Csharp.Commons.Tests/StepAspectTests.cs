using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    [Collection("ContextManager")]
    public class StepAspectTests
    {
        private class WrapperException : Exception
        {
            public WrapperException(Exception inner) : base("wrapper", inner) { }
        }

        private static bool _valueTaskStepFinished;

        [Step]
        private static async ValueTask SlowValueTaskStep()
        {
            await Task.Delay(50);
            _valueTaskStepFinished = true;
        }

        [Step]
        private static async ValueTask<int> SlowValueTaskStepWithResult()
        {
            await Task.Delay(10);
            return 42;
        }

        [Step]
        private static void ThrowingStep()
        {
            throw new WrapperException(new IOException("disk"));
        }

        [Fact]
        public async Task ValueTaskStep_ShouldBeCompletedOnlyAfterItFinishes()
        {
            var key = $"Step.ValueTask.{Guid.NewGuid():N}";
            ContextManager.BeginTestCase(key);
            _valueTaskStepFinished = false;

            await SlowValueTaskStep();
            var result = await SlowValueTaskStepWithResult();

            _valueTaskStepFinished.Should().BeTrue();
            result.Should().Be(42);
            var steps = ContextManager.GetCompletedSteps(key);
            steps.Should().HaveCount(2);
            steps.Should().OnlyContain(s => s.Execution!.Status == StepResultStatus.Passed);
            // A step closed when the ValueTask was returned would last ~0 ms instead of the 50 ms delay.
            steps[0].Execution!.Duration.Should().BeGreaterThanOrEqualTo(40);
        }

        [Fact]
        public void ThrowingStep_ShouldRethrowTheStepsOwnException()
        {
            ContextManager.BeginTestCase($"Step.Throw.{Guid.NewGuid():N}");

            Action act = ThrowingStep;

            act.Should().Throw<WrapperException>().WithInnerException<IOException>();
        }
    }
}
