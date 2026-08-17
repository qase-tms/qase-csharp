# Qase.ApiClient.V1.Model.ReviewUpdate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Reviewers** | **List&lt;Guid&gt;** | Author UUIDs of team members assigned as reviewers (see &#x60;GET /author&#x60;). When provided, replaces the current reviewer list; an empty array removes all reviewers. Omit to leave reviewers unchanged. | [optional] 
**ProposedCase** | [**ReviewCaseData**](ReviewCaseData.md) | Sent fields are merged into the stored proposal. Changing the proposal resets all existing approvals; updating only the reviewers keeps them. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

