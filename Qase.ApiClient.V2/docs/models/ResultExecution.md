# Qase.ApiClient.V2.Model.ResultExecution

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Status** | **string** | Can have the following values passed, failed, blocked, skipped, invalid + custom statuses | 
**StartTime** | **double** | Unix epoch time in seconds (whole part) and milliseconds (fractional part). | [optional] 
**EndTime** | **double** | Unix epoch time in seconds (whole part) and milliseconds (fractional part). | [optional] 
**Duration** | **long** | Duration of the test execution in milliseconds. | [optional] 
**Stacktrace** | **string** |  | [optional] 
**ErrorContext** | **string** | Free-form failure context captured by the reporter. For Playwright this is the content of error-context.md (test info, error details, page snapshot), so it may include rendered page content. Stored verbatim so it can be copied as raw text. Values longer than 262144 characters are silently truncated by Qase and the request still succeeds. Write-only — not returned by the result read endpoints. | [optional] 
**Thread** | **string** |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

