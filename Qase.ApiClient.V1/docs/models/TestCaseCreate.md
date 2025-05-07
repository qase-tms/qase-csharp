# Qase.ApiClient.V1.Model.TestCaseCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Title** | **string** |  | 
**Description** | **string** |  | [optional] 
**Preconditions** | **string** |  | [optional] 
**Postconditions** | **string** |  | [optional] 
**Severity** | **int** |  | [optional] 
**Priority** | **int** |  | [optional] 
**Behavior** | **int** |  | [optional] 
**Type** | **int** |  | [optional] 
**Layer** | **int** |  | [optional] 
**IsFlaky** | **int** |  | [optional] 
**AuthorId** | **int** |  | [optional] 
**SuiteId** | **long** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**Automation** | **int** |  | [optional] 
**Status** | **int** |  | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | [**List&lt;TestStepCreate&gt;**](TestStepCreate.md) |  | [optional] 
**Tags** | **List&lt;string&gt;** |  | [optional] 
**Params** | **Dictionary&lt;string, List&lt;string&gt;&gt;** |  | [optional] 
**CustomField** | **Dictionary&lt;string, string&gt;** | A map of custom fields values (id &#x3D;&gt; value) | [optional] 
**CreatedAt** | **string** |  | [optional] 
**UpdatedAt** | **string** |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

