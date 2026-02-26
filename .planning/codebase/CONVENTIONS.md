# Coding Conventions

**Analysis Date:** 2026-02-26

## Naming Patterns

**Files:**
- PascalCase for class files: `ClientV1.cs`, `QaseAttribute.cs`, `CoreReporter.cs`
- Interfaces prefixed with `I`: `IClient.cs`, `IInternalReporter.cs`, `ICoreReporter.cs`
- Test files suffixed with `Tests`: `ClientTests.cs`, `CoreReporterTests.cs`, `StatusMappingUtilsTests.cs`
- Static utility classes in `Utils` directory: `StatusMappingUtils.cs`, `Signature.cs`, `ClientHeadersBuilder.cs`
- Configuration classes in `config` directory: `QaseConfig.cs`, `TestOpsConfig.cs`, `ReportConfig.cs`

**Namespaces:**
- Company.Project.Module pattern: `Qase.Csharp.Commons.Config`, `Qase.Csharp.Commons.Reporters`, `Qase.Csharp.Commons.Models.Domain`
- Subdirectory structure matches namespace hierarchy
- Examples: `Qase.Csharp.Commons/config/` contains `namespace Qase.Csharp.Commons.Config`

**Classes:**
- PascalCase for class names: `ClientV1`, `CoreReporter`, `QaseConfig`, `TestResult`
- Interfaces use PascalCase with `I` prefix: `IClient`, `IInternalReporter`, `ICoreReporter`, `IQaseAttribute`
- Abstract base pattern used sparingly; inheritance through interfaces preferred

**Methods:**
- camelCase method names: `startTestRun()`, `completeTestRun()`, `addResult()`, `uploadResults()` in interfaces/reporters
- PascalCase method names in public classes: `CreateTestRunAsync()`, `CompleteTestRunAsync()`, `UploadAttachmentsAsync()`
- Async methods suffixed with `Async`: `CreateTestRunAsync()`, `UploadAttachmentsAsync()`, `GetConfigurationIdsAsync()`
- Private/helper methods prefixed with underscore in fields: `_logger`, `_config`, `_client`
- Private methods use PascalCase with no underscore: `PrepareAttachmentFileAsync()`, `ValidateAttachments()`, `CreateBatches()`

**Properties:**
- PascalCase for public properties: `Mode`, `Fallback`, `Environment`, `RootSuite`, `Debug`
- Auto-properties with getters/setters: `public string? Environment { get; set; }`
- Nullable reference types enabled with `?` notation: `public string? Environment { get; set; }`

**Variables:**
- camelCase for local variables: `runId`, `fileInfo`, `response`, `expectedIds`
- Tuple fields use camelCase: `(Stream Stream, string FileName, long Size)`
- Constants use UPPER_SNAKE_CASE: `MaxFileSize`, `MaxRequestSize`, `MaxFilesPerRequest`

**Enums:**
- PascalCase for enum names: `Mode`, `TestResultStatus`, `StepResultStatus`, `Format`, `Driver`
- PascalCase for enum values: `Off`, `TestOps`, `Report`, `Passed`, `Failed`, `Skipped`

**Type Naming:**
- Record types for immutable data follow naming: Configuration data models as classes with properties
- Domain models in `Models.Domain` namespace: `TestResult`, `TestResultExecution`, `Attachment`, `Suite`, `SuiteData`

## Code Style

