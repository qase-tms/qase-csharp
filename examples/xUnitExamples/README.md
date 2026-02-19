# xUnit Examples

Demonstrates all Qase TestOps reporter features with the xUnit testing framework.

## Test Files

| File | What it demonstrates |
|------|---------------------|
| `AttributeTests.cs` | `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`, class-level attributes |
| `MetadataTests.cs` | `Metadata.Comment()` — contextual comments on test results |
| `StepTests.cs` | `[Step]` — basic, nested, custom titles, mixed pass/fail status |
| `AttachmentTests.cs` | `Metadata.Attach()` — file, multiple files, byte[], step-level |
| `ParameterizedTests.cs` | `[Theory]` with `[InlineData]` and `[MemberData]` |
| `CombinedTests.cs` | E2E scenarios combining all features together |

## Test Data

The `testdata/` directory contains real files used by attachment examples:
- `screenshot.png` — sample screenshot image
- `test-log.txt` — sample application log

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/qase-tms/qase-csharp.git
   cd qase-csharp/examples/xUnitExamples
   ```

2. Create a `qase.config.json` file (see [Configuration](../README.md#configuration)).

3. Run the tests:
   ```bash
   dotnet test
   ```

## Resources

- [xUnit Reporter Documentation](../../Qase.XUnit.Reporter/README.md)
- [Steps Guide](../../Qase.XUnit.Reporter/docs/STEPS.md)
- [Attachments Guide](../../Qase.XUnit.Reporter/docs/ATTACHMENTS.md)
- [Usage Guide](../../Qase.XUnit.Reporter/docs/usage.md)
