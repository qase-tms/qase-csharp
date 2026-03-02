# MSTest Examples

Demonstrates all Qase TestOps reporter features with the MSTest v3 testing framework.

## Test Files

| File | What it demonstrates |
|------|---------------------|
| `AttributeTests.cs` | `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`, class-level attributes |
| `MetadataTests.cs` | `Metadata.Comment()` — contextual comments on test results |
| `StepTests.cs` | `[Step]` — basic, nested, custom titles, mixed pass/fail status |
| `AttachmentTests.cs` | `Metadata.Attach()` — file, multiple files, byte[], step-level |
| `ParameterizedTests.cs` | `[DataRow]`, `[DynamicData]` — parameterized test variations |
| `CombinedTests.cs` | E2E scenarios combining all features together |

## Test Data

The `testdata/` directory contains real files used by attachment examples:
- `screenshot.png` — sample screenshot image
- `test-log.txt` — sample application log

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/qase-tms/qase-csharp.git
   cd qase-csharp/examples/MSTestExamples
   ```

2. Create a `qase.config.json` file (see [Configuration](../README.md#configuration)).

3. Run the tests:
   ```bash
   dotnet test
   ```

## Notes

- This example project uses standard VSTest mode (`dotnet test`).
- MSTest v3 supports `[DataRow]` directly on `[TestMethod]` — `[DataTestMethod]` is not needed.
- The `[Ignore]` attribute used here is from `Qase.Csharp.Commons.Attributes`, not MSTest's built-in `[Ignore]`.

## Resources

- [MSTest Reporter Documentation](../../Qase.MSTest.Reporter/README.md)
