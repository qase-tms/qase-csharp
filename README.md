# [Qase TestOps](https://qase.io) Reporters for C#

Monorepo containing [Qase TestOps](https://qase.io) integration tools for C# testing frameworks. This repository
includes reporters and API clients designed to streamline the process of reporting test results to Qase TestOps.

## Reporters

| Reporter | Description | Documentation | Package |
|----------|-------------|---------------|---------|
| [Qase.xUnit.Reporter](Qase.XUnit.Reporter/) | xUnit test framework integration with real-time result reporting | [README](Qase.XUnit.Reporter/README.md) · [Usage](Qase.XUnit.Reporter/docs/usage.md) · [Docs](Qase.XUnit.Reporter/docs/) | [![NuGet](https://img.shields.io/nuget/v/Qase.XUnit.Reporter)](https://nuget.org/packages/Qase.XUnit.Reporter) |
| [Qase.NUnit.Reporter](Qase.NUnit.Reporter/) | NUnit test framework integration with real-time result reporting | [README](Qase.NUnit.Reporter/README.md) · [Usage](Qase.NUnit.Reporter/docs/usage.md) · [Docs](Qase.NUnit.Reporter/docs/) | [![NuGet](https://img.shields.io/nuget/v/Qase.NUnit.Reporter)](https://nuget.org/packages/Qase.NUnit.Reporter) |
| Qase.MSTest.Reporter | MSTest test framework integration | Coming soon | - |
| Qase.SpecFlow.Reporter | SpecFlow BDD framework integration | Coming soon | - |

## Libraries

| Library | Description | Links |
|---------|-------------|-------|
| [Qase.Csharp.Commons](Qase.Csharp.Commons/) | Shared SDK for test reporters — configuration, API integration, result processing | [README](Qase.Csharp.Commons/README.md) · [![NuGet](https://img.shields.io/nuget/v/Qase.Csharp.Commons)](https://nuget.org/packages/Qase.Csharp.Commons) |

## API Clients

| Client | Description | Links |
|--------|-------------|-------|
| [Qase.ApiClient.V1](Qase.ApiClient.V1/) | Qase TestOps API v1 client | [README](Qase.ApiClient.V1/README.md) |
| [Qase.ApiClient.V2](Qase.ApiClient.V2/) | Qase TestOps API v2 client | [README](Qase.ApiClient.V2/README.md) |

## Configuration

All Qase reporters share a common configuration through `qase.config.json`. See the [Configuration Reference](Qase.Csharp.Commons/README.md#configuration) for all options.

## Examples

Example projects are available in the [examples/](examples/) directory for both xUnit and NUnit frameworks.
