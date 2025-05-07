# Qase.ApiClient.V1.Model.TestCase

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **long** |  | [optional] 
**Position** | **int** |  | [optional] 
**Title** | **string** |  | [optional] 
**Description** | **string** |  | [optional] 
**Preconditions** | **string** |  | [optional] 
**Postconditions** | **string** |  | [optional] 
**Severity** | **int** |  | [optional] 
**Priority** | **int** |  | [optional] 
**Type** | **int** |  | [optional] 
**Layer** | **int** |  | [optional] 
**IsFlaky** | **int** |  | [optional] 
**Behavior** | **int** |  | [optional] 
**Automation** | **int** |  | [optional] 
**Status** | **int** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**SuiteId** | **long** |  | [optional] 
**CustomFields** | [**List&lt;CustomFieldValue&gt;**](CustomFieldValue.md) |  | [optional] 
**Attachments** | [**List&lt;Attachment&gt;**](Attachment.md) |  | [optional] 
**StepsType** | **string** |  | [optional] 
**Steps** | [**List&lt;TestStep&gt;**](TestStep.md) |  | [optional] 
**Params** | [**TestCaseParams**](TestCaseParams.md) |  | [optional] 
**Tags** | [**List&lt;TagValue&gt;**](TagValue.md) |  | [optional] 
**MemberId** | **long** | Deprecated, use &#x60;author_id&#x60; instead. | [optional] 
**AuthorId** | **long** |  | [optional] 
**CreatedAt** | **DateTime** |  | [optional] 
**UpdatedAt** | **DateTime** |  | [optional] 
**Deleted** | **string** |  | [optional] 
**Created** | **string** | Deprecated, use the &#x60;created_at&#x60; property instead. | [optional] 
**Updated** | **string** | Deprecated, use the &#x60;updated_at&#x60; property instead. | [optional] 
**ExternalIssues** | [**List&lt;ExternalIssue&gt;**](ExternalIssue.md) |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

