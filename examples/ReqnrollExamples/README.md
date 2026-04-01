# Reqnroll Examples

Demonstrates all Qase TestOps reporter features with the Reqnroll BDD testing framework.

## Test Files

| File | What it demonstrates |
|------|---------------------|
| `Features/TagTests.feature` | `@QaseId`, `@QaseTitle`, `@QaseFields`, `@QaseSuite`, `@QaseIgnore`, feature-level tags |
| `Features/MetadataTests.feature` | `Metadata.Comment()` — contextual comments on test results |
| `Features/StepTests.feature` | Automatic step reporting from Given/When/Then steps |
| `Features/AttachmentTests.feature` | `Metadata.Attach()` — file, multiple files, byte[], step-level |
| `Features/ParameterizedTests.feature` | Scenario Outline with Examples tables |
| `Features/CombinedTests.feature` | E2E scenarios combining all features together |

## Test Data

The `testdata/` directory contains real files used by attachment examples:
- `screenshot.png` — sample screenshot image
- `test-log.txt` — sample application log

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/qase-tms/qase-csharp.git
   cd qase-csharp/examples/ReqnrollExamples
   ```

2. Create a `qase.config.json` file (see [Configuration](../README.md#configuration)).

3. Run the tests:
   ```bash
   dotnet test
   ```

## Resources

- [Reqnroll Reporter Documentation](../../Qase.Reqnroll.Reporter/README.md)
- [Steps Guide](../../Qase.Reqnroll.Reporter/docs/STEPS.md)
- [Attachments Guide](../../Qase.Reqnroll.Reporter/docs/ATTACHMENTS.md)
- [Usage Guide](../../Qase.Reqnroll.Reporter/docs/usage.md)
