# Qase TestOps MSTest Reporter

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](../LICENSE) [![NuGet Downloads](https://img.shields.io/nuget/dt/Qase.MSTest.Reporter)](https://www.nuget.org/packages/Qase.MSTest.Reporter/)

Publish your test results easily and effectively with Qase TestOps.

## Installation

```bash
dotnet add package Qase.MSTest.Reporter
```

Or add directly to your `.csproj` file:

```xml
<PackageReference Include="Qase.MSTest.Reporter" Version="2.0.0" />
```

## Features

- Automatic test case generation from MSTest tests
- Link automated tests to existing Qase test cases using `[QaseIds]` attribute
- Report test results in real-time to Qase TestOps
- Support for test steps using `[Step]` attribute
- Attach files and screenshots to test results
- Capture test metadata (title, fields, suites)
- Parametrized test support with `[DataRow]` and `[DynamicData]`
- Flexible configuration via `qase.config.json` or environment variables

## Project Setup

The Qase MSTest reporter is a Microsoft Testing Platform (MTP) v2 extension and requires MSTest 4. MSTest 3 projects should stay on `Qase.MSTest.Reporter` 1.2.x; see [Upgrading to 2.0](docs/UPGRADE.md). Your test project requires specific settings in the `.csproj` file for the reporter to work.

### Required `.csproj` Settings

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnableMSTestRunner>true</EnableMSTestRunner>
    <OutputType>Exe</OutputType>
    <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MSTest.TestFramework" Version="4.5.2" />
    <PackageReference Include="MSTest.TestAdapter" Version="4.5.2" />
    <PackageReference Include="Qase.MSTest.Reporter" Version="2.0.0" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="qase.config.json" Condition="Exists('qase.config.json')">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
```

| Property | Required | Description |
|----------|----------|-------------|
| `EnableMSTestRunner` | Yes | Enables the MTP v2 test runner instead of the legacy VSTest runner |
| `OutputType` | Yes | Must be `Exe` when `EnableMSTestRunner` is enabled |
| `TestingPlatformDotnetTestSupport` | Yes | Enables `dotnet test` integration with the MTP v2 runner |

> **Important:** Without these three properties, the Qase reporter extension will not be registered and test results will not be reported.

### `global.json`

On the .NET 10 SDK, `dotnet test` runs MTP projects only when the new test experience is enabled. Add a `global.json` next to the solution (or the test project):

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

and run `dotnet test --project <TestProject>.csproj`. Without it, the build fails with "Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later".

## Quick Start

### 1. Configure the Reporter

Create a `qase.config.json` file in your project root:

```json
{
  "mode": "testops",
  "testops": {
    "api": {
      "token": "<your-api-token>",
      "host": "qase.io"
    },
    "project": "<your-project-code>"
  }
}
```

If your workspace is on the EU cluster (you open Qase at `app-eu.qase.io`), set `host` to `eu.qase.io`. The default, `qase.io`, is the US cluster.

Make sure the config file is copied to the output directory (see the `<Content>` item in the `.csproj` example above).

### 2. Add Qase Attributes to Your Tests

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class ExampleTests
{
    [TestMethod]
    [QaseIds(1)]
    [Title("Example Test")]
    public void SimpleTest()
    {
        Assert.IsTrue(true);
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
| Upgrading to 2.0 | Moving from MSTest 3 / reporter 1.x to MSTest 4 / reporter 2.0 | [docs/UPGRADE.md](docs/UPGRADE.md) |
| Configuration Reference | Complete configuration options for Qase reporters | [Qase.Csharp.Commons](../Qase.Csharp.Commons/README.md#configuration) |

## Test Result Statuses

| MSTest Status | Qase Status | Description |
|---------------|-------------|-------------|
| Passed | Passed | Test assertions succeeded |
| Failed | Failed | Test assertion failed (Assert.AreEqual, Assert.IsTrue, etc.) |
| Error | Invalid | Test failed due to runtime exception (not assertion) |
| Skipped | Skipped | Test was skipped via MSTest `[Ignore]` or conditional skip |

## Requirements

- **MSTest**: Version 4.0 or higher is required. For MSTest 3, use `Qase.MSTest.Reporter` 1.2.x.
- **.NET**: Version 6.0 or higher is required.

For further assistance, please refer to the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
