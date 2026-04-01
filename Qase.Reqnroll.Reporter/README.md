# Qase TestOps Reqnroll Reporter

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](../LICENSE) [![NuGet Downloads](https://img.shields.io/nuget/dt/Qase.Reqnroll.Reporter)](https://www.nuget.org/packages/Qase.Reqnroll.Reporter/)

Publish your test results easily and effectively with Qase TestOps.

## Installation

```bash
dotnet add package Qase.Reqnroll.Reporter
```

Or add directly to your `.csproj` file:

```xml
<PackageReference Include="Qase.Reqnroll.Reporter" Version="1.1.12" />
```

The plugin is auto-discovered by Reqnroll. No additional configuration in `reqnroll.json` is needed.

## Features

- Automatic test case generation from Reqnroll scenarios
- Link scenarios to existing Qase test cases using `@QaseId` Gherkin tags
- Report test results in real-time to Qase TestOps
- Automatic step reporting from Given/When/Then steps
- Attach files and screenshots to test results
- Capture test metadata via tags (title, fields, suites)
- Scenario Outline parameter support
- Flexible configuration via `qase.config.json` or environment variables

## Quick Start

### 1. Configure the Reporter

Create a `qase.config.json` file in your project root:

```json
{
  "mode": "testops",
  "testOps": {
    "api": {
      "token": "YOUR_QASE_API_TOKEN",
      "host": "qase.io"
    },
    "project": "YOUR_PROJECT_CODE",
    "run": {
      "title": "Reqnroll Test Run",
      "complete": true
    }
  }
}
```

### 2. Add Qase Tags to Your Scenarios

```gherkin
@QaseId:123
Scenario: User can login with valid credentials
  Given the user is on the login page
  When the user enters valid credentials
  Then the user should see the dashboard
```

### 3. Run Your Tests

Execute your tests using the dotnet CLI:

```bash
dotnet test
```

View your results at: `https://app.qase.io/run/PROJECT_CODE`

## Documentation

| Document | Description | Link |
|----------|-------------|------|
| Usage Guide | Comprehensive guide to using all reporter features | [docs/usage.md](docs/usage.md) |
| Attachments | How to attach files and screenshots to test results | [docs/ATTACHMENTS.md](docs/ATTACHMENTS.md) |
| Test Steps | How automatic step reporting works | [docs/STEPS.md](docs/STEPS.md) |
| Configuration Reference | Complete configuration options for Qase reporters | [Qase.Csharp.Commons](../Qase.Csharp.Commons/README.md#configuration) |

## Available Tags

| Tag | Description | Example |
|-----|-------------|---------|
| `@QaseId:ID` | Link to Qase test case ID(s) | `@QaseId:123` or `@QaseId:123,456` |
| `@QaseTitle:Title` | Override test title (underscores become spaces) | `@QaseTitle:My_Test` |
| `@QaseFields:key:value` | Set custom field | `@QaseFields:severity:critical` |
| `@QaseSuite:Path` | Set suite hierarchy (backslash-separated) | `@QaseSuite:Auth\Login` |
| `@QaseIgnore` | Exclude from Qase reporting | `@QaseIgnore` |

All tag prefixes are case-insensitive.

## Test Result Statuses

| Reqnroll Status | Qase Status | Description |
|-----------------|-------------|-------------|
| OK | Passed | All steps executed successfully |
| TestError | Failed | A step definition threw an exception or assertion failure |
| UndefinedStep | Invalid | A step has no matching step definition |
| BindingError | Invalid | A step definition binding is invalid |
| StepDefinitionPending | Skipped | A step definition is marked as pending |
| Skipped | Skipped | Scenario was skipped |

## Requirements

- **Reqnroll**: Version 2.0.0 or higher is required.
- **.NET**: Version 6.0 or higher is required (or .NET Standard 2.0).

For further assistance, please refer to the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
