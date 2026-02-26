# Codebase Structure

**Analysis Date:** 2026-02-26

## Directory Layout

```
qase-csharp/
├── Qase.ApiClient.V1/           # API client for Qase V1 endpoints
│   ├── Api/                     # Auto-generated API classes
│   ├── Client/                  # Client infrastructure
│   ├── Model/                   # Data transfer objects (173 files)
│   └── Extensions/              # ServiceCollection extensions
├── Qase.ApiClient.V2/           # API client for Qase V2 endpoints
│   └── [same structure as V1]
├── Qase.Csharp.Commons/         # Shared core library
│   ├── Attributes/              # [Qase, Title, Step, Suite, Fields, Ignore, QaseIds]
│   ├── Aspects/                 # AspectInjector implementations
│   ├── config/                  # Configuration classes and factory
│   ├── core/                    # Core utilities (QaseException, MimeTypes, HostInfo)
│   ├── clients/                 # API client wrappers (ClientV1, ClientV2)
│   ├── models/                  # Domain models (TestResult, StepResult, Attachment, etc.)
│   ├── reporters/               # Reporting implementations
│   ├── serialization/           # JSON converters (SnakeCaseNamingPolicy, LowercaseEnumConverter)
│   ├── Utils/                   # Utilities (StatusMappingUtils, ClientHeadersBuilder, Signature)
│   ├── writers/                 # File output (FileWriter)
│   ├── ContextManager.cs        # Global context for test tracking
│   └── ServiceCollectionExtensions.cs # DI setup
├── Qase.NUnit.Reporter/         # NUnit integration
│   ├── QaseNUnitEventListener.cs # ITestEventListener implementation
│   ├── build/                   # NuGet build targets
│   └── Qase.NUnit.Reporter.addins # NUnit addin manifest
├── Qase.XUnit.Reporter/         # xUnit integration
│   ├── QaseMessageSink.cs       # IMessageSink implementation
│   ├── QaseRunnerReporter.cs    # Runner configuration
│   └── [similar structure to NUnit]
├── Qase.Csharp.Commons.Tests/   # Unit tests
│   ├── ConfigFactoryTests.cs    # Configuration factory tests
│   ├── CoreReporterTests.cs     # Reporter logic tests
│   ├── TestopsReporterTests.cs  # Cloud reporter tests
│   ├── ClientV1Tests.cs         # V1 API client tests
│   ├── ContextManagerTests.cs   # Context management tests
│   ├── StatusMappingUtilsTests.cs # Status mapping tests
│   └── qase.config.json         # Test configuration
├── Qase.NUnit.Reporter.Tests/   # NUnit reporter tests
├── Qase.XUnit.Reporter.Tests/   # xUnit reporter tests
├── examples/
│   ├── NUnitExamples/           # NUnit example projects
│   └── xUnitExamples/           # xUnit example projects
├── Directory.Build.props         # Global MSBuild properties
├── qase-csharp.sln             # Solution file
├── changelog.md                 # Version history
└── docs/                        # Documentation
```

## Directory Purposes

**Qase.ApiClient.V1:**
- Purpose: Auto-generated wrapper around Qase TMS API V1
- Contains: 21 API endpoint classes, DTO models (173 files), client infrastructure
- Key files: `Api/RunsApi.cs`, `Api/ResultsApi.cs`, `Api/AttachmentsApi.cs`, `Api/ConfigurationsApi.cs`

**Qase.ApiClient.V2:**
- Purpose: Auto-generated wrapper around Qase TMS API V2
- Contains: Latest API endpoints with improved data structures
- Key files: `Api/ResultsApi.cs` (primary interface for result submission)

**Qase.Csharp.Commons:**
- Purpose: Core library shared by all reporters
- Contains: Configuration, domain models, abstract reporters, utilities, test context management
- Key files:
  - `ContextManager.cs`: Thread-safe context for active test
  - `ServiceCollectionExtensions.cs`: DI container setup
  - `config/ConfigFactory.cs`: Environment-based configuration (1800+ lines)
  - `reporters/CoreReporter.cs`: Primary/fallback orchestration
  - `models/domain/TestResult.cs`: Unified test result model

