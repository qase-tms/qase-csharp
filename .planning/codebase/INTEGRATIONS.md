# External Integrations

**Analysis Date:** 2026-02-26

## APIs & External Services

**Qase TestOps API:**
- **Primary Integration** - Core platform for test result reporting and project management
  - SDK/Client: `Qase.ApiClient.V1` (for runs, attachments, configurations) and `Qase.ApiClient.V2` (for result uploads)
  - Auth: `QASE_TESTOPS_API_TOKEN` (bearer token in Authorization header)
  - Endpoint: Configurable via `QASE_TESTOPS_API_HOST` (default: qase.io)
  - Base URL: `https://app.qase.io/` or `https://app-{host}/` for custom installations
  - API Client Implementation: `Qase.ApiClient.V1/Api/*Api.cs` classes (ProjectsApi, RunsApi, AttachmentsApi, ConfigurationsApi)

**Jira Integration (External Link Attachment):**
- **For external issue linking** - Attach test runs to Jira issues
  - Configuration: `QASE_TESTOPS_RUN_EXTERNAL_LINK` or separate env vars:
    - `QASE_TESTOPS_RUN_EXTERNAL_LINK_TYPE` (JiraCloud|JiraServer)
    - `QASE_TESTOPS_RUN_EXTERNAL_LINK_LINK` (issue key)
  - Type enum: `Qase.Csharp.Commons.Config.ExternalLinkType`
  - Implementation: `ClientV1.AttachExternalLinkAsync()` in `Qase.Csharp.Commons/clients/ClientV1.cs`
  - Not a direct Jira integration - passes link info to Qase API for attachment

## Data Storage

**Databases:**
- Not used - This is an SDK/client library, not a backend service

**File Storage:**
- **Local filesystem only** - For test reports and attachments
  - Reports saved to: Path configured in `QASE_REPORT_CONNECTION_PATH` (format: JSON)
  - Attachments: Uploaded to Qase TestOps, temporarily stored in system temp directory during batch processing
  - Temp directory: `Path.GetTempPath()` - used for preparing attachment files before upload
  - Max file size: 32 MB per attachment
  - Max request size: 128 MB for attachment batch
  - Max files per request: 20 files

**Caching:**
- None detected - Client library operates stateless

## Authentication & Identity

**Auth Provider:**
- Custom API token-based authentication

**Implementation:**
- Token passed in HTTP `Authorization: Bearer {token}` header
- Token source: `QASE_TESTOPS_API_TOKEN` environment variable or config file
- Configuration class: `Qase.Csharp.Commons.Config.ApiConfig`
- Token validation: `ConfigFactory.ValidateConfig()` - required for TestOps mode
- Token location: `Qase.Csharp.Commons/clients/ClientV1.cs` and `ClientV2.cs` pass token via API clients
- HTTP client setup: `Qase.ApiClient.V1/Extensions/IServiceCollectionExtensions.cs` - configures HttpClient factory

## Monitoring & Observability

**Error Tracking:**
- None built-in - Client library propagates errors as exceptions
- Custom exception type: `Qase.Csharp.Commons.Core.QaseException`
- Exception hierarchy: `QaseException` wraps API and validation errors

**Logs:**
- **Serilog structured logging** - Core logging framework
  - Configuration: `Qase.Csharp.Commons.Config.LoggingConfig`
  - Console sink: `Serilog.Sinks.Console` - log output to console
  - File sink: `Serilog.Sinks.File` - log output to file
  - Enable/disable via:
    - `QASE_LOGGING_CONSOLE` (boolean, default: true in LoggingConfig)
    - `QASE_LOGGING_FILE` (boolean, default: true in LoggingConfig)
  - Integration: `Qase.Csharp.Commons/ServiceCollectionExtensions.cs` registers Serilog with DI
  - Logger usage: Injected `ILogger<T>` throughout API clients and reporters
  - Debug logging: `QASE_DEBUG` environment variable enables extra diagnostic logs

**Observability Pattern:**
- AOP-based step execution logging via `AspectInjector`
- `StepAspect` class wraps methods marked with `[Step]` attribute
- Logs step entry, exit, duration, and exceptions
- Location: `Qase.Csharp.Commons/Aspects/StepAspect.cs`

## CI/CD & Deployment

**Hosting:**
- N/A - This is a client library distributed as NuGet packages

**CI Pipeline:**
- GitHub Actions
- File: `.github/workflows/build.yaml`
- Triggers: All push events and PRs to main
- Build environment: ubuntu-latest
- .NET version: 9.0.x
- Stages:
  1. **Test** - Runs unit tests on all test projects
  2. **Publish** - Publishes NuGet packages on git tag

**Publishing:**
- NuGet package registry: https://api.nuget.org/v3/index.json
- Packages:
  - `Qase.ApiClient.V1`
  - `Qase.ApiClient.V2`
  - `Qase.Csharp.Commons`
  - `Qase.NUnit.Reporter`
  - `Qase.XUnit.Reporter`
