# Changelog

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