**Qase.NUnit.Reporter:**
- Purpose: NUnit framework integration
- Contains: Event listener implementation for NUnit Engine
- Key files: `QaseNUnitEventListener.cs` (550+ lines)
- Integration: Auto-loaded via `Qase.NUnit.Reporter.addins` manifest

**Qase.XUnit.Reporter:**
- Purpose: xUnit framework integration
- Contains: Message sink implementation for xUnit
- Key files: `QaseMessageSink.cs`
- Integration: Configured in test project RuntimeOptions

**Qase.Csharp.Commons.Tests:**
- Purpose: Comprehensive unit tests for core library
- Contains: 16 test classes, ~2000+ test cases
- Framework: xUnit + Moq + FluentAssertions
- Key test files:
  - `ConfigFactoryTests.cs`: 600+ lines, comprehensive config scenarios
  - `CoreReporterTests.cs`: Primary/fallback reporter behavior
  - `TestopsReporterTests.cs`: Cloud reporting logic
  - `ClientV1Tests.cs`: API V1 client interactions

**examples/NUnitExamples:**
- Purpose: Demonstration of Qase + NUnit integration
- Contains: Sample test classes showing attributes, steps, assertions
- Usage: Reference for implementing Qase in new NUnit projects

**examples/xUnitExamples:**
- Purpose: Demonstration of Qase + xUnit integration
- Contains: Sample test classes showing parameterized tests, steps
- Usage: Reference for implementing Qase in new xUnit projects

## Key File Locations

**Entry Points:**
- `Qase.NUnit.Reporter/QaseNUnitEventListener.cs`: NUnit integration (ITestEventListener)
- `Qase.XUnit.Reporter/QaseMessageSink.cs`: xUnit integration (IMessageSink)
- `Qase.Csharp.Commons/ContextManager.cs`: Global test context accessor

**Configuration:**
- `Qase.Csharp.Commons/config/ConfigFactory.cs`: Environment + JSON configuration
- `Qase.Csharp.Commons/config/QaseConfig.cs`: Root configuration object
- `Directory.Build.props`: Global project settings (version 1.1.4)

**Core Logic:**
- `Qase.Csharp.Commons/reporters/CoreReporter.cs`: Primary/fallback orchestration
- `Qase.Csharp.Commons/reporters/TestopsReporter.cs`: Cloud reporting implementation
- `Qase.Csharp.Commons/reporters/FileReporter.cs`: Local JSON reporting
- `Qase.Csharp.Commons/clients/ClientV1.cs`: V1 API wrapper (runs, attachments, configurations)
- `Qase.Csharp.Commons/clients/ClientV2.cs`: V2 API wrapper (results submission)

**Domain Models:**
- `Qase.Csharp.Commons/models/domain/TestResult.cs`: Main test result entity
- `Qase.Csharp.Commons/models/domain/StepResult.cs`: Step execution within test
- `Qase.Csharp.Commons/models/domain/Attachment.cs`: File/data attachment
- `Qase.Csharp.Commons/models/domain/TestResultExecution.cs`: Timing and status info

**Testing:**
- `Qase.Csharp.Commons.Tests/`: xUnit test project
- `Qase.NUnit.Reporter.Tests/`: Tests for NUnit listener
- `Qase.XUnit.Reporter.Tests/`: Tests for xUnit message sink

**Attributes & Interception:**
- `Qase.Csharp.Commons/Attributes/QaseAttribute.cs`: Marks test methods for Qase tracking
- `Qase.Csharp.Commons/Attributes/StepAttribute.cs`: Marks test steps
- `Qase.Csharp.Commons/Attributes/SuitesAttribute.cs`: Assigns suite relations
- `Qase.Csharp.Commons/Aspects/QaseAspect.cs`: AspectInjector advice for context setup
- `Qase.Csharp.Commons/Aspects/StepAspect.cs`: Step tracking via aspect interception

