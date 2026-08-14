# Qase.ApiClient.V1.Model.ReviewStepData
A step of the proposed test case. When `steps_type` is `gherkin` the step carries the scenario in `value` and nothing else: a non-empty `action`, `expected_result`, `data`, `attachments`, `shared` or nested `steps` is rejected.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Action** | **string** | Step action text. Classic steps only. | [optional] 
**Shared** | **string** | Hash of an existing shared step to insert at this position. | [optional] 
**ExpectedResult** | **string** |  | [optional] 
**Data** | **string** |  | [optional] 
**Value** | **string** | Gherkin scenario text. Used when steps_type is \&quot;gherkin\&quot;. Example: \&quot;Given a user exists\\nWhen they log in\\nThen they see the dashboard\&quot; | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | **List&lt;Object&gt;** | Nested steps may be passed here. Use same structure for them. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

