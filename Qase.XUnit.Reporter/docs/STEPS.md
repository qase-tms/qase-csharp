# Test Steps

## Overview

Test steps are named sub-actions within a test that appear in Qase TestOps as a structured execution log. They help break down complex tests into readable, manageable parts, making it easier to identify exactly where a test fails.

**Why use test steps?**
- Break complex tests into logical phases
- Easier debugging: pinpoint the exact step that failed
- Better test documentation: step names create a readable test narrative
- Detailed execution history in Qase TestOps

**Prerequisites:**
1. The test method must have the `[Qase]` attribute
2. Step methods must have the `[Step]` attribute
3. Both attributes come from `Qase.Csharp.Commons.Attributes`

See also: [Usage Guide - Test Steps](usage.md#test-steps)

## Basic Steps

Here's how to create a test with steps:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class BasicStepExample
{
    [Fact]
    [Qase]
    public void TestWithBasicSteps()
    {
        // Call step methods
        OpenApplication();
        PerformAction();
    }

    [Step]
    public void OpenApplication()
    {
        // Step logic: launch application
        Assert.True(true);
    }

    [Step]
    public void PerformAction()
    {
        // Step logic: perform some action
        Assert.True(true);
    }
}
```

Each `[Step]`-decorated method call is recorded as a separate step in the Qase test run result. The step method name becomes the step name displayed in Qase TestOps.

**How it works:**
- When `OpenApplication()` is called, it's recorded as a step named "OpenApplication"
- When `PerformAction()` is called, it's recorded as a step named "PerformAction"
- Both steps appear in sequence in the Qase test result

## Nested Steps

Steps can call other steps, creating a hierarchy:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class NestedStepExample
{
    [Fact]
    [Qase]
    public void TestWithNestedSteps()
    {
        LoginStep();
        PerformUserAction();
    }

    [Step]
    public void LoginStep()
    {
        // This step calls other steps
        EnterCredentials();
        ClickLoginButton();
    }

    [Step]
    public void EnterCredentials()
    {
        // Enter username and password
        Assert.True(true);
    }

    [Step]
    public void ClickLoginButton()
    {
        // Click the login button
        Assert.True(true);
    }

    [Step]
    public void PerformUserAction()
    {
        // Another top-level step
        Assert.True(true);
    }
}
```

**Step hierarchy in Qase:**
- `LoginStep` (parent step)
  - `EnterCredentials` (child step)
  - `ClickLoginButton` (child step)
- `PerformUserAction` (parent step)

Nested steps create a clear structure showing the relationship between actions. Parent steps contain child steps, making the test flow easier to understand.

## Step Status

The xUnit reporter automatically tracks step status based on execution:

**Status rules:**
- **Passed**: Step method completes without throwing an exception
- **Failed**: Step method throws any exception (assertion failures, runtime exceptions, etc.)

### Example with Passing Steps

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class PassingStepExample
{
    [Fact]
    [Qase]
    public void TestWithPassingSteps()
    {
        StepOne();
        StepTwo();
    }

    [Step]
    public void StepOne()
    {
        // This step passes
        Assert.True(true);
    }

    [Step]
    public void StepTwo()
    {
        // This step also passes
        Assert.Equal(2, 1 + 1);
    }
}
```

Both steps will have "Passed" status in Qase TestOps.

### Example with Failing Step

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class FailingStepExample
{
    [Fact]
    [Qase]
    public void TestWithFailingStep()
    {
        StepOne();

        try
        {
            StepTwo(); // This step will fail
        }
        catch
        {
            // Catch to allow step three to execute
        }

        StepThree();
    }

    [Step]
    public void StepOne()
    {
        // This step passes
        Assert.True(true);
    }

    [Step]
    public void StepTwo()
    {
        // This step fails
        Assert.True(false, "This assertion fails");
    }

    [Step]
    public void StepThree()
    {
        // This step executes despite step two failing
        Assert.True(true);
    }
}
```

**Step statuses in Qase:**
- `StepOne`: Passed
- `StepTwo`: Failed (assertion failed)
- `StepThree`: Passed (executed after catching the exception)

### Failure Propagation

When a step fails without exception handling, the test stops:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class FailurePropagationExample
{
    [Fact]
    [Qase]
    public void TestWithUnhandledFailure()
    {
        StepOne();
        StepTwo(); // Fails and stops test execution
        StepThree(); // Never executes
    }

    [Step]
    public void StepOne()
    {
        Assert.True(true);
    }

    [Step]
    public void StepTwo()
    {
        // This fails and stops the test
        Assert.True(false, "Critical failure");
    }

    [Step]
    public void StepThree()
    {
        // This step never executes
        Assert.True(true);
    }
}
```

**Result:**
- `StepOne`: Passed
- `StepTwo`: Failed
- `StepThree`: Not executed
- Overall test result: Failed

Step statuses are visible in the Qase test run result alongside the overall test result, providing detailed insight into where and why a test failed.
