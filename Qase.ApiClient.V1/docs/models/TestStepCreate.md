# Qase.ApiClient.V1.Model.TestStepCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Action** | **string** | Step action text. Used for classic steps. For gherkin steps, use the \&quot;value\&quot; property instead. | [optional] 
**ExpectedResult** | **string** |  | [optional] 
**Data** | **string** |  | [optional] 
**Value** | **string** | Gherkin scenario text. Used when steps_type is \&quot;gherkin\&quot;. Example: \&quot;Given a user exists\\nWhen they log in\\nThen they see the dashboard\&quot; | [optional] 
**Position** | **int** |  | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | **List&lt;Object&gt;** | Nested steps may be passed here. Use same structure for them. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

