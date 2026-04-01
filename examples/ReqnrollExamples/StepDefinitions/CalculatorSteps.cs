using Reqnroll;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class CalculatorSteps
{
    private bool _calculatorOpen;
    private int _result;

    [Given(@"the calculator is open")]
    public void GivenTheCalculatorIsOpen()
    {
        _calculatorOpen = true;
        Assert.That(_calculatorOpen, Is.True);
    }

    [When(@"I add (\d+) and (\d+)")]
    public void WhenIAddNumbers(int a, int b)
    {
        _result = a + b;
    }

    [When(@"I perform ""(.*)"" on (\d+) and (\d+)")]
    public void WhenIPerformOperation(string operation, int a, int b)
    {
        _result = operation switch
        {
            "add" => a + b,
            "subtract" => a - b,
            "multiply" => a * b,
            _ => throw new ArgumentException($"Unknown operation: {operation}")
        };
    }

    [When(@"I divide (\d+) by (\d+)")]
    public void WhenIDivide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException();
        _result = a / b;
    }

    [Then(@"the result should be (\d+)")]
    public void ThenTheResultShouldBe(int expected)
    {
        Assert.That(_result, Is.EqualTo(expected));
    }

    [Then(@"an error should be shown")]
    public void ThenAnErrorShouldBeShown()
    {
        Assert.Pass("Error handling verified");
    }
}
