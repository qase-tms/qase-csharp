# Changelog

## qase-csharp 1.1.26

- Added retries for the test results upload, covering transport failures and HTTP 408, 429 and 5xx, with an exponential backoff that honours the `Retry-After` header
- Added a configurable request timeout, defaulting to 30 seconds
- Added the `testops.api.timeout`, `testops.api.retries` and `testops.api.retryBackoff` options, with `QASE_TESTOPS_API_TIMEOUT`, `QASE_TESTOPS_API_RETRIES` and `QASE_TESTOPS_API_RETRY_BACKOFF` overrides
- A test run is now left open when a batch of results cannot be uploaded, and the number of lost results is reported

## qase-csharp 1.1.25

- Updated API clients to the latest specification

## qase-csharp 1.1.24

- Updated API clients to the latest specification

## qase-csharp 1.1.23

- Updated API clients to the latest specification

## qase-csharp 1.1.22

- Updated API clients to the latest specification

## qase-csharp 1.1.21

- Updated API clients to the latest specification


## qase-csharp 1.1.20

- Fixed build file lock that blocked rebuilds in projects referencing a Qase reporter — downgraded `AspectInjector` to 2.8.2, avoiding the MSBuild Task in 2.9.0 that held a handle on the output assembly and caused the lock during the `CreateAppHost` step ([pamidur/aspect-injector#244](https://github.com/pamidur/aspect-injector/issues/244))

## qase-csharp 1.1.19

- Updated API clients to the latest specification

## qase-csharp 1.1.18

- Refactored all 5 reporters (NUnit, MSTest, xUnit v2, xUnit v3, Reqnroll) to use shared `TestResultBuilder` pipeline — shared logic for type resolution, attribute extraction, parameter parsing, suite hierarchy, failure classification, ContextManager integration, and signature generation is now centralized in Commons
- Added `RawTestData` intermediate model — each reporter fills framework-specific fields, builder assembles the complete `TestResult`
- Added `FailureClassifier` — unified assertion vs runtime error detection replacing separate implementations in NUnit and xUnit reporters
- Added `DisplayNameGenerator` — shared ContextManager key generation
- Fixed empty parameter values causing Qase API v2 to reject entire batch upload — empty string params (e.g. `email=""`) are now replaced with `"empty"`, preventing silent fallback to local file reporting
- Added 29 new unit tests for builder, failure classifier, and display name generator

## qase-csharp 1.1.17

- Improved API error logging across all V1 API calls — error responses now include HTTP status code and full response body with detailed error messages from the Qase API, making it much easier to diagnose issues like invalid environment slugs, missing projects, or authentication problems

## qase-csharp 1.1.16

- Fixed detection of all reporter types in HostInfo
- Added Tags attribute usage documentation to all reporter docs

## qase-csharp 1.1.15

- Added `[Tags]` attribute for assigning tags to test cases from test code
- Tags support on both class and method levels with merge semantics (class + method tags are combined)
- Added `@QaseTags:tag1,tag2` Gherkin tag support for Reqnroll reporter
- Tags are passed to Qase API v2 via `ResultCreateFields.Tags` (comma-separated)
- Added tags to example projects for all frameworks (NUnit, xUnit, xUnit v3, MSTest, Reqnroll)
- Updated expected YAML files for integration testing with tags validation

## qase-csharp 1.1.14

- Updated API clients to the latest specification


## qase-csharp 1.1.13

- Added Qase Reqnroll Reporter for [Reqnroll](https://reqnroll.net/) BDD framework integration
- Automatic step reporting from Given/When/Then Gherkin steps — no code changes required
- Gherkin tag-based metadata: `@QaseId`, `@QaseTitle`, `@QaseFields`, `@QaseSuite`, `@QaseIgnore`
- Scenario Outline parameter capture from Examples tables
- Suite hierarchy from Feature name or `@QaseSuite` tag override
- Attachments and comments via `Metadata.Attach()` / `Metadata.Comment()` from step definitions
- Auto-discovered as Reqnroll runtime plugin (`*.ReqnrollPlugin.dll` naming convention)
- Added Reqnroll example project with 6 feature files demonstrating all reporter capabilities
- Added Reqnroll reporter documentation (README, usage guide, steps, attachments)

## qase-csharp 1.1.12

- Added `User-Agent: qase-api-client-csharp/<version>` header to all API requests (V1 and V2 clients)
- Fixed `FormatVersion` to trim trailing `.0` from 4-part assembly versions

## qase-csharp 1.1.11

- Fixed StepAspect swallowing exceptions in sync and async step wrappers — tests with failing steps were incorrectly marked as passed
- Fixed async steps being marked as passed before the Task actually completed
- Fixed `InvalidCastException` for `void` and non-generic `Task` step methods by adding dedicated wrappers
- Re-throw exceptions from steps to ensure tests are properly marked as failed

## qase-csharp 1.1.10

- Updated API clients to the latest specification versions

## qase-csharp 1.1.9

- Unified HostData model to align field names across all Qase reporter languages
- Renamed `Csharp` -> `Language` and `BuildTool` -> `PackageManager` in HostInfoModel
- Added `Framework`, `Commons`, `ApiClientV1`, `ApiClientV2` properties to HostInfoModel
- Normalized `system` field: macOS -> `darwin`, Windows -> `windows`, Linux -> `linux`
- X-Platform headers retain language-specific keys (`csharp=`, `nuget=`)

## qase-csharp 1.1.8

- Fixed `TestopsReporter` ignoring `QASE_TESTOPS_RUN_COMPLETE` setting — test runs were always marked as complete regardless of environment variable or config file value
- Fixed `ConfigFactory` JSON parser not handling `"complete": false` in `qase.config.json` — only `true` was recognized, `false` was silently ignored

## qase-csharp 1.1.7

- Added Qase xUnit v3 Reporter for xUnit v3 with Microsoft.Testing.Platform (MTP v2) integration
- Supports all Qase attributes: `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`
- Step tracking via `[Step]`/`[Qase]` attributes through ContextManager
- Attachments via `Metadata.Attach()` and comments via `Metadata.Comment()`
- Parameterized test support with `[Theory]`, `[InlineData]`, and `[MemberData]`
- Native `TestMethodIdentifierProperty` extraction (no VSTest bridge needed)
- Auto-registration via `TestingPlatformBuilderHook` and MSBuild `.props` import
- Added xUnit v3 example project with comprehensive test coverage
- Added xUnit v3 reporter documentation (README, usage guide, steps, attachments)
- Requires `global.json` with `"test": {"runner": "Microsoft.Testing.Platform"}` on .NET 10 SDK

## qase-csharp 1.1.6

- Extracted shared `SuiteParser` utility into Commons for consistent suite hierarchy extraction across all reporters
- Extracted shared `TypeMethodResolver` utility into Commons with `ConcurrentDictionary` caching for assembly type/method resolution
- Extracted shared `AttributeExtractor` utility into Commons for unified Qase attribute extraction (`[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`)
- Extracted shared `ParameterParser` utility into Commons for consistent parameter value parsing across reporters
- Refactored xUnit, NUnit, and MSTest reporters to use Commons utilities instead of inline implementations
- Removed dead private methods from MSTest and NUnit reporters (`ExtractParameterValuesFromDisplayName`, `ParseSuiteFromFullName`, `ExtractParameterValuesFromName`)
- Removed duplicate attribute extraction tests from xUnit and NUnit reporter test projects (covered by Commons tests)

## qase-csharp 1.1.5

- Added Qase MSTest Reporter for MSTest v3+ with Microsoft.Testing.Platform (MTP v2) integration
- Supports all Qase attributes: `[QaseIds]`, `[Title]`, `[Fields]`, `[Suites]`, `[Ignore]`
- Step tracking via `[Step]`/`[Qase]` attributes through ContextManager
- Attachments via `Metadata.Attach()` and comments via `Metadata.Comment()`
- Parameterized test support with `[DataRow]` and `[DynamicData]`
- Test signature generation for cross-run correlation

## qase-csharp 1.1.3

- Fixed NUnit reporter incorrectly splitting `fullName` on dots inside decimal parameter values, which caused test method name fragments to appear in suite data and wrong display names for ContextManager lookups
- Fixed thread-safety issue in `ContextManager` by replacing `Dictionary` with `ConcurrentDictionary` for safe parallel test execution
- Added logging to `FileWriter` for diagnosing attachment write failures
- Fixed `FileWriter.Prepare()` to clean results and attachments directories before each test run, preventing stale data accumulation
- Fixed MIME type detection for attachments by setting `FileName` when creating `Attachment` from file path

## qase-csharp 1.1.2

- Fixed step `step_type` field to default to `"text"` so it is always present in report JSON
- Fixed step `duration` computation using `Stop()` method
- Added attachment saving to `attachments/` directory (file copy, string content, byte content)
- Added `suites` field to `run.json` populated from test result relations
- Added `host_data` field to `run.json` with machine info

## qase-csharp 1.1.1

- Added custom `SnakeCaseNamingPolicy` for `netstandard2.0` JSON serialization
- Added `LowercaseEnumConverter<T>` for enum-to-lowercase-string serialization
- Added report model classes (`Run`, `RunStats`, `RunExecution`, `ShortResult`) for `run.json`
- Rewritten `FileReporter` for directory-based output with custom serialization
- Refactored `FileWriter` to write `run.json`, individual results, and attachments
- Made `StepExecution.StartTime` nullable to match spec
- Excluded non-spec fields from JSON serialization (`RunId`, `Ignore`, `ContentBytes`, `Comment`)

## qase-scharp 1.0.17

- Fixed an issue where the test run link was not being generated correctly when filtering by status.

## qase-scharp 1.0.16

- Added support for uploading multiple attachments in a single request

## qase-scharp 1.0.15

- Updated API clients to the latest specification versions

## qase-scharp 1.0.14

- Added support for showing public report link after test run completion

## qase-scharp 1.0.13

- Added support for logging configuration

## qase-scharp 1.0.12

- Added support for status mapping

## qase-scharp 1.0.11

- Updated API clients to the latest specification versions

## qase-scharp 1.0.10

- Added support for external link configuration
- Added support for status filter configuration

## qase-scharp 1.0.9

- Improved the logic for determining the status of a test result
- Added support for the `Invalid` status

## qase-scharp 1.0.7

- Updated API clients to the latest specification versions

## qase-scharp 1.0.6

- Added support for test run configurations

## qase-scharp 1.0.5

- Added support for file attachments upload

## qase-scharp 1.0.4

- Added support for steps
- Added support for comments

## qase-scharp 1.0.3

- Fixed a link to failed test in the console output

## qase-scharp 1.0.2

- Added signature generation for test cases
- Added support for test run tags

## What's new

- Added API clients for Qase API v1 and v2
