# NUnit Examples

Demonstrates all Qase TestOps reporter features with the NUnit testing framework.

## Test Files

| File | What it demonstrates |
|------|---------------------|
| `AttributeTests.cs` | `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`, class-level attributes |
| `MetadataTests.cs` | `Metadata.Comment()` — contextual comments on test results |
| `StepTests.cs` | `[Step]` — basic, nested, custom titles, mixed pass/fail status |
| `AttachmentTests.cs` | `Metadata.Attach()` — file, multiple files, byte[], step-level |
| `ParameterizedTests.cs` | `[TestCase]`, `[Values]`/`[Range]`, `ExpectedResult` |
| `CombinedTests.cs` | E2E scenarios combining all features together |

## Test Data

The `testdata/` directory contains real files used by attachment examples:
- `screenshot.png` — sample screenshot image
- `test-log.txt` — sample application log

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/qase-tms/qase-csharp.git
   cd qase-csharp/examples/NUnitExamples
   ```

2. Create a `qase.config.json` file (see [Configuration](../README.md#configuration)).

3. Run the tests:
   ```bash
   dotnet test
   ```

## Resources

- [NUnit Reporter Documentation](../../Qase.NUnit.Reporter/README.md)
- [Steps Guide](../../Qase.NUnit.Reporter/docs/STEPS.md)
- [Attachments Guide](../../Qase.NUnit.Reporter/docs/ATTACHMENTS.md)
- [Usage Guide](../../Qase.NUnit.Reporter/docs/usage.md)
