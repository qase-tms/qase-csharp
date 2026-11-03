# Upgrading to Qase.MSTest.Reporter 2.0

Version 2.0 moves the reporter to MSTest 4 and Microsoft Testing Platform (MTP) 2. MTP 2 is binary incompatible with MTP 1, so one reporter build cannot serve both MSTest 3 and MSTest 4.

| Your project | Reporter version |
|--------------|------------------|
| MSTest 4.x | 2.0 or later |
| MSTest 3.x | 1.2.x |

The other Qase reporters (NUnit, xUnit, xUnit v3, Reqnroll) have no changes in 2.0; only their version number follows the shared release.

## What stays the same

- Qase attributes (`[QaseIds]`, `[Title]`, `[Suites]`, `[Fields]`, `[Tags]`, `[Step]`, `[Ignore]`), `Metadata.Comment`/`Metadata.Attach` and the configuration (`qase.config.json`, `QASE_*` variables) are unchanged.
- Result signatures are unchanged: results keep linking to the same Qase test cases, no new cases are created by the upgrade.
- The local report format is unchanged.

## Steps

### 1. Upgrade MSTest to 4.x

```xml
<PackageReference Include="MSTest.TestFramework" Version="4.5.2" />
<PackageReference Include="MSTest.TestAdapter" Version="4.5.2" />
<PackageReference Include="Qase.MSTest.Reporter" Version="2.0.0" />
```

MSTest 4 has breaking changes of its own; see Microsoft's [MSTest v3 to v4 migration guide](https://learn.microsoft.com/dotnet/core/testing/unit-testing-mstest-migration-v3-v4).

Keep `EnableMSTestRunner` and `<OutputType>Exe</OutputType>`. `Microsoft.NET.Test.Sdk` is not needed for the MTP runner and can be removed.

### 2. Enable the MTP runner for `dotnet test` (.NET 10 SDK)

Add a `global.json` next to the solution or the test project:

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

and pass the project with `--project`:

```bash
dotnet test --project MyTests.csproj
```

`dotnet test` reads `global.json` from the current directory upwards, so run it from a directory under the one that holds the file. In CI, change the command the same way.

## Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| `error NU1605: Detected package downgrade: MSTest.TestFramework from 4.0.0 to 3.x` on restore | Reporter 2.0 in an MSTest 3 project | Upgrade MSTest to 4.x, or stay on reporter 1.2.x |
| `TypeLoadException: Could not load type '...IDataConsumer'`, zero tests ran | Reporter 1.2.x in an MSTest 4 project | Upgrade the reporter to 2.0 |
| `Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later` | No `global.json` with the MTP runner | Add `global.json` (step 2) |
| Tests run, but nothing is reported to Qase | The project runs on VSTest (`EnableMSTestRunner` is missing) | Set `EnableMSTestRunner` and `OutputType` as in the [README](../README.md#project-setup) |

Failure messages in Qase look different after the upgrade (`Assertion failed. Expected condition to be true.` instead of `Assert.IsTrue failed.`): MSTest 4 formats assertion messages differently.
