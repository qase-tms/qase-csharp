# Qase C# Examples

Example projects demonstrating all Qase TestOps reporter capabilities for xUnit and NUnit frameworks.

## Example Categories

Each example project contains the following test files:

| File | Description |
|------|-------------|
| `AttributeTests.cs` | All reporter attributes: `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`, class-level attributes |
| `MetadataTests.cs` | `Metadata.Comment()` for adding contextual comments to test results |
| `StepTests.cs` | Basic steps, nested steps, steps with custom `[Title]`, mixed-status steps |
| `AttachmentTests.cs` | Single file, multiple files, byte array, step-level attachments |
| `ParameterizedTests.cs` | Data-driven tests (xUnit: `Theory`/`InlineData`/`MemberData`, NUnit: `TestCase`/`Values`/`Range`) |
| `CombinedTests.cs` | End-to-end scenarios combining steps, attachments, comments, and attributes |

## Available Projects

### xUnit Examples

**Location**: [`xUnitExamples/`](xUnitExamples/)

```bash
cd xUnitExamples
dotnet test
```

For details, see [xUnitExamples/README.md](xUnitExamples/README.md).

### NUnit Examples

**Location**: [`NUnitExamples/`](NUnitExamples/)

```bash
cd NUnitExamples
dotnet test
```

For details, see [NUnitExamples/README.md](NUnitExamples/README.md).

## Configuration

Both projects require a `qase.config.json` configuration file:

1. Copy `qase.config.json.example` to `qase.config.json`
2. Fill in your Qase API token and project code

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
- Qase TestOps account with API token

## Additional Resources

- [Qase C# Documentation](https://github.com/qase-tms/qase-csharp)
- [Qase API Documentation](https://developers.qase.io/)
- [xUnit Documentation](https://xunit.net/)
- [NUnit Documentation](https://docs.nunit.org/)
