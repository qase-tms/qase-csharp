# Qase.ApiClient.V1.Model.ReviewProposedStep

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Action** | **string** | Step action text. Used for classic steps. For gherkin steps, use the \&quot;value\&quot; property instead. | [optional] 
**ExpectedResult** | **string** |  | [optional] 
**Data** | **string** |  | [optional] 
**Value** | **string** | Gherkin scenario text. Used when steps_type is \&quot;gherkin\&quot;. | [optional] 
**Shared** | **string** | Hash of the referenced shared step. | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | **List&lt;Object&gt;** | Nested steps use the same structure. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

