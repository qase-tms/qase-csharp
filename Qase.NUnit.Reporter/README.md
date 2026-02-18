# Qase TMS NUnit Reporter

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](../LICENSE) [![NuGet Downloads](https://img.shields.io/nuget/dt/Qase.NUnit.Reporter)](https://www.nuget.org/packages/Qase.NUnit.Reporter/)

Publish your test results easily and effectively with Qase TMS.

## Features

- Automatic test case generation from NUnit tests
- Link automated tests to existing Qase test cases using `[QaseIds]` attribute
- Report test results in real-time to Qase TestOps
- Support for test steps using `[Step]` attribute
- Attach files and screenshots to test results
- Capture test metadata (title, fields, suites)
- Parametrized test support with `[TestCase]`
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
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
public class ExampleTests
{
    [Test]
    [QaseIds(1)]
    [Title("Example Test")]
    public void SimpleTest()
    {
        Assert.That(true, Is.True);
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

| NUnit Status | Qase Status | Description |
|--------------|-------------|-------------|
| Passed | Passed | Test assertions succeeded |
| Failed (assertion) | Failed | Test assertion failed (Assert.That, Assert.AreEqual, etc.) |
| Failed (error) | Invalid | Test failed due to exception or error (not assertion) |
| Skipped | Skipped | Test was skipped via [Ignore("reason")] or conditional skip |
| Inconclusive | Skipped | Test was marked as inconclusive via Assert.Inconclusive() |

## Requirements

- **NUnit**: Version 3.4 or higher is required.
- **.NET**: Version 6.0 or higher is required (or .NET Standard 2.0).

For further assistance, please refer to the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
