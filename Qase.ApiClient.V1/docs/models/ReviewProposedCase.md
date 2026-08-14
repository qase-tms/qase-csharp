# Qase.ApiClient.V1.Model.ReviewProposedCase
The test case state proposed by the review. Only the fields the proposal carries are present.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Title** | **string** |  | [optional] 
**Description** | **string** |  | [optional] 
**Preconditions** | **string** |  | [optional] 
**Postconditions** | **string** |  | [optional] 
**Severity** | **int** |  | [optional] 
**Priority** | **int** |  | [optional] 
**Behavior** | **int** |  | [optional] 
**Type** | **int** |  | [optional] 
**Layer** | **int** |  | [optional] 
**IsFlaky** | **int** |  | [optional] 
**IsMuted** | **bool** |  | [optional] 
**SuiteId** | **long** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**IsManual** | **bool** | &#x60;true&#x60; if the case is manual, &#x60;false&#x60; if it is automated. | [optional] 
**IsToBeAutomated** | **bool** | &#x60;true&#x60; if a manual case is planned to be automated. | [optional] 
**Status** | **int** |  | [optional] 
**StepsType** | **string** |  | [optional] 
**Attachments** | **List&lt;string&gt;** | Attachment hashes. | [optional] 
**Steps** | [**List&lt;ReviewProposedStep&gt;**](ReviewProposedStep.md) |  | [optional] 
**Tags** | **List&lt;string&gt;** |  | [optional] 
**Parameters** | [**List&lt;TestCaseParameter&gt;**](TestCaseParameter.md) |  | [optional] 
**CustomFields** | [**List&lt;CustomFieldValue&gt;**](CustomFieldValue.md) |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

