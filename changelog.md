# Changelog

## qase-csharp 2.0.0

**Breaking:** `Qase.MSTest.Reporter` targets MSTest 4 and Microsoft Testing Platform 2. Version 1.2.1 crashed the test host of an MSTest 4 project (`TypeLoadException` for `IDataConsumer`), so no tests ran. MSTest 3 projects should stay on 1.2.x: restoring 2.0.0 there fails with `NU1605`. See the [upgrade guide](Qase.MSTest.Reporter/docs/UPGRADE.md).

- With the .NET 10 SDK, `dotnet test` needs `"test": { "runner": "Microsoft.Testing.Platform" }` in `global.json`; see the MSTest reporter README
- Attributes, configuration, result signatures and the report format are unchanged: results keep linking to the same test cases
- The other reporters have no changes; their version follows the shared release version

## qase-csharp 1.2.1

- `rootSuite` / `QASE_ROOT_SUITE` now adds a top-level suite to every result; it was read but never applied. The signature does not include it, so changing it does not relink existing cases
- `logging.file` follows `debug` when it is not set explicitly, as documented
- Reqnroll reports the steps skipped after a failed step as `Skipped`; they were missing from the result, because Reqnroll runs no step hooks for them
- `[Step]` methods returning `ValueTask` or `ValueTask<T>` are completed when the step finishes, not when it starts
- xUnit v2 honours `[Fact(DisplayName = ...)]` and `[Theory(DisplayName = ...)]`
- A custom MSTest/xUnit v3 display name with parentheses, such as `Login (admin)`, is no longer parsed as parameter values
- MSTest resolves overloads with generic parameter types correctly
- A failed test run completion no longer writes an empty local report when every result was already uploaded
- `testops.plan.id` case lookup uses the test plan API instead of fetching a run with the plan's id
- MSTest and xUnit v3 extensions report the package version instead of `1.0.0`
- Removed the non-existent `testops.api.enterprise` option from the documentation and corrected the xUnit v2 status table

## qase-csharp 1.2.0

**Breaking:** the result signature now includes the test method (or Reqnroll scenario) name: `ids::suites::method::params`. Two tests without Qase IDs in the same class used to share one signature and were merged into one test case. Tests without `[QaseIds]` will be linked to newly created cases on the first run after the upgrade.

- A broken configuration no longer aborts the test run. An empty variable (`QASE_TESTOPS_RUN_ID=`, typical for unset CI inputs) is treated as unset, an invalid number, boolean or enum value is ignored with a warning, and `mode`/`fallback`/`report.driver`/`report.connection.local.format` are case-insensitive (`QASE_MODE=TestOps`). Previously xUnit v3 and MSTest ran zero tests, xUnit v2 hit a catastrophic failure and NUnit/Reqnroll failed every test
- Boolean variables also accept `1`/`0`, `yes`/`no` and `on`/`off`
- The batch size is clamped to the documented `1`–`200`
- With `mode: testops` and no token or project, the reporter now falls back to the local report when `fallback: report` is set, instead of reporting nothing; `fallback: testops` without credentials no longer disables `mode: report`
- `testops.showPublicReportLink` is read at the documented level; `testops.run.showPublicReportLink` keeps working
- The API token is masked in debug logs; it used to be written in full to the console and to the log file
- Fixed xUnit v3 Theory results: parameter values carried their names (`query: "query: usb hub"`), which corrupted the parameters and the signature and dropped the steps, comments and attachments of every Theory row
- xUnit v3 plain tests are titled by the method name, like in the other reporters, instead of the fully qualified name
- Steps, comments and attachments are no longer lost for parameterized tests with `decimal`, `double`, enum or empty-string arguments, or for tests inherited from a base class; class-level Qase attributes are inherited too
- Comments and attachments no longer leak into a later result with the same name (retries, repeats, Reqnroll scenarios with the same title in different features)
- NUnit identifies tests by the `classname`/`methodname` NUnit reports, so parameterized fixtures (`[TestFixture(1)]`) and `TestName` overrides keep their Qase attributes; NUnit numeric literals are reported without the type suffix (`29.99`, not `29.99d`)
- NUnit `Warning` results (`Assert.Warn`) are reported as passed instead of skipped; the raw NUnit event log is written only with `debug: true`
- Failures from FluentAssertions, Shouldly and other assertion libraries are reported as failed instead of invalid under NUnit
- MSTest and xUnit v3 report timed-out tests as failed and cancelled tests as skipped; they used to be dropped
- A network error or timeout when uploading results now switches to the fallback report instead of losing the results; Qase errors never fail the test session in MSTest, xUnit v2 and xUnit v3
- Results reported from parallel tests are no longer lost or uploaded twice, and a result is no longer duplicated in the fallback report after a failed batch upload
- A failed attachment upload no longer fails the whole batch: the result is reported without it. More than 20 attachments per result are split into several requests, and an oversized file is skipped with a warning
- Attachments are uploaded with their MIME type, so images and text files are previewed in Qase instead of being stored as `application/octet-stream`
- In-memory attachments are no longer written to the temp directory, which could overwrite and then delete a user's file when the attachment name was a path
- A non-existent `environment` no longer prevents the test run from being created: the run is created without it and a warning is logged
- `[Step]` methods rethrow the step's own exception with its original stack trace
- The `Retry-After` delay is capped at 10 minutes, the run start time no longer depends on the machine's calendar, and the default run title is `Automated run <date>` as documented
- The local JSON report is written without a UTF-8 BOM, and the log file is named `logs/qase-<date>.log` instead of `logs/<date><date>.log`

