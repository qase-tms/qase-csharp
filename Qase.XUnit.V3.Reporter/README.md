# Qase TestOps xUnit v3 Reporter

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](../LICENSE) [![NuGet Downloads](https://img.shields.io/nuget/dt/Qase.XUnit.V3.Reporter)](https://www.nuget.org/packages/Qase.XUnit.V3.Reporter/)

Publish your test results easily and effectively with Qase TestOps.

## Installation

```bash
dotnet add package Qase.XUnit.V3.Reporter
```

Or add directly to your `.csproj` file:

```xml
<PackageReference Include="Qase.XUnit.V3.Reporter" Version="1.1.8" />
```

## Features

- Automatic test case generation from xUnit v3 tests
- Link automated tests to existing Qase test cases using `[QaseIds]` attribute
- Report test results in real-time to Qase TestOps
- Support for test steps using `[Step]` attribute
- Attach files and screenshots to test results
- Capture test metadata (title, fields, suites)
- Parametrized test support with `[Theory]`, `[InlineData]`, and `[MemberData]`
- Flexible configuration via `qase.config.json` or environment variables

## Project Setup

The Qase xUnit v3 reporter uses Microsoft Testing Platform (MTP) v2. Your test project requires specific settings in the `.csproj` file for the reporter to work.

### Required `.csproj` Settings

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit.v3.mtp-v2" Version="3.2.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.1" />
    <PackageReference Include="Qase.XUnit.V3.Reporter" Version="1.1.8" />
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
| `OutputType` | Yes | Must be `Exe` for xUnit v3 projects using MTP v2 |
| `TestingPlatformDotnetTestSupport` | Yes | Enables `dotnet test` integration with the MTP v2 runner |

> **Important:** Without these properties, the Qase reporter extension will not be registered and test results will not be reported.

> **Note:** The `xunit.v3` meta-package defaults to MTP v1. Use `xunit.v3.mtp-v2` for explicit MTP v2 support, which is required by the Qase reporter.

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

Make sure the config file is copied to the output directory (see the `<Content>` item in the `.csproj` example above).

### 2. Add Qase Attributes to Your Tests

```csharp
using Qase.Csharp.Commons.Attributes;

public class ExampleTests
{
    [Fact]
    [QaseIds(1)]
    [Title("Example Test")]
    public void SimpleTest()
    {
        Assert.True(true);
    }
}
```

> **Note:** The `using Xunit;` directive is not needed if your project uses xUnit v3's global usings via the `xunit.v3.mtp-v2` package, which automatically imports `Xunit` namespaces.

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

| xUnit v3 Status | Qase Status | Description |
|------------------|-------------|-------------|
| PassedTestNodeStateProperty | Passed | Test assertions succeeded |
| FailedTestNodeStateProperty | Failed | Test assertion failed (Assert.Equal, Assert.True, etc.) |
| ErrorTestNodeStateProperty | Invalid | Test failed due to runtime exception (not assertion) |
| SkippedTestNodeStateProperty | Skipped | Test was skipped via Skip parameter or Assert.Skip() |

## Requirements

- **xUnit v3**: Version 3.0 or higher is required.
- **.NET**: Version 8.0 or higher is required.

For further assistance, please refer to the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
