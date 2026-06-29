# Qase.ApiClient.V1.Model.TestStepResult

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Status** | **int** | 1 - passed, 2 - failed, 3 - blocked, 5 - skipped, 7 - in_progress | [optional] 
**Position** | **int** |  | [optional] 
**Comment** | **string** | Comment left for the step. | [optional] 
**StartTime** | **long** | Unix timestamp of the step start time. | [optional] 
**EndTime** | **long** | Unix timestamp of the step end time. | [optional] 
**DurationMs** | **long** | Step duration in milliseconds. | [optional] 
**Attachments** | [**List&lt;Attachment&gt;**](Attachment.md) |  | [optional] 
**Steps** | [**List&lt;TestStepResult&gt;**](TestStepResult.md) | Nested steps results will be here. The same structure is used for them. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