- API Key: `NUGET_API_KEY` secret in GitHub
- Symbol packages: Included (.snupkg format)
- Version: 1.1.4 (from `Directory.Build.props`)

## Environment Configuration

**Required env vars:**
```
QASE_MODE                              # "testops" or "report"
QASE_TESTOPS_PROJECT                   # Project code (e.g., "proj")
QASE_TESTOPS_API_TOKEN                 # API token from Qase TestOps
```

**Optional env vars:**
```
QASE_FALLBACK                          # Fallback mode if primary fails ("off", "testops", "report")
QASE_ENVIRONMENT                       # Environment slug for test run
QASE_ROOT_SUITE                        # Root suite name for result hierarchy
QASE_DEBUG                             # Enable debug logging (true/false)
QASE_TESTOPS_API_HOST                  # Custom API host (default: "qase.io")
QASE_TESTOPS_RUN_TITLE                 # Test run title
QASE_TESTOPS_RUN_DESCRIPTION           # Test run description
QASE_TESTOPS_RUN_ID                    # Existing run ID to add results to
QASE_TESTOPS_RUN_COMPLETE              # Auto-complete run (true/false)
QASE_TESTOPS_RUN_TAGS                  # Comma-separated tags
QASE_TESTOPS_SHOW_PUBLIC_REPORT_LINK   # Show public report URL (true/false)
QASE_TESTOPS_RUN_EXTERNAL_LINK         # External issue link in format "type:link"
QASE_TESTOPS_CONFIGURATIONS_VALUES     # Configurations as "name=value,name=value"
QASE_TESTOPS_CONFIGURATIONS_CREATE_IF_NOT_EXISTS  # Auto-create configs (true/false)
QASE_TESTOPS_PLAN_ID                   # Plan ID for test case filtering
QASE_TESTOPS_BATCH_SIZE                # Batch size for result upload (default: 100)
QASE_TESTOPS_STATUS_FILTER             # Comma-separated list of statuses to report
QASE_STATUS_MAPPING                    # Status mapping as "src:dst,src:dst"
QASE_REPORT_DRIVER                     # Report driver ("local")
QASE_REPORT_CONNECTION_PATH            # Output directory for local report
QASE_REPORT_CONNECTION_FORMAT          # Report format ("json")
QASE_LOGGING_CONSOLE                   # Console logging (true/false)
QASE_LOGGING_FILE                      # File logging (true/false)
```

**Secrets location:**
- Environment variables only (no secrets committed)
- CI/CD: GitHub Secrets (NUGET_API_KEY, GITHUB_TOKEN)

**Configuration files:**
- `qase.config.json` - JSON configuration file in project root or test output directory
- Schema location: See examples in `examples/NUnitExamples/qase.config.json` and `Qase.Csharp.Commons.Tests/qase.config.json`
- Priority order: File (lowest) → Environment variables (highest)

## Webhooks & Callbacks

**Incoming:**
- None - Client library does not expose HTTP endpoints

**Outgoing:**
- Qase API calls only (REST HTTP)
- No webhooks or callbacks from Qase TestOps
- Test result reporting is pull-based (reporter sends results)

## API Endpoints Used

**Qase TestOps API V1 (via Qase.ApiClient.V1):**
- `POST /project/{projectCode}/run` - Create test run
- `PUT /project/{projectCode}/run/{runId}` - Complete test run
- `GET /project/{projectCode}/run/{runId}` - Get run details (for case filtering)
- `POST /project/{projectCode}/run/{runId}/attachment` - Upload attachments
- `GET /project/{projectCode}/configuration` - List configurations
- `POST /project/{projectCode}/configuration` - Create configuration group
- `POST /project/{projectCode}/configuration/{groupId}/configuration` - Create configuration
- `PUT /project/{projectCode}/run/{runId}/public` - Set run as public
- `PUT /project/{projectCode}/run/{runId}/external-issues` - Attach external issue link

**Qase TestOps API V2 (via Qase.ApiClient.V2):**
- Result upload endpoints (V2 client prepared for future use)

## HTTP Client Configuration

**Resilience Policies (Polly):**
- Retry policy: Configurable via `AddRetryPolicy()` extension
- Timeout policy: Configurable via `AddTimeoutPolicy()` extension
- Circuit breaker: Configurable via `AddCircuitBreakerPolicy()` extension
- Location: `Qase.ApiClient.V1/Extensions/IHttpClientBuilderExtensions.cs`
- Applied to named HttpClient for API calls

**Headers:**
- Standard HTTP headers configured in API clients
- User-Agent: Built with version and client info via `ClientHeadersBuilder` in `Qase.Csharp.Commons/Utils/`

---

*Integration audit: 2026-02-26*
