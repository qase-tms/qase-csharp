# Qase.ApiClient.V1.Model.ReviewCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ProposedCase** | [**ReviewCaseData**](ReviewCaseData.md) | For &#x60;create&#x60; reviews &#x60;title&#x60; and all required project fields are required. For &#x60;edit&#x60; reviews send only the fields the proposal changes. | 
**CaseId** | **long** | ID of the reviewed test case. When present an &#x60;edit&#x60; review is created, otherwise a &#x60;create&#x60; review with a new-case draft. | [optional] 
**Reviewers** | **List&lt;Guid&gt;** | Author UUIDs of team members to assign as reviewers (see &#x60;GET /author&#x60;). | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

