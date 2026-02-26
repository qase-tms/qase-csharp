# Qase MSTest Reporter

## What This Is

MSTest v3+ reporter for Qase TMS, integrating with Microsoft.Testing.Platform to capture test results, steps, attachments and report them to Qase cloud (TestOps mode) or local file (Report mode). Built following the same architecture as existing NUnit and xUnit reporters in this repository.

## Core Value

MSTest users can report test results to Qase TMS with the same feature parity and developer experience as NUnit and xUnit reporters.

## Requirements

### Validated

(None yet — ship to validate)

### Active

- [ ] MSTest v3+ test lifecycle events captured (test started, passed, failed, skipped)
- [ ] Qase attributes work with MSTest tests ([Qase], [Title], [Step], [Suite], [Fields], [Ignore], [QaseIds])
- [ ] Test results reported to Qase TestOps (cloud) via CoreReporter
- [ ] Test results reported to local file (Report mode) via CoreReporter
- [ ] Parameterized tests (DataRow, DynamicData) properly handled
- [ ] Step tracking via StepAspect works in MSTest context
- [ ] Attachments captured and uploaded
- [ ] Test suite hierarchy preserved
- [ ] NuGet package publishable (Qase.MSTest.Reporter)
- [ ] Unit tests for reporter logic (Qase.MSTest.Reporter.Tests)
- [ ] Example project demonstrating integration (examples/MSTestExamples)
- [ ] Integration with solution file (qase-csharp.sln)

### Out of Scope

- MSTest v2 (classic TestFramework/TestAdapter) support — focusing on modern v3+ only
- Custom MSTest test runner — using standard Microsoft.Testing.Platform
- Changes to Qase.Csharp.Commons — reuse existing infrastructure as-is

## Context

- Repository already has NUnit and xUnit reporters with well-established patterns
- Qase.Csharp.Commons provides all shared infrastructure: CoreReporter, ContextManager, configuration, API clients, domain models
- MSTest v3 uses Microsoft.Testing.Platform with extension points for test lifecycle hooks
- Current version is 1.1.4 (from Directory.Build.props)
- Tests use xUnit + Moq + FluentAssertions across the project
- NuGet packaging follows existing .csproj patterns with PackageId, Description, etc.

## Constraints

- **Tech stack**: Must use Microsoft.Testing.Platform APIs (MSTest v3+), not legacy MSTestV2 adapter
- **Architecture**: Must follow existing reporter pattern — integrate with CoreReporter via Qase.Csharp.Commons
- **Compatibility**: Same .NET target frameworks as existing reporters
- **Versioning**: Same version (1.1.4) as other packages in the solution

## Key Decisions

| Decision | Rationale | Outcome |
|----------|-----------|---------|
| MSTest v3+ only (no v2) | Modern platform with proper extension points, v2 is legacy | — Pending |
| Reuse Qase.Csharp.Commons as-is | Avoid breaking existing reporters, shared infrastructure is stable | — Pending |

---
*Last updated: 2026-02-26 after initialization*
