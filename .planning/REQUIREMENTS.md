# Requirements: Qase MSTest Reporter

**Defined:** 2026-02-26
**Core Value:** MSTest users can report test results to Qase TMS with same feature parity as NUnit/xUnit reporters

## v1 Requirements

Requirements for initial release. Each maps to roadmap phases.

### Test Lifecycle

- [ ] **LIFE-01**: Reporter captures test passed/failed/skipped/error statuses via MTP TestNodeUpdateMessage
- [ ] **LIFE-02**: Reporter distinguishes assertion failures from runtime errors (FailedTestNodeStateProperty vs ErrorTestNodeStateProperty)
- [ ] **LIFE-03**: Reporter starts test session via ITestSessionLifetimeHandler.OnTestSessionStartingAsync
- [ ] **LIFE-04**: Reporter completes test session via ITestSessionLifetimeHandler.OnTestSessionFinishingAsync
- [ ] **LIFE-05**: Reporter captures test duration and timing from TimingProperty
- [ ] **LIFE-06**: Reporter extracts failure message and stack trace from state property Exception fields
- [ ] **LIFE-07**: Reporter uses native async (await in ConsumeAsync, no GetAwaiter().GetResult())

### Qase Integration

- [ ] **QASE-01**: Reporter extracts [QaseIds] attribute values via reflection using TestMethodIdentifierProperty
- [ ] **QASE-02**: Reporter extracts [Title] attribute for custom test names
- [ ] **QASE-03**: Reporter extracts [Fields] attribute for custom field values
- [ ] **QASE-04**: Reporter extracts [Suites] attribute for suite hierarchy
- [ ] **QASE-05**: Reporter extracts [Ignore] attribute for defect association
- [ ] **QASE-06**: Step tracking works via [Step]/[Qase] attributes through ContextManager
- [ ] **QASE-07**: Attachments work via Metadata.Attach() API
- [ ] **QASE-08**: Comments work via Metadata.Comment() API
- [ ] **QASE-09**: Test signature generated via Signature.Generate() for cross-run correlation
- [ ] **QASE-10**: Suite hierarchy derived from namespace/class structure or [Suites] attribute

### Parameterized Tests

- [ ] **PARAM-01**: DataRow parameterized tests produce separate TestResult per data row
- [ ] **PARAM-02**: DynamicData parameterized tests produce separate TestResult per data set
- [ ] **PARAM-03**: Parameters correctly extracted and included in test result

### Infrastructure

- [ ] **INFRA-01**: Qase.MSTest.Reporter project created with net8.0 target framework
- [ ] **INFRA-02**: Project references Qase.Csharp.Commons and Microsoft.Testing.Platform v2
- [ ] **INFRA-03**: Extension uses CompositeExtensionFactory<T> for IDataConsumer + ITestSessionLifetimeHandler
- [ ] **INFRA-04**: Reporter delegates to CoreReporter for result submission (TestOps and Report modes)
- [ ] **INFRA-05**: Project integrated into qase-csharp.sln solution file

### Packaging

- [ ] **PKG-01**: NuGet package configured (Qase.MSTest.Reporter) with proper metadata
- [ ] **PKG-02**: Unit tests project created (Qase.MSTest.Reporter.Tests)
- [ ] **PKG-03**: Example project created (examples/MSTestExamples) demonstrating integration
- [ ] **PKG-04**: CHANGELOG.md updated with MSTest reporter addition

## v2 Requirements

### Advanced Features

- **ADV-01**: Extension auto-registration via NuGet MSBuild .props/.targets (zero-config for users)
- **ADV-02**: Retry attribute ([Retry]) support with correct final outcome reporting
- **ADV-03**: Parallel test execution support with proper thread isolation
- **ADV-04**: Real-time result streaming (report results as tests complete, not just at session end)

## Out of Scope

| Feature | Reason |
|---------|--------|
| MSTest v2 (legacy TestAdapter) support | Project targets modern MTP v2 only |
| netstandard2.0 target framework | MTP v2 requires net8.0+ |
| Changes to Qase.Csharp.Commons | Reuse existing infrastructure as-is |
| VSTest integration | Legacy platform, MTP is the future |
| Custom MSTest test runner | Use standard Microsoft.Testing.Platform |

## Traceability

| Requirement | Phase | Status |
|-------------|-------|--------|
| INFRA-01 | Phase 1 | Pending |
| INFRA-02 | Phase 1 | Pending |
| INFRA-03 | Phase 1 | Pending |
| INFRA-05 | Phase 1 | Pending |
| LIFE-03 | Phase 2 | Pending |
| LIFE-04 | Phase 2 | Pending |
| INFRA-04 | Phase 2 | Pending |
| LIFE-01 | Phase 3 | Pending |
| LIFE-02 | Phase 3 | Pending |
| LIFE-05 | Phase 3 | Pending |
| LIFE-06 | Phase 3 | Pending |
| LIFE-07 | Phase 3 | Pending |
| QASE-01 | Phase 4 | Pending |
| QASE-02 | Phase 4 | Pending |
| QASE-03 | Phase 4 | Pending |
| QASE-04 | Phase 4 | Pending |
| QASE-05 | Phase 4 | Pending |
| QASE-06 | Phase 4 | Pending |
| QASE-07 | Phase 4 | Pending |
| QASE-08 | Phase 4 | Pending |
| QASE-09 | Phase 4 | Pending |
| QASE-10 | Phase 4 | Pending |
| PARAM-01 | Phase 5 | Pending |
| PARAM-02 | Phase 5 | Pending |
| PARAM-03 | Phase 5 | Pending |
| PKG-01 | Phase 6 | Pending |
| PKG-02 | Phase 6 | Pending |
| PKG-03 | Phase 6 | Pending |
| PKG-04 | Phase 6 | Pending |

**Coverage:**
- v1 requirements: 29 total
- Mapped to phases: 29
- Unmapped: 0

---
*Requirements defined: 2026-02-26*
*Last updated: 2026-02-26 after roadmap creation*
