# Roadmap: Qase MSTest Reporter

## Overview

Build an MSTest v3+ reporter for Qase TMS that integrates with Microsoft.Testing.Platform (MTP) v2 to capture test lifecycle events, Qase attributes, parameterized test data, and report results via the shared CoreReporter infrastructure. The build follows a strict dependency chain: project scaffolding must exist before extension hooks work, session lifecycle must work before test results flow, basic result capture must work before metadata extraction, and all features must work before packaging. Each phase delivers a verifiable capability building toward full feature parity with existing NUnit and xUnit reporters.

## Phases

**Phase Numbering:**
- Integer phases (1, 2, 3): Planned milestone work
- Decimal phases (2.1, 2.2): Urgent insertions (marked with INSERTED)

Decimal phases appear between their surrounding integers in numeric order.

- [ ] **Phase 1: Foundation & Extension Scaffolding** - Project structure, MTP v2 dependencies, skeleton extension class, solution integration
- [ ] **Phase 2: Session Lifecycle & CoreReporter Integration** - Session start/finish hooks, CoreReporter wiring, conditional activation
- [ ] **Phase 3: Core Test Result Capture** - ConsumeAsync pipeline, status mapping, timing, error extraction, native async
- [ ] **Phase 4: Qase Attribute Extraction & Metadata** - Reflection-based attribute reading, step tracking, attachments, comments, suite hierarchy, signatures
- [ ] **Phase 5: Parameterized Test Handling** - DataRow/DynamicData support, parameter extraction, per-row result reporting
- [ ] **Phase 6: Packaging, Examples & Tests** - NuGet package, unit tests project, example project, changelog

## Phase Details

### Phase 1: Foundation & Extension Scaffolding
**Goal**: MSTest reporter project exists, compiles, and can be referenced by test projects with correct MTP v2 dependencies
**Depends on**: Nothing (first phase)
**Requirements**: INFRA-01, INFRA-02, INFRA-03, INFRA-05
**Success Criteria** (what must be TRUE):
  1. Qase.MSTest.Reporter.csproj targets net8.0 and references Microsoft.Testing.Platform v2 and Qase.Csharp.Commons
  2. QaseMSTestExtension skeleton class implements IDataConsumer and ITestSessionLifetimeHandler via CompositeExtensionFactory with all methods returning defaults
  3. Project is included in qase-csharp.sln and the entire solution builds without errors
  4. A test project can reference Qase.MSTest.Reporter and register the extension without runtime TypeLoadException
**Plans**: TBD

Plans:
- [ ] 01-01: TBD

### Phase 2: Session Lifecycle & CoreReporter Integration
**Goal**: Reporter initializes CoreReporter on test session start and uploads/completes results on session finish
**Depends on**: Phase 1
**Requirements**: LIFE-03, LIFE-04, INFRA-04
**Success Criteria** (what must be TRUE):
  1. OnTestSessionStartingAsync creates CoreReporter instance and calls startTestRun() when Qase configuration is present
  2. OnTestSessionFinishingAsync calls CoreReporter.uploadResults() and completeTestRun() to flush all pending results
  3. IsEnabledAsync returns false (disabling the extension) when no Qase configuration is detected
  4. Unit tests verify lifecycle method ordering and CoreReporter integration
**Plans**: TBD

Plans:
- [ ] 02-01: TBD

### Phase 3: Core Test Result Capture
**Goal**: Reporter captures test pass/fail/skip/error outcomes with timing and error details, submitting them to CoreReporter via native async
**Depends on**: Phase 2
**Requirements**: LIFE-01, LIFE-02, LIFE-05, LIFE-06, LIFE-07
**Success Criteria** (what must be TRUE):
  1. ConsumeAsync correctly maps PassedTestNodeStateProperty, FailedTestNodeStateProperty, ErrorTestNodeStateProperty, and SkippedTestNodeStateProperty to corresponding TestResultStatus values
  2. Assertion failures (FailedTestNodeStateProperty) are distinguished from runtime errors (ErrorTestNodeStateProperty) in reported results
  3. Test duration, start time, and end time are extracted from TimingProperty and included in each TestResult
  4. Failure messages and stack traces are extracted from state property Exception fields
  5. All async operations use native await (no GetAwaiter().GetResult() blocking calls)
**Plans**: TBD

Plans:
- [ ] 03-01: TBD

### Phase 4: Qase Attribute Extraction & Metadata
**Goal**: Reporter extracts all Qase attributes via reflection, tracks steps/attachments/comments via ContextManager, and generates suite hierarchy and test signatures
**Depends on**: Phase 3
**Requirements**: QASE-01, QASE-02, QASE-03, QASE-04, QASE-05, QASE-06, QASE-07, QASE-08, QASE-09, QASE-10
**Success Criteria** (what must be TRUE):
  1. Tests annotated with [QaseIds], [Title], [Fields], [Suites], [Ignore] have their attribute values correctly extracted via reflection using TestMethodIdentifierProperty and included in TestResult
  2. Step tracking via [Step]/[Qase] attributes works in MSTest context with ContextManager correctly associating steps to the running test
  3. Attachments added via Metadata.Attach() and comments added via Metadata.Comment() are captured and included in reported results
  4. Suite hierarchy is derived from namespace/class structure by default, overridden by [Suites] attribute when present
  5. Test signatures are generated via Signature.Generate() enabling cross-run correlation in Qase
**Plans**: TBD

Plans:
- [ ] 04-01: TBD

### Phase 5: Parameterized Test Handling
**Goal**: DataRow and DynamicData parameterized tests each produce a separate, correctly attributed test result with extracted parameters
**Depends on**: Phase 4
**Requirements**: PARAM-01, PARAM-02, PARAM-03
**Success Criteria** (what must be TRUE):
  1. Each DataRow invocation produces a separate TestResult with correct pass/fail status and parameter values
  2. Each DynamicData invocation produces a separate TestResult with correct pass/fail status and parameter values
  3. Parameter names and values are included in the TestResult and reflected in test signatures for unique identification
**Plans**: TBD

Plans:
- [ ] 05-01: TBD

### Phase 6: Packaging, Examples & Tests
**Goal**: Reporter is packaged as a publishable NuGet, validated by a comprehensive example project and unit test suite, with changelog updated
**Depends on**: Phase 5
**Requirements**: PKG-01, PKG-02, PKG-03, PKG-04
**Success Criteria** (what must be TRUE):
  1. Qase.MSTest.Reporter NuGet package is configured with proper metadata (PackageId, Description, license, etc.) matching existing reporter package patterns
  2. Qase.MSTest.Reporter.Tests project exists with unit tests covering reporter logic using xUnit + Moq + FluentAssertions
  3. examples/MSTestExamples project demonstrates attribute usage, parameterized tests, step tracking, attachments, and metadata in working MSTest tests
  4. CHANGELOG.md is updated documenting the MSTest reporter addition
**Plans**: TBD

Plans:
- [ ] 06-01: TBD

## Progress

**Execution Order:**
Phases execute in numeric order: 1 -> 2 -> 3 -> 4 -> 5 -> 6

| Phase | Plans Complete | Status | Completed |
|-------|----------------|--------|-----------|
| 1. Foundation & Extension Scaffolding | 0/? | Not started | - |
| 2. Session Lifecycle & CoreReporter Integration | 0/? | Not started | - |
| 3. Core Test Result Capture | 0/? | Not started | - |
| 4. Qase Attribute Extraction & Metadata | 0/? | Not started | - |
| 5. Parameterized Test Handling | 0/? | Not started | - |
| 6. Packaging, Examples & Tests | 0/? | Not started | - |
