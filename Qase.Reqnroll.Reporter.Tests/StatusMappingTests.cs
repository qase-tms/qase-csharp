using Reqnroll;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Reqnroll.Reporter;

namespace Qase.Reqnroll.Reporter.Tests;

public class StatusMappingTests
{
    [Theory]
    [InlineData(ScenarioExecutionStatus.OK, TestResultStatus.Passed)]
    [InlineData(ScenarioExecutionStatus.TestError, TestResultStatus.Failed)]
    [InlineData(ScenarioExecutionStatus.UndefinedStep, TestResultStatus.Invalid)]
    [InlineData(ScenarioExecutionStatus.BindingError, TestResultStatus.Invalid)]
    [InlineData(ScenarioExecutionStatus.StepDefinitionPending, TestResultStatus.Skipped)]
    [InlineData(ScenarioExecutionStatus.Skipped, TestResultStatus.Skipped)]
    public void MapStatus_MapsCorrectly(ScenarioExecutionStatus input, TestResultStatus expected)
    {
        var result = QaseReqnrollBindings.MapStatus(input);

        result.Should().Be(expected);
    }
}
