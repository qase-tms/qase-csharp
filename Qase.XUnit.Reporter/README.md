# Qase TestOps xUnit Reporter

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](../LICENSE) [![NuGet Downloads](https://img.shields.io/nuget/dt/Qase.XUnit.Reporter)](https://www.nuget.org/packages/Qase.XUnit.Reporter/)

Publish your test results easily and effectively with Qase TestOps.

## Features

- Automatic test case generation from xUnit tests
- Link automated tests to existing Qase test cases using `[QaseIds]` attribute
- Report test results in real-time to Qase TestOps
- Support for test steps using `[Step]` attribute
- Attach files and screenshots to test results
- Capture test metadata (title, fields, suites)
- Parametrized test support with `[Theory]` and `[InlineData]`
- Flexible configuration via `qase.config.json` or environment variables

## Quick Start

### 1. Configure the Reporter

Create a `qase.config.json` file in your project root:

```json
{
  "mode": "testops",
  "testops": {
    "api": {
      "token": "<your-api-token>"
    },
    "project": "<your-project-code>"
  }
}
```

### 2. Add Qase Attributes to Your Tests

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class ExampleTests
{
    [Fact]
    [QaseIds(1)]
    [Title("Example Test")]
    public void SimpleTest()
    {
        Assert.True(true);
    }
}
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
| Test Steps | How to add structured steps to your tests | [docs/STEPS.md](docs/STEPS.md) |
| Configuration Reference | Complete configuration options for Qase reporters | [Qase.Csharp.Commons](../Qase.Csharp.Commons/README.md#configuration) |

## Test Result Statuses

| xUnit Status | Qase Status | Description |
|--------------|-------------|-------------|
| Passed | Passed | Test assertions succeeded |
| Failed (assertion) | Failed | Test assertion failed (Assert.Equal, Assert.True, etc.) |
| Failed (other) | Invalid | Test failed due to exception or error (not assertion) |
| Skipped | Skipped | Test was skipped via [Skip] or conditional skip |

## Requirements

- **xUnit**: Version 2.4.0 or higher is required.
- **.NET**: Version 6.0 or higher is required.

For further assistance, please refer to the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
