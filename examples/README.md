# Qase C# Examples

This directory contains example projects demonstrating how to use Qase TMS integration with different testing frameworks.

## Available Examples

### xUnit Examples

The `xUnitExamples` project demonstrates how to use Qase TMS with xUnit testing framework.

**Location**: `xUnitExamples/`

**Features**:
- Simple tests with Qase attributes
- Parameterized tests using `[Theory]` and `[InlineData]`
- Usage of Qase attributes: `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`
- Test steps using `[Step]` attribute
- Class-level attributes

**Quick Start**:
```bash
cd xUnitExamples
# Create qase.config.json (see qase.config.json.example)
dotnet test
```

For more details, see [xUnitExamples/README.md](xUnitExamples/README.md).

### NUnit Examples

The `NUnitExamples` project demonstrates how to use Qase TMS with NUnit testing framework.

**Location**: `NUnitExamples/`

**Features**:
- Simple tests with Qase attributes
- Parameterized tests using `[TestCase]`
- Usage of Qase attributes: `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`
- Test steps using `[Step]` attribute
- Class-level attributes

**Quick Start**:
```bash
cd NUnitExamples
# Create qase.config.json (copy from qase.config.json.example)
# Create .addins file or use NuGet package automatic setup
dotnet test
# Or use NUnit Console Runner:
# nunit3-console NUnitExamples.dll
```

For more details, see [NUnitExamples/README.md](NUnitExamples/README.md).

## Configuration

Both example projects require a `qase.config.json` configuration file. You can:

1. Copy the example file: `qase.config.json.example` → `qase.config.json`
2. Fill in your Qase API token and project code
3. Adjust other settings as needed

Example configuration:
```json
{
  "mode": "testops",
  "fallback": "report",
  "testops": {
    "api": {
      "token": "<your_token_here>",
      "host": "qase.io"
    },
    "project": "<your_project_code>",
    "run": {
      "title": "Example Run",
      "complete": true
    }
  }
}
```

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) or higher
- Qase TMS account with API token
- For NUnit: NUnit Console Runner (optional, for command-line execution)

## Additional Resources

- [Qase C# Documentation](https://github.com/qase-tms/qase-csharp)
- [Qase API Documentation](https://developers.qase.io/)
- [xUnit Documentation](https://xunit.net/)
- [NUnit Documentation](https://docs.nunit.org/)
