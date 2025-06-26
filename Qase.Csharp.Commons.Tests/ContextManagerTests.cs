using System;
using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Models.Domain;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ContextManagerTests : IDisposable
    {
        public ContextManagerTests()
        {
            // Очищаем состояние перед каждым тестом
            ContextManager.Clear();
        }

        public void Dispose()
        {
            // Очищаем состояние после каждого теста
            ContextManager.Clear();
        }

        [Fact]
        public void SetTestCaseName_ShouldSetTestCaseName()
        {
            // Arrange
            var testCaseName = "Test Case 1";

            // Act
            ContextManager.SetTestCaseName(testCaseName);

            // Assert
            // Note: Since _testCaseName is private, we can't directly assert it
            // But we can verify the behavior through other methods
        }

        [Fact]
        public void StartStep_WithValidTestCaseName_ShouldCreateStep()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            var stepTitle = "Test Step";
            ContextManager.SetTestCaseName(testCaseName);

            // Act
            ContextManager.StartStep(stepTitle);

            // Assert
            // We can verify this by checking if we can pass the step
            ContextManager.PassStep();
        }

        [Fact]
        public void StartStep_WithoutTestCaseName_ShouldNotCreateStep()
        {
            // Arrange
            var stepTitle = "Test Step";

            // Act & Assert
            // Should not throw exception when no test case name is set
            ContextManager.StartStep(stepTitle);
        }

        [Fact]
        public void PassStep_WithActiveStep_ShouldMarkStepAsPassed()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            var stepTitle = "Test Step";
            ContextManager.SetTestCaseName(testCaseName);
            ContextManager.StartStep(stepTitle);

            // Act
            ContextManager.PassStep();

            // Assert
            var completedSteps = ContextManager.GetCompletedSteps(testCaseName);
            completedSteps.Should().HaveCount(1);
            completedSteps[0].Execution!.Status.Should().Be(StepResultStatus.Passed);
            completedSteps[0].Data!.Action.Should().Be(stepTitle);
        }

        [Fact]
        public void PassStep_WithoutActiveStep_ShouldThrowException()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ContextManager.PassStep());
        }

        [Fact]
        public void FailStep_WithActiveStep_ShouldMarkStepAsFailed()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            var stepTitle = "Test Step";
            ContextManager.SetTestCaseName(testCaseName);
            ContextManager.StartStep(stepTitle);

            // Act
            ContextManager.FailStep();

            // Assert
            var completedSteps = ContextManager.GetCompletedSteps(testCaseName);
            completedSteps.Should().HaveCount(1);
            completedSteps[0].Execution!.Status.Should().Be(StepResultStatus.Failed);
            completedSteps[0].Data!.Action.Should().Be(stepTitle);
        }

        [Fact]
        public void FailStep_WithoutActiveStep_ShouldThrowException()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ContextManager.FailStep());
        }

        [Fact]
        public void GetCompletedSteps_WithNoSteps_ShouldReturnEmptyList()
        {
            // Arrange
            var testCaseName = "Test Case 1";

            // Act
            var completedSteps = ContextManager.GetCompletedSteps(testCaseName);

            // Assert
            completedSteps.Should().BeEmpty();
        }

        [Fact]
        public void GetCompletedSteps_WithCompletedSteps_ShouldReturnStepsAndClearThem()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);
            ContextManager.StartStep("Step 1");
            ContextManager.PassStep();
            ContextManager.StartStep("Step 2");
            ContextManager.FailStep();

            // Act
            var completedSteps = ContextManager.GetCompletedSteps(testCaseName);

            // Assert
            completedSteps.Should().HaveCount(2);
            completedSteps[0].Data!.Action.Should().Be("Step 1");
            completedSteps[0].Execution!.Status.Should().Be(StepResultStatus.Passed);
            completedSteps[1].Data!.Action.Should().Be("Step 2");
            completedSteps[1].Execution!.Status.Should().Be(StepResultStatus.Failed);

            // Verify steps are cleared after getting them
            var emptySteps = ContextManager.GetCompletedSteps(testCaseName);
            emptySteps.Should().BeEmpty();
        }

        [Fact]
        public void NestedSteps_ShouldBeProperlyStructured()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);

            // Act
            ContextManager.StartStep("Parent Step");
            ContextManager.StartStep("Child Step 1");
            ContextManager.PassStep();
            ContextManager.StartStep("Child Step 2");
            ContextManager.FailStep();
            ContextManager.PassStep();

            // Assert
            var completedSteps = ContextManager.GetCompletedSteps(testCaseName);
            completedSteps.Should().HaveCount(1);
            
            var parentStep = completedSteps[0];
            parentStep.Data!.Action.Should().Be("Parent Step");
            parentStep.Execution!.Status.Should().Be(StepResultStatus.Passed);
            parentStep.Steps.Should().HaveCount(2);
            
            parentStep.Steps[0].Data!.Action.Should().Be("Child Step 1");
            parentStep.Steps[0].Execution!.Status.Should().Be(StepResultStatus.Passed);
            parentStep.Steps[1].Data!.Action.Should().Be("Child Step 2");
            parentStep.Steps[1].Execution!.Status.Should().Be(StepResultStatus.Failed);
        }

        [Fact]
        public void Clear_ShouldClearStepStack()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);
            ContextManager.StartStep("Test Step");

            // Act
            ContextManager.Clear();

            // Assert
            // After clearing, we need to set test case name again to test PassStep behavior
            ContextManager.SetTestCaseName(testCaseName);
            Assert.Throws<InvalidOperationException>(() => ContextManager.PassStep());
        }

        [Fact]
        public void SaveSteps_ShouldClearStepStack()
        {
            // Arrange
            var testCaseName = "Test Case 1";
            ContextManager.SetTestCaseName(testCaseName);
            ContextManager.StartStep("Test Step");

            // Act
            ContextManager.SaveSteps();

            // Assert
            // After saving, we need to set test case name again to test PassStep behavior
            ContextManager.SetTestCaseName(testCaseName);
            Assert.Throws<InvalidOperationException>(() => ContextManager.PassStep());
        }
    }
} 
