# Qase Reqnroll Reporter - Usage Guide

## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Linking Test Cases](#linking-test-cases)
- [Test Metadata](#test-metadata)
- [Test Steps](#test-steps)
- [Attachments](#attachments)
- [Parameters](#parameters)
- [Ignoring Tests](#ignoring-tests)
- [Comments](#comments)

## Installation

To install the Qase Reqnroll Reporter, use the NuGet package manager:

### NuGet CLI

```bash
dotnet add package Qase.Reqnroll.Reporter
```

### PackageReference

Add the following to your `.csproj` file:

```xml
<PackageReference Include="Qase.Reqnroll.Reporter" Version="1.1.12" />
```

The `Qase.Csharp.Commons` package is included as a transitive dependency and provides the metadata APIs used throughout this guide.

The plugin is auto-discovered by Reqnroll via the `[assembly: RuntimePlugin]` attribute. No additional configuration in `reqnroll.json` is needed.

## Configuration

The Qase Reqnroll reporter can be configured using two methods:

1. **Configuration file**: `qase.config.json` in your project root
2. **Environment variables**: Override values from the configuration file

### Configuration File

Create a `qase.config.json` file with the following structure:

```json
{
  "mode": "testops",
  "fallback": "report",
  "debug": true,
  "environment": "local",
  "logging": {
    "console": true,
    "file": false
  },
  "report": {
    "driver": "local",
    "connection": {
      "local": {
        "path": "./build/qase-report",
        "format": "json"
      }
    }
  },
  "testops": {
    "api": {
      "token": "<token>",
      "host": "qase.io"
    },
    "run": {
      "title": "Regression Run",
      "description": "Description of the regression run",
      "complete": true
    },
    "defect": false,
    "project": "<project_code>",
    "batch": {
      "size": 100
    }
  }
}
```

Make sure to copy the config file to the output directory by adding this to your `.csproj`:

```xml
<ItemGroup>
  <Content Include="qase.config.json" Condition="Exists('qase.config.json')">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

### Environment Variables

You can override configuration values using environment variables:

- `QASE_MODE` - Reporter mode (testops, report, off)
- `QASE_TESTOPS_API_TOKEN` - Your Qase API token
- `QASE_TESTOPS_PROJECT` - Your Qase project code

For the complete list of configuration options, see [Configuration Reference](../../Qase.Csharp.Commons/README.md#configuration).

## Linking Test Cases

Use the `@QaseId` tag in your `.feature` files to link scenarios to existing test cases in Qase. This ensures a reliable binding even if you rename or move your scenarios.

### Single Test Case ID

```gherkin
@QaseId:123
Scenario: User can login with valid credentials
  Given the user is on the login page
  When the user enters valid credentials
  Then the user should see the dashboard
```

### Multiple Test Case IDs

Link a scenario to multiple test cases by separating IDs with commas:

```gherkin
@QaseId:123,456,789
Scenario: User can login and access profile
  Given the user is on the login page
  When the user logs in
  Then the user can access the profile page
```

### Scenario Outline with QaseId

```gherkin
@QaseId:200
Scenario Outline: Login with different users
  Given the user is on the login page
  When the user enters "<username>" and "<password>"
  Then the login result should be "<result>"

  Examples:
    | username | password  | result  |
    | admin    | pass123   | success |
    | user1    | secret    | success |
    | invalid  | wrong     | failure |
```

### Auto-Generation

If you don't specify `@QaseId`, the reporter will automatically generate test cases in Qase based on your scenario names and feature files. Subsequent runs will match the same test cases as long as names remain unchanged.

## Test Metadata

Enhance your test cases with metadata using Gherkin tags.

### Title

Set a custom title for your test case. Underscores in the tag value are replaced with spaces:

```gherkin
@QaseTitle:User_can_successfully_log_in_with_valid_credentials
Scenario: Login test
  Given the user is on the login page
  When the user enters valid credentials
  Then the user should see the dashboard
```

This scenario will appear in Qase as "User can successfully log in with valid credentials".

### Fields

Add custom fields to your test case:

```gherkin
@QaseFields:priority:high
@QaseFields:component:authentication
@QaseFields:severity:critical
Scenario: Critical login test
  Given the user is on the login page
  When the user enters valid credentials
  Then the user should see the dashboard
```

### Suites

Organize test cases into suites using the `@QaseSuite` tag. Use backslash (`\`) to define hierarchy levels:

```gherkin
@QaseSuite:API\Authentication\Login
Scenario: Test in a nested suite
  Given the API is available
  When the user authenticates
  Then the response is successful
```

This creates the suite hierarchy: API > Authentication > Login.

By default, the Feature name is used as the suite. The `@QaseSuite` tag overrides this behavior.

### Feature-Level Tags

Tags placed at the Feature level apply to all scenarios in the file:

```gherkin
@QaseFields:component:user-api
@QaseSuite:API\User_Management
Feature: User API

  @QaseId:1001
  @QaseTitle:Create_User_-_Valid_Data
  @QaseFields:priority:high
  Scenario: Create user with valid data
    Given the API is available
    When a new user is created
    Then the user exists in the system

  @QaseId:1002
  Scenario: Delete user
    Given the API is available
    When the user is deleted
    Then the user no longer exists
```

Both scenarios inherit `@QaseFields:component:user-api` and `@QaseSuite:API\User_Management` from the Feature level. The first scenario additionally overrides the priority field and title.

### Combined Metadata Example

```gherkin
@QaseId:1001
@QaseTitle:Create_User_-_Valid_Data
@QaseFields:priority:high
@QaseFields:component:User_API
@QaseSuite:API\User_Management\Creation
Scenario: Create user with valid data should succeed
  Given the API is available
  When a new user is created with valid data
  Then the response status code is 201
  And the user exists in the system
```

## Test Steps

The Qase Reqnroll reporter **automatically captures every Given/When/Then step** as a test step in Qase. No additional code or attributes are required.

Each Gherkin step is reported with:
- Step type and text (e.g., "Given the user is on the login page")
- Individual status (Passed, Failed, or Skipped)
- Timing information

```gherkin
Scenario: Checkout process
  Given the user is logged in
  When the user adds an item to the cart
  And the user proceeds to checkout
  Then the order confirmation is displayed
```

This produces 4 steps in Qase, each with its own status and duration.

For more details, see [Test Steps Guide](STEPS.md).

## Attachments

Attach files, screenshots, and other content to your test results from within step definitions.

### Attach a File by Path

```csharp
using Reqnroll;
using Qase.Csharp.Commons;

[Binding]
public class StepDefinitions
{
    [When(@"the user takes a screenshot")]
    public void WhenTheUserTakesAScreenshot()
    {
        CaptureScreenshot("screenshot.png");
        Metadata.Attach("screenshot.png");
    }

    private void CaptureScreenshot(string filename) { }
}
```

### Attach Multiple Files

```csharp
[Then(@"the test artifacts are collected")]
public void ThenTheTestArtifactsAreCollected()
{
    Metadata.Attach(new List<string>
    {
        "app.log",
        "config.json",
        "screenshot.png"
    });
}
```

### Attach Content from Byte Array

```csharp
using System.Text;

[Then(@"the API response is attached")]
public void ThenTheApiResponseIsAttached()
{
    string apiResponse = GetApiResponse();
    Metadata.Attach(
        Encoding.UTF8.GetBytes(apiResponse),
        "api-response.json"
    );
}
```

For more details, see [Attachments Guide](ATTACHMENTS.md).

## Parameters

Scenario Outline examples are automatically captured and reported to Qase as test parameters. Each row in the Examples table creates a separate test result with the parameter values displayed.

```gherkin
@QaseId:300
Scenario Outline: Calculator addition
  Given the calculator is open
  When I enter <a> and <b>
  Then the result should be <expected>

  Examples:
    | a   | b   | expected |
    | 2   | 3   | 5        |
    | 10  | 20  | 30       |
    | -5  | 5   | 0        |
```

Each row creates a separate test result in Qase with the parameter values (a, b, expected) captured and displayed. This allows you to track results for each data variation independently.

## Ignoring Tests

Use the `@QaseIgnore` tag to exclude specific scenarios from being reported to Qase. The test will still execute normally, but results won't be sent to Qase.

```gherkin
Feature: Example tests

  @QaseIgnore
  Scenario: Work in progress - not reported to Qase
    Given some setup
    When some action
    Then some result

  Scenario: Normal test - reported to Qase
    Given some setup
    When some action
    Then some result
```

**Note**: `@QaseIgnore` prevents reporting to Qase only. The scenario still runs normally with the underlying test runner.

## Comments

Add comments to your test results from within step definitions using the `Metadata.Comment()` method.

```csharp
using Reqnroll;
using Qase.Csharp.Commons;

[Binding]
public class StepDefinitions
{
    [Given(@"the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        Metadata.Comment("Navigating to the login page");
        // Step implementation
    }

    [Then(@"the user should see the dashboard")]
    public void ThenTheUserShouldSeeTheDashboard()
    {
        Metadata.Comment("Dashboard loaded successfully");
        // Step implementation
    }
}
```

Comments appear in the test result details in Qase and are useful for adding context, debugging information, or execution notes.
