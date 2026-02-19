# Roadmap: Qase C# Reporters

## Milestones

- ✅ **v1.0 Documentation Update** — Phases 1-3 (shipped 2026-02-18)
- ✅ **v1.1 Spec Compliance** — Phases 4-5 (shipped 2026-02-18)
- 🚧 **v1.2 Examples Showcase** — Phases 6-9 (in progress)

## Phases

<details>
<summary>✅ v1.0 Documentation Update (Phases 1-3) — SHIPPED 2026-02-18</summary>

- [x] Phase 1: xUnit Reporter Documentation (2/2 plans) — completed 2026-02-18
- [x] Phase 2: NUnit Reporter Documentation (2/2 plans) — completed 2026-02-18
- [x] Phase 3: Commons, Root & Cross-cutting (2/2 plans) — completed 2026-02-18

</details>

<details>
<summary>✅ v1.1 Spec Compliance (Phases 4-5) — SHIPPED 2026-02-18</summary>

- [x] Phase 4: Serialization & Model Alignment (2/2 plans) — completed 2026-02-18
- [x] Phase 5: Report Structure (2/2 plans) — completed 2026-02-18

</details>

### 🚧 v1.2 Examples Showcase (In Progress)

**Milestone Goal:** Comprehensive, realistic test examples showcasing all reporter capabilities for both xUnit and NUnit frameworks

- [ ] **Phase 6: Attributes & Metadata** - Basic reporter attributes and metadata examples
- [ ] **Phase 7: Steps & Attachments** - Steps and attachments with real test data files
- [ ] **Phase 8: Parameterized Tests** - Data-driven test examples for both frameworks
- [ ] **Phase 9: Combined Scenarios & Documentation** - Full E2E examples and documentation

## Phase Details

### Phase 6: Attributes & Metadata
**Goal**: Developers can see examples of all reporter attributes and metadata features in realistic test scenarios
**Depends on**: Phase 5 (completed)
**Requirements**: ATTR-01, ATTR-02, ATTR-03, ATTR-04, ATTR-05, ATTR-06, META-01
**Success Criteria** (what must be TRUE):
  1. Examples directory contains tests demonstrating QaseIds, Title, Fields, Suites, and Ignore attributes for both xUnit and NUnit
  2. Class-level attribute examples show how to apply Fields and Suites to entire test classes
  3. Metadata.Comment() usage is demonstrated in realistic test failure scenarios
  4. Each example has realistic test names and meaningful context (not "Test1", "Test2")
**Plans**: TBD

Plans:
- [ ] 06-01: TBD
- [ ] 06-02: TBD

### Phase 7: Steps & Attachments
**Goal**: Developers can see how to structure tests with steps and attach various file types
**Depends on**: Phase 6
**Requirements**: STEP-01, STEP-02, STEP-03, STEP-04, ATCH-01, ATCH-02, ATCH-03, ATCH-04, ATCH-05
**Success Criteria** (what must be TRUE):
  1. Examples show basic steps, nested steps, steps with custom titles, and mixed-status steps
  2. Real test data files exist in testdata/ directory (PNG images, log/txt files)
  3. Attachment examples cover single file, multiple files, byte array content, and step-level attachments
  4. Tests demonstrate realistic scenarios (e.g., screenshot on failure, log file capture)
**Plans**: TBD

Plans:
- [ ] 07-01: TBD
- [ ] 07-02: TBD

### Phase 8: Parameterized Tests
**Goal**: Developers can see data-driven testing patterns for both frameworks
**Depends on**: Phase 6 (attributes), Phase 7 (steps/attachments)
**Requirements**: PARM-01, PARM-02, PARM-03, PARM-04
**Success Criteria** (what must be TRUE):
  1. xUnit examples show Theory with InlineData and MemberData in realistic scenarios
  2. NUnit examples show TestCase and Values/Range parameters in realistic scenarios
  3. Parameterized tests use meaningful test data (not just integers or strings)
  4. Examples demonstrate how reporter handles multiple test iterations
**Plans**: TBD

Plans:
- [ ] 08-01: TBD

### Phase 9: Combined Scenarios & Documentation
**Goal**: Developers can see complete end-to-end examples and understand the full example suite structure
**Depends on**: Phase 6, Phase 7, Phase 8
**Requirements**: COMB-01, COMB-02, DOCS-01, DOCS-02
**Success Criteria** (what must be TRUE):
  1. Full E2E test combines steps, attachments, comments, and multiple attributes in one realistic scenario
  2. Data-driven E2E test shows parameterization with steps and attachments together
  3. examples/README.md lists and describes all example categories with usage instructions
  4. xUnitExamples and NUnitExamples project READMEs explain how to run examples and what each demonstrates
**Plans**: TBD

Plans:
- [ ] 09-01: TBD

## Progress

| Phase | Milestone | Plans Complete | Status | Completed |
|-------|-----------|----------------|--------|-----------|
| 1. xUnit Reporter Documentation | v1.0 | 2/2 | Complete | 2026-02-18 |
| 2. NUnit Reporter Documentation | v1.0 | 2/2 | Complete | 2026-02-18 |
| 3. Commons, Root & Cross-cutting | v1.0 | 2/2 | Complete | 2026-02-18 |
| 4. Serialization & Model Alignment | v1.1 | 2/2 | Complete | 2026-02-18 |
| 5. Report Structure | v1.1 | 2/2 | Complete | 2026-02-18 |
| 6. Attributes & Metadata | v1.2 | 0/2 | Not started | - |
| 7. Steps & Attachments | v1.2 | 0/2 | Not started | - |
| 8. Parameterized Tests | v1.2 | 0/1 | Not started | - |
| 9. Combined Scenarios & Documentation | v1.2 | 0/1 | Not started | - |
