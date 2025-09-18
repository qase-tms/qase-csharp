# Qase.ApiClient.V1.Model.TestCaseUpdate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Description** | **string** |  | [optional] 
**Preconditions** | **string** |  | [optional] 
**Postconditions** | **string** |  | [optional] 
**Title** | **string** |  | [optional] 
**Severity** | **int** |  | [optional] 
**Priority** | **int** |  | [optional] 
**Behavior** | **int** |  | [optional] 
**Type** | **int** |  | [optional] 
**Layer** | **int** |  | [optional] 
**IsFlaky** | **int** |  | [optional] 
**SuiteId** | **long** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**Automation** | **int** |  | [optional] 
**Status** | **int** |  | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | [**List&lt;TestStepCreate&gt;**](TestStepCreate.md) |  | [optional] 
**Tags** | **List&lt;string&gt;** |  | [optional] 
**Params** | **Dictionary&lt;string, List&lt;string&gt;&gt;** | Deprecated, use &#x60;parameters&#x60; instead. | [optional] 
**Parameters** | [**List&lt;TestCaseParameterCreate&gt;**](TestCaseParameterCreate.md) |  | [optional] 
**CustomField** | **Dictionary&lt;string, string&gt;** | A map of custom fields values (id &#x3D;&gt; value) | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

