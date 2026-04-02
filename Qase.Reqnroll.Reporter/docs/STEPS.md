# Test Steps

## Overview

The Qase Reqnroll reporter automatically captures every Given/When/Then step from your Gherkin scenarios as structured test steps in Qase TestOps. This happens without any additional code or configuration — every step is tracked with its status and timing.

**Why test steps matter in BDD:**
- Each Gherkin step becomes a visible entry in the Qase test result
- Easier debugging: pinpoint the exact step that failed
- Step-by-step execution log shows the full scenario flow
- Timing per step helps identify performance bottlenecks

**No prerequisites required** — automatic step tracking works out of the box for all scenarios.

See also: [Usage Guide - Test Steps](usage.md#test-steps)

## Automatic Step Capture

Every Given, When, Then, And, and But step is automatically reported:

```gherkin
Scenario: User registration
  Given the registration page is open
  When the user fills in the registration form
  And the user submits the form
  Then the confirmation page is displayed
  And a welcome email is sent
```

**Steps in Qase:**
- `Given the registration page is open` — Passed
- `When the user fills in the registration form` — Passed
- `And the user submits the form` — Passed
- `Then the confirmation page is displayed` — Passed
- `And a welcome email is sent` — Passed

Each step includes:
- The step keyword and text (e.g., "Given the registration page is open")
- Execution status (Passed, Failed, or Skipped)
- Start time, end time, and duration

## Step Status

The reporter automatically tracks step status based on execution:

**Status rules:**
- **Passed**: Step definition executes without throwing an exception
- **Failed**: Step definition throws any exception (assertion failures, runtime exceptions, etc.)
- **Skipped**: Steps that run after a failure are automatically marked as Skipped

### Example with Passing Steps

```gherkin
Scenario: Successful login
  Given the user is on the login page
  When the user enters valid credentials
  Then the dashboard is displayed
```

```csharp
[Binding]
public class LoginSteps
{
    [Given(@"the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        // Navigate to login page
        Assert.True(driver.Url.Contains("/login"));
    }

    [When(@"the user enters valid credentials")]
    public void WhenTheUserEntersValidCredentials()
    {
        // Enter credentials
        driver.FindElement(By.Id("username")).SendKeys("admin");
        driver.FindElement(By.Id("password")).SendKeys("password123");
        driver.FindElement(By.Id("login-btn")).Click();
    }

    [Then(@"the dashboard is displayed")]
    public void ThenTheDashboardIsDisplayed()
    {
        Assert.True(driver.Url.Contains("/dashboard"));
    }
}
```

All three steps will have "Passed" status in Qase TestOps.

### Example with Failing Step

```gherkin
Scenario: Login with invalid password
  Given the user is on the login page
  When the user enters an invalid password
  Then an error message is displayed
  And the user remains on the login page
```

If the step "Then an error message is displayed" throws an assertion error:

**Step statuses in Qase:**
- `Given the user is on the login page` — Passed
- `When the user enters an invalid password` — Passed
- `Then an error message is displayed` — Failed
- `And the user remains on the login page` — Skipped

### Failure Propagation

When a step fails, all subsequent steps in the same scenario are automatically marked as Skipped. This accurately reflects that those steps were never executed:

```gherkin
Scenario: Multi-step checkout
  Given the user has items in the cart
  When the user proceeds to checkout
  And the user enters payment details
  Then the order is confirmed
  And a confirmation email is sent
```

If "And the user enters payment details" fails:

**Result:**
- `Given the user has items in the cart` — Passed
- `When the user proceeds to checkout` — Passed
- `And the user enters payment details` — Failed
- `Then the order is confirmed` — Skipped
- `And a confirmation email is sent` — Skipped
- Overall scenario result: Failed

## Manual Steps with ContextManager

In addition to automatic Gherkin step capture, you can add manual sub-steps within your step definitions using the `ContextManager`:

```csharp
using Qase.Csharp.Commons;

[Binding]
public class DetailedSteps
{
    [When(@"the user completes the registration form")]
    public void WhenTheUserCompletesTheRegistrationForm()
    {
        ContextManager.StartStep("Fill in personal information");
        // Fill first name, last name, etc.
        ContextManager.PassStep();

        ContextManager.StartStep("Set password");
        // Set password and confirm
        ContextManager.PassStep();

        ContextManager.StartStep("Accept terms and conditions");
        // Check the terms checkbox
        ContextManager.PassStep();
    }
}
```

Manual steps from `ContextManager` are collected alongside automatic Gherkin steps and appear in the Qase test result, providing additional detail within a Gherkin step.

## Scenario Outline Steps

Steps in Scenario Outlines work the same way. Each example row generates a separate test result, and the steps for each row are tracked independently:

```gherkin
Scenario Outline: Login validation
  Given the user is on the login page
  When the user enters "<username>" and "<password>"
  Then the result should be "<expected>"

  Examples:
    | username | password | expected |
    | admin    | pass123  | success  |
    | invalid  | wrong    | failure  |
```

Each row produces a separate test result in Qase, each with its own 3 steps (Given, When, Then) and their individual statuses.

Step statuses are visible in the Qase test run result alongside the overall test result, providing detailed insight into where and why a scenario failed.
