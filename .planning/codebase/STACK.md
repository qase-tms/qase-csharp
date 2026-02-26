# Technology Stack

**Analysis Date:** 2026-02-26

## Languages

**Primary:**
- C# 10 - Entire codebase, all projects
- JSON - Configuration files (qase.config.json)

## Runtime

**Environment:**
- .NET / .NET Standard (multi-targeting across projects)

**Target Frameworks:**
- `netstandard2.0` - Qase.ApiClient.V1, Qase.ApiClient.V2, Qase.Csharp.Commons, Qase.NUnit.Reporter, Qase.XUnit.Reporter
- `net6.0` - Qase.NUnit.Reporter (multi-target)
- `net9.0` - Qase.Csharp.Commons.Tests
- `net10.0` - Qase.XUnit.Reporter.Tests

**Build System:**
- MSBuild (.NET SDK format)
- Solution file: `qase-csharp.sln`

## Frameworks

**Core:**
- Microsoft.Extensions.Http 5.0.0 - HTTP client factory and dependency injection
- Microsoft.Extensions.Hosting 5.0.0 - Dependency injection and service hosting
- Microsoft.Extensions.Http.Polly 5.0.1 - Resilience policies for HTTP calls (retry, timeout, circuit breaker)
- System.Threading.Channels 8.0.0 - Async channels for concurrent operations
- System.ComponentModel.Annotations 5.0.0 - Data validation attributes

**Logging & Observability:**
- Serilog 4.3.0 - Structured logging framework
- Serilog.Extensions.Logging 9.0.2 - Integration with Microsoft.Extensions.Logging
- Serilog.Sinks.Console 6.1.1 - Console output sink
- Serilog.Sinks.File 7.0.0 - File output sink
- AspectInjector 2.9.0 - AOP framework for aspect-oriented programming (used for `StepAspect` and `QaseAspect`)

**Testing:**
- xUnit 2.6.6 (core, extensibility, runner utilities) - Unit testing framework
- NUnit 4.2.2 - Alternative unit testing framework
- NUnit.Engine.Api 3.15.0 - NUnit test engine API
- xunit.runner.visualstudio 2.8.2 - Visual Studio test adapter for xUnit

**Testing Support:**
- Moq 4.20.72 - Mocking library for unit tests
- FluentAssertions 8.8.0 - Fluent assertion library
- coverlet.collector 6.0.4 - Code coverage collection
- Microsoft.NET.Test.Sdk 18.0.1 - Test SDK runtime

**Build & Distribution:**
- Microsoft.SourceLink.GitHub 8.0.0 - Source code linking for debugging (PrivateAssets)

## Key Dependencies

**Critical:**
- Serilog (4.3.0) - Core logging infrastructure, used throughout codebase for diagnostic logging
- Microsoft.Extensions.Http (5.0.0) - HTTP client configuration, essential for API communication
- Microsoft.Extensions.Http.Polly (5.0.1) - Enables resilience patterns (retry, timeout, circuit breaker) for API calls
- AspectInjector (2.9.0) - Enables AOP for step execution tracking and Qase attribute processing

**Infrastructure:**
- Microsoft.Extensions.Hosting (5.0.0) - Provides dependency injection container
- System.Threading.Channels (8.0.0) - For async batch processing of test results
- System.ComponentModel.Annotations (5.0.0) - Data validation in models

**Testing:**
- xUnit 2.6.6 - Distributed across test projects (Qase.Csharp.Commons.Tests, Qase.XUnit.Reporter.Tests)
- NUnit 4.2.2 - Supported in Qase.NUnit.Reporter and Qase.NUnit.Reporter.Tests
- Moq 4.20.72 - Mocking in test projects (`Qase.Csharp.Commons.Tests`, `Qase.NUnit.Reporter.Tests`)

## Configuration

**Environment:**
- JSON-based: `qase.config.json` in project root or output directory
- Environment variables: `QASE_*` prefix (see ConfigFactory.LoadFromEnv)
- System properties: Not currently implemented (placeholder in ConfigFactory.LoadFromSystemProperties)

**Key Configs Required:**
- `QASE_MODE` - Mode of operation (off|testops|report)
- `QASE_TESTOPS_API_TOKEN` - API token for Qase TestOps access
- `QASE_TESTOPS_PROJECT` - Project code in Qase TestOps
- `QASE_TESTOPS_API_HOST` - API host (default: qase.io)
- `QASE_REPORT_CONNECTION_PATH` - Local report output path

**Build:**
- `Directory.Build.props` - Global properties for all projects (version 1.1.4, metadata, debug settings, SourceLink)
- Language version: C# 10
- Nullable reference types: Enabled across all projects
- Package generation: Enabled for library projects (IsPackable=true)

## Platform Requirements

**Development:**
- .NET SDK 9.0.x or higher (per build.yaml workflow)
- Visual Studio 17.0+ or JetBrains Rider
- Git for source control

**Production:**
- .NET Runtime targeting netstandard2.0 minimum (.NET Framework 4.6.1+, .NET Core 2.0+)
- For net6.0 packages: .NET 6.0 runtime or later
- Qase TMS instance (SaaS at qase.io or on-premise with custom host)

## Deployment & Publishing

**NuGet Package Distribution:**
- Packages published to: https://api.nuget.org/v3/index.json
- Automatic package generation on tag (ci/cd/build.yaml: "publish" job)
- Symbol packages (.snupkg) included
- SourceLink enabled for source navigation

**CI/CD:**
- GitHub Actions: `.github/workflows/build.yaml`
- Test on: ubuntu-latest with .NET 9.0.x
- Publish on: git tags (refs/tags/*)
- Secrets: NUGET_API_KEY for package publishing

---

*Stack analysis: 2026-02-26*