## Naming Conventions

**Files:**
- PascalCase: `QaseConfig.cs`, `ContextManager.cs`, `ClientV1.cs`
- Suffixes: `*Tests.cs` (test classes), `*Factory.cs` (factory pattern), `*Reporter.cs` (reporter implementations), `*Attribute.cs` (attributes), `*Aspect.cs` (aspects)
- Interface prefix: `I*` (e.g., `ICoreReporter.cs`, `IInternalReporter.cs`)

**Directories:**
- Lowercase with single purpose: `config/`, `core/`, `models/`, `reporters/`, `clients/`, `Utils/`, `writers/`, `serialization/`
- Test projects: `*.Tests` suffix (e.g., `Qase.Csharp.Commons.Tests`)
- Feature groups: Grouped by functional area, not by layer type

**Namespaces:**
- Follow directory structure: `Qase.Csharp.Commons.Config`, `Qase.Csharp.Commons.Reporters`, `Qase.Csharp.Commons.Models.Domain`
- Root namespace matches assembly name: `Qase.ApiClient.V1`, `Qase.NUnit.Reporter`

## Where to Add New Code

**New Feature (e.g., custom status mapping):**
- Primary code: `Qase.Csharp.Commons/config/` (config classes) + `Qase.Csharp.Commons/Utils/` (logic)
- Tests: `Qase.Csharp.Commons.Tests/{FeatureName}Tests.cs`
- Example: See `StatusMappingConfig.cs` and `StatusMappingUtils.cs`

**New Attribute Type:**
- Implementation: `Qase.Csharp.Commons/Attributes/{AttributeName}Attribute.cs`
- Aspect handling (if needed): `Qase.Csharp.Commons/Aspects/{FeatureName}Aspect.cs`
- Tests: `Qase.Csharp.Commons.Tests/AttributesTests.cs`

**New Reporter Mode:**
- Abstract implementation: `Qase.Csharp.Commons/reporters/IInternalReporter.cs` (update interface if needed)
- Concrete implementation: `Qase.Csharp.Commons/reporters/{ModeName}Reporter.cs`
- Integration: Update `ServiceCollectionExtensions.cs` to register new reporter
- Tests: `Qase.Csharp.Commons.Tests/{ModeName}ReporterTests.cs`

**New API Integration:**
- Regenerate API clients from OpenAPI spec (not manually added)
- Update `ClientV1.cs` or `ClientV2.cs` to expose new endpoints
- Update tests in `ClientV1Tests.cs` or `ClientV2Tests.cs`

**Domain Model Changes:**
- Add model: `Qase.Csharp.Commons/models/domain/{ModelName}.cs`
- Update TestResult if needed: `Qase.Csharp.Commons/models/domain/TestResult.cs`
- Add serialization converters if needed: `Qase.Csharp.Commons/serialization/`
- Add tests: `Qase.Csharp.Commons.Tests/{ModelName}Tests.cs`

**Utilities:**
- Shared helpers: `Qase.Csharp.Commons/Utils/` (e.g., `StatusMappingUtils.cs`)
- Core utilities: `Qase.Csharp.Commons/core/` (if framework-agnostic)
- Tests: `Qase.Csharp.Commons.Tests/{UtilityName}Tests.cs`

## Special Directories

**obj/ and bin/:**
- Purpose: Build outputs (compiled DLLs, symbols)
- Generated: Yes (during build)
- Committed: No

**Qase.ApiClient.V1/Model/ and Qase.ApiClient.V2/Model/:**
- Purpose: 173 auto-generated data transfer object classes
- Generated: Yes (from OpenAPI/Swagger spec)
- Committed: Yes (committed to version control)
- Manual changes: Avoid; regenerate if spec changes

**.planning/:**
- Purpose: GSD planning documents
- Generated: Yes (by orchestrator)
- Committed: No (added to .gitignore)

**examples/:**
- Purpose: Reference implementations
- Committed: Yes (for documentation)
- Usage: Read-only templates for new projects

---

*Structure analysis: 2026-02-26*