**Formatting:**
- LangVersion set to 10 in `Directory.Build.props` (C# 10.0)
- Nullable reference types enabled: `<Nullable>enable</Nullable>` in project files
- Implicit usings enabled: `<ImplicitUsings>enable</ImplicitUsings>` in test projects
- Brace style: Allman style for public members, Kernighan & Ritchie for inline conditions
  ```csharp
  public async Task UploadResultsAsync()
  {
      // Implementation
  }

  if (condition) {
      // Inline simple condition
  }
  ```

**Linting:**
- No explicit .editorconfig file detected
- Directory.Build.props suppresses warnings: `<NoWarn>$(NoWarn);0612</NoWarn>`
- SourceLink enabled for all packages: `Microsoft.SourceLink.GitHub` PackageReference
- Debug symbols included in releases: `<IncludeSymbols>true</IncludeSymbols>`

## Import Organization

**Order:**
1. System namespaces: `using System;`, `using System.Collections.Generic;`, `using System.Linq;`
2. System.xxx extensions: `using System.Threading.Tasks;`, `using System.Reflection;`
3. Third-party libraries: `using Microsoft.Extensions.Logging;`, `using AspectInjector.Broker;`, `using Moq;`
4. Generated/API clients: `using Qase.ApiClient.V1.Api;`, `using Qase.ApiClient.V1.Model;`
5. Local project namespaces: `using Qase.Csharp.Commons.Config;`, `using Qase.Csharp.Commons.Core;`

**Examples from codebase:**
- `ClientV1.cs` imports:
  ```csharp
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Logging;
  using Qase.ApiClient.V1.Api;
  using Qase.ApiClient.V1.Model;
  using Qase.Csharp.Commons.Config;
  ```

- Test file imports:
  ```csharp
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using Moq;
  using Qase.Csharp.Commons.Config;
  using Xunit;
  ```

**No explicit barrel files** used. Imports are direct to specific types.

## Error Handling

**Patterns:**
- Custom exception `QaseException` for domain-level errors: `public class QaseException : Exception`
- Wrapped exceptions with inner exception: `throw new QaseException($"Failed to create test run: {ex.Message}", ex);`
- Exception filtering in catch blocks: `catch (Exception ex) when (!(ex is QaseException))`
- Graceful degradation with try-catch-finally for resource cleanup

Example from `ClientV1.cs`:
```csharp
try
{
    _logger.LogDebug("Uploading {Count} attachments", attachments.Count);
    // Operation
}
catch (QaseException)
{
    throw;
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error uploading attachments");
    throw new QaseException($"Failed to upload attachments: {ex.Message}", ex);
}
finally
{
    // Cleanup resources
    foreach (var fileInfo in fileInfos)
    {
        try
        {
            fileInfo.Stream?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to dispose stream");
        }
    }
}
```

**Null handling:**
- Nullable reference types: `string?`, `long?`, `List<T>?`
- Null-coalescing: `resp.Ok()?.Result?.Id ?? throw new QaseException(...)`
- Null-conditional operators: `fileInfo?.Stream?.Dispose()`

## Logging

**Framework:** `Microsoft.Extensions.Logging` (ILogger<T>)

**Patterns:**
- Injected via constructor: `private readonly ILogger<ClientV1> _logger;`
- Structured logging with named placeholders:
  ```csharp
  _logger.LogDebug("Sending request to create test run with data: {@RunData}", runData);
  _logger.LogInformation("Test run completed successfully. Link: {Url}run/{Project}/dashboard/{RunId}",
      _url, _config.TestOps.Project, runId);
  _logger.LogError(ex, "Failed to dispose stream");
  ```
- Log levels used:
  - `LogDebug()` for detailed operational information
  - `LogInformation()` for high-level important information
  - `LogWarning()` for warnings
  - `LogError(ex, ...)` for errors with exception

**Example:**
```csharp
_logger.LogDebug("Uploading {Count} attachments", attachments.Count);
_logger.LogDebug("Created {BatchCount} batches for upload", batches.Count);
_logger.LogError("Failed to upload attachment batch: {Reason}", resp.ReasonPhrase + " " + resp.RawContent);
```

## Comments

**When to Comment:**
- XML documentation for public classes and methods: `/// <summary>`, `/// <param>`, `/// <returns>`
- Complex algorithms receive explanatory comments
- Non-obvious validation logic is explained
- Business rules are documented

**JSDoc/TSDoc Style (C# equivalent - XML Documentation):**
```csharp
/// <summary>
/// Uploads attachments to Qase TMS
/// </summary>
/// <param name="attachments">The attachments to upload</param>
/// <returns>List of hashes of the uploaded attachments</returns>
/// <exception cref="QaseException">Thrown when validation fails or upload fails</exception>
public async Task<List<string>> UploadAttachmentsAsync(List<Models.Domain.Attachment> attachments)
{
    // Implementation
}
```

**Observable in `ClientV1.cs`:**
- All public methods have `/// <summary>` documentation
- Constructor parameters documented with `/// <param>`
- `/// <inheritdoc />` used for interface implementations to reuse parent documentation
- Private helper methods may omit documentation but have inline comments for complex logic

## Function Design

**Size:**
- Most public methods 20-50 lines of code
- Larger methods (100+ lines) are well-structured with helper methods: `ClientV1.UploadAttachmentsAsync()` uses private `ValidateAttachments()`, `PrepareAttachmentFileAsync()`, `CreateBatches()`
- Helper methods extract validation, resource management, and batch processing logic

**Parameters:**
- Limited to 3-5 parameters for public methods
- Configuration and dependencies injected via constructor
- Parameter names are descriptive: `runId`, `attachment`, `configItem`
- Use of tuples for returning multiple values: `(Stream Stream, string FileName, long Size)?`

**Return Values:**
- Explicit types, no implicit returns in C#
- Async methods return `Task` or `Task<T>`
- Nullable return types marked: `Task<string>`, `List<long>?`
- Early return pattern used for validation:
  ```csharp
  if (attachments == null || attachments.Count == 0)
  {
      _logger.LogWarning("No attachments provided for upload");
      return new List<string>();
  }
  ```

## Module Design

**Exports:**
- `public` keyword explicitly marks exported types
- Internal classes use default (`internal`) access
- Interfaces define contracts: `IClient`, `ICoreReporter`, `IInternalReporter`

**Folder structure implies module boundaries:**
- `Attributes/` - Attribute classes: `QaseAttribute.cs`, `StepAttribute.cs`, `TitleAttribute.cs`
- `Config/` - Configuration classes: `QaseConfig.cs`, `TestOpsConfig.cs`, `BatchConfig.cs`
- `Models/Domain/` - Domain models: `TestResult.cs`, `Attachment.cs`, `Suite.cs`
- `Reporters/` - Reporter implementations: `CoreReporter.cs`, `TestopsReporter.cs`
- `Clients/` - API client implementations: `ClientV1.cs`, `ClientV2.cs`
- `Utils/` - Utility functions: `StatusMappingUtils.cs`, `Signature.cs`

**No barrel files/index files:** Imports are direct to specific classes.

---

*Convention analysis: 2026-02-26*
