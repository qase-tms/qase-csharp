# xUnit v3 Examples

Demonstrates all Qase TestOps reporter features with the xUnit v3 testing framework using Microsoft Testing Platform v2.

## Test Files

| File | What it demonstrates |
|------|---------------------|
| `AttributeTests.cs` | `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`, class-level attributes |
| `MetadataTests.cs` | `Metadata.Comment()` -- contextual comments on test results |
| `StepTests.cs` | `[Step]` -- basic, nested, custom titles, mixed pass/fail status |
| `AttachmentTests.cs` | `Metadata.Attach()` -- file, multiple files, byte[], step-level |
| `ParameterizedTests.cs` | `[Theory]` with `[InlineData]` and `[MemberData]` |
| `CombinedTests.cs` | E2E scenarios combining all features together |

## Test Data

The `testdata/` directory contains real files used by attachment examples:
- `screenshot.png` -- sample screenshot image
- `test-log.txt` -- sample application log

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/qase-tms/qase-csharp.git
   cd qase-csharp/examples/xUnitV3Examples
   ```

2. Create a `qase.config.json` file (see [Configuration](../README.md#configuration)).

3. Run the tests:
   ```bash
   dotnet run
   ```

   Or with `dotnet test` (requires .NET 10 SDK with MTP opt-in via `global.json`):
   ```bash
   dotnet test
   ```

## MTP v2 Notes

xUnit v3 uses Microsoft Testing Platform v2 (MTP v2) instead of the classic VSTest runner. This requires several project configuration differences compared to xUnit v2:

- **OutputType must be `Exe`** -- MTP v2 test projects are self-contained executables
- **TestingPlatformDotnetTestSupport** -- enables `dotnet test` to use MTP v2 instead of VSTest
- **xunit.v3.mtp-v2 package** -- the explicit MTP v2 integration package (not `xunit.v3`)

These settings are already configured in `xUnitV3Examples.csproj`.

### Running on .NET 10 SDK

On .NET 10 SDK, `dotnet test` requires opting in to the Microsoft Testing Platform runner via `global.json`:

```json
{
  "testRunner": {
    "commandName": "microsoft-testing-platform"
  }
}
```

Alternatively, since xUnit v3 MTP v2 projects are self-contained executables, you can run tests directly:

```bash
dotnet run --project examples/xUnitV3Examples/
```

## Resources

- [xUnit v3 Reporter Documentation](../../Qase.XUnit.V3.Reporter/README.md)
- [Steps Guide](../../Qase.XUnit.V3.Reporter/docs/STEPS.md)
- [Attachments Guide](../../Qase.XUnit.V3.Reporter/docs/ATTACHMENTS.md)
- [Usage Guide](../../Qase.XUnit.V3.Reporter/docs/usage.md)