## qase-csharp 1.1.30

- Fixed run and result links for enterprise hosts: with `testops.api.host` set to `example.qase.io` the completed run link pointed at `https://app-example.qase.io/`, which does not exist. Enterprise links now point at `https://example.qase.io/`, while the EU cluster (`eu.qase.io`) keeps linking to `https://app-eu.qase.io/`
- Fixed failed-result links in the log pointing at `https://eu.qase.io` instead of `https://app-eu.qase.io` for the EU cluster

## qase-csharp 1.1.29

- Fixed every reporter crashing when `mode` is `off`: the core reporter was resolved with a required internal reporter, which is only registered for `testops` and `report`, so the run failed with `No service for type 'IInternalReporter' has been registered`. This surfaced as a catastrophic failure in xUnit v2, NUnit and Reqnroll, which build the reporter unconditionally
- `mode: off` now keeps the reporters fully silent: no service container, no log file and no Qase output in the test run

## qase-csharp 1.1.28

- Fixed the xUnit v2 reporter never reporting results on Linux: xUnit v2 discovers runner reporters by scanning the test output folder for `*reporters*.dll`, and that glob is case-sensitive on Linux, so `Qase.XUnit.Reporters.dll` was silently skipped and the run fell back to the default reporter. The assembly is now named `qase.xunit.reporters.dll` and is discovered on every platform. Public types, namespaces and configuration are unchanged, but run a clean build after upgrading so the previous assembly is removed from the output folder

## qase-csharp 1.1.27

- Fixed `testops.plan.id` being read from the configuration but never sent when creating a test run — the run is now created from the given test plan across all five reporters (NUnit, MSTest, xUnit v2, xUnit v3, Reqnroll)

## qase-csharp 1.1.26

- Added retries for the test results upload, covering transport failures and HTTP 408, 429 and 5xx, with an exponential backoff that honours the `Retry-After` header
- Added a configurable request timeout, defaulting to 30 seconds
- Added the `testops.api.timeout`, `testops.api.retries` and `testops.api.retryBackoff` options, with `QASE_TESTOPS_API_TIMEOUT`, `QASE_TESTOPS_API_RETRIES` and `QASE_TESTOPS_API_RETRY_BACKOFF` overrides
- A test run is now left open when a batch of results cannot be uploaded, and the number of lost results is reported
- Fixed the default batch size, which was `0` instead of the documented `200` and made every result its own request

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
