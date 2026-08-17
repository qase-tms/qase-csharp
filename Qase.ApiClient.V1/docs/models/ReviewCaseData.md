# Qase.ApiClient.V1.Model.ReviewCaseData
The test case fields proposed by the review.

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
**IsMuted** | **bool** | Mute state of the proposed test case. | [optional] 
**SuiteId** | **long** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**IsManual** | **bool** | &#x60;true&#x60; if the case is manual, &#x60;false&#x60; if it is automated. | [optional] 
**IsToBeAutomated** | **bool** | &#x60;true&#x60; if a manual case is planned to be automated. | [optional] 
**Status** | **int** |  | [optional] 
**StepsType** | **string** | Format of the steps field. Omit to keep the current one, &#x60;classic&#x60; for a new-case draft; changing it requires sending &#x60;steps&#x60; in the same request. | [optional] 
**Attachments** | **List&lt;string&gt;** | A list of Attachment hashes. | [optional] 
**Steps** | [**List&lt;ReviewStepData&gt;**](ReviewStepData.md) | For gherkin steps send the scenario in &#x60;value&#x60;. | [optional] 
**Tags** | **List&lt;string&gt;** |  | [optional] 
**Parameters** | [**List&lt;TestCaseParameterCreate&gt;**](TestCaseParameterCreate.md) |  | [optional] 
**CustomField** | **Dictionary&lt;string, string&gt;** | Map of custom field ID to value. A &#x60;create&#x60; review must carry every required custom field. An &#x60;edit&#x60; review is validated against the current test case, so send only the fields the proposal changes. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

