# Architecture

**Analysis Date:** 2026-02-26

## Pattern Overview

**Overall:** Multi-layered modular architecture with a reporter abstraction pattern and dependency injection.

**Key Characteristics:**
- Core abstraction layer for test framework integration (NUnit, xUnit)
- Dual reporting modes: TestOps (cloud-based) and Report (local file-based)
- Primary-fallback reporter pattern for resilience
- Aspect-based interception for context management and step tracking
- Configuration factory pattern for environment-based setup

## Layers

**API Client Layer:**
- Purpose: Encapsulates HTTP communication with Qase TMS API
- Location: `Qase.ApiClient.V1/`, `Qase.ApiClient.V2/`
- Contains: Auto-generated API clients for V1 and V2 endpoints (RunsApi, ResultsApi, AttachmentsApi, ConfigurationsApi, etc.)
- Depends on: Microsoft.Extensions.Http, Microsoft.Extensions.Hosting
- Used by: `ClientV1`, `ClientV2` in Commons layer

**Commons Core Layer:**
- Purpose: Shared infrastructure and abstractions for all reporters
- Location: `Qase.Csharp.Commons/`
- Contains:
  - Configuration system (`config/`)
  - Core abstractions (`core/`, `reporters/`)
  - Domain models (`models/`)
  - Context management and test tracking
  - Utility functions (signature, status mapping, host detection)
- Depends on: ApiClient V1 and V2, Serilog, AspectInjector
- Used by: NUnit Reporter, xUnit Reporter

**Reporter Integration Layer:**
- Purpose: Framework-specific test event listeners and reporting logic
- Location: `Qase.NUnit.Reporter/`, `Qase.XUnit.Reporter/`
- Contains:
  - NUnit: `QaseNUnitEventListener` (ITestEventListener implementation)
  - xUnit: `QaseMessageSink` (IMessageSink implementation)
  - Framework-specific attribute parsing and event handling
- Depends on: Commons, NUnit.Engine.Api / xunit
- Used by: Test projects in examples

**Configuration & Initialization:**
- `ServiceCollectionExtensions` configures Microsoft.Extensions.DependencyInjection with:
  - Serilog logging (console and file outputs)
  - API clients (ClientV1, ClientV2) with authentication
  - Reporter selection (TestOps or Report mode) with fallback support
  - File writer for local report generation

## Data Flow

**Test Execution Reporting:**

1. Framework captures test event (NUnit/xUnit)
2. Reporter listener (QaseNUnitEventListener/QaseMessageSink) receives event
3. Listener extracts test metadata (attributes, parameters, results)
4. `ContextManager` stores test execution context (steps, attachments, messages)
5. Test result converted to `TestResult` domain model
6. Result passed to `CoreReporter` (primary reporting handler)
7. `CoreReporter` applies status mapping and delegates to active reporter:
   - **TestOps mode**: `TestopsReporter` uploads to cloud via ClientV2
   - **Report mode**: `FileReporter` writes to local JSON via FileWriter
8. On primary reporter failure: Fallback reporter activated and results retried
9. `uploadResults()` performs batch completion

**Batch Completion Flow:**
1. Test run started: `CoreReporter.startTestRun()` → calls `TestopsReporter.startTestRun()`
2. Test run completed: `CoreReporter.completeTestRun()` → finalizes batch
3. Results accumulated in reporter's internal state
4. `uploadResults()` submits batch via API (V2 for TestOps, file system for Report)

**State Management:**
- `ContextManager` uses AsyncLocal<T> for test case context (thread-scoped)
- ConcurrentDictionary for step stacks, messages, attachments (thread-safe)
- Test results accumulated in reporter implementations until batch completion
- Configuration singleton injected across all components

## Key Abstractions

**IClient Interface:**
- Purpose: Abstracts API client operations for result submission
- Examples: `ClientV1`, `ClientV2`
- Pattern: Implements async methods for API interactions (CreateRun, SubmitResults, UploadAttachment)

**ICoreReporter Interface:**
- Purpose: Defines public reporting contract
- Examples: `CoreReporter`
- Pattern: Async task-based methods for lifecycle events (startTestRun, completeTestRun, addResult)

**IInternalReporter Interface:**
- Purpose: Abstracts backend reporting implementations
- Examples: `TestopsReporter`, `FileReporter`
- Pattern: Internal-only interface with full reporting lifecycle

**TestResult Domain Model:**
- Purpose: Unified representation of test execution
- Location: `Qase.Csharp.Commons/models/domain/TestResult.cs`
- Contains: Status, steps, attachments, parameters, signatures, relations
- Serialization: JSON with snake_case and lowercase enum conversion

**Configuration System:**
- Pattern: ConfigFactory creates QaseConfig from environment variables and local qase.config.json
- Supports: Mode switching (TestOps/Report), logging configuration, API credentials, batch settings
- Location: `Qase.Csharp.Commons/config/ConfigFactory.cs` (1800+ lines)

## Entry Points

**NUnit Integration:**
- Location: `Qase.NUnit.Reporter/QaseNUnitEventListener.cs`
- Triggers: NUnit Engine initialization (auto-loaded via .addins file)
- Responsibilities:
  - Parse NUnit XML events
  - Extract test attributes and results
  - Build TestResult objects
  - Submit to CoreReporter

**xUnit Integration:**
- Location: `Qase.XUnit.Reporter/QaseMessageSink.cs`
- Triggers: xUnit test execution pipeline
- Responsibilities:
  - Capture IMessageSink events
  - Parse xUnit test output
  - Track test lifecycle (started, passed, failed, skipped)
  - Submit to CoreReporter

**Test Project Examples:**
- NUnit examples: `examples/NUnitExamples/`
- xUnit examples: `examples/xUnitExamples/`
- Configure via `qase.config.json` + environment variables

## Error Handling

**Strategy:** Graceful degradation with fallback mechanism

**Patterns:**
- QaseException: Custom exception for Qase-specific errors
- Primary reporter failures trigger fallback activation
- Failed operations logged but don't crash test execution
- File writer failures fall back to console logging
- API call failures logged with Serilog

**Resilience:**
- `CoreReporter.ExecuteWithFallbackAsync()`: Catches QaseException, invokes fallback, retries
- Fallback reporter must be pre-initialized to avoid race conditions
- Both reporters share result state via `getResults()` / `setResults()`

## Cross-Cutting Concerns

**Logging:** Serilog configured via `ServiceCollectionExtensions`
- Console output (optional)
- File output (optional, daily rolling logs)
- Debug level based on config.Debug flag
- Template: `[{Timestamp:HH:mm:ss} {Level:u3}] qase: {Message:lj}`

**Validation:** Configuration factory validates:
- Required API token presence
- Valid mode selection (TestOps/Report)
- Path accessibility for file-based reporting
- Batch size > 0

**Authentication:** API key token-based via ApiKeyToken in ClientV1/ClientV2
- Injected into HttpClient headers
- Dynamic host URL switching (qase.io vs custom subdomain)

**Serialization:** JSON with custom converters
- SnakeCaseNamingPolicy for API compatibility
- LowercaseEnumConverter for status values
- Null handling for optional fields

---

*Architecture analysis: 2026-02-26*
