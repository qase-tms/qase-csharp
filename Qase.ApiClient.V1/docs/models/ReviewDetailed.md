# Qase.ApiClient.V1.Model.ReviewDetailed

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **long** | Review ID, unique within the project. | [optional] 
**Title** | **string** |  | [optional] 
**Type** | **string** | &#x60;create&#x60; — the review proposes a new test case; &#x60;edit&#x60; — the review proposes changes to an existing test case. | [optional] 
**Status** | **string** |  | [optional] 
**CaseId** | **long** | ID of the reviewed test case. Null for new-case draft reviews. | [optional] 
**AuthorUuid** | **Guid** | Author UUID of the review creator (see &#x60;GET /author&#x60;). | [optional] 
**Reviewers** | [**List&lt;ReviewReviewersInner&gt;**](ReviewReviewersInner.md) |  | [optional] 
**CreatedAt** | **DateTime** |  | [optional] 
**UpdatedAt** | **DateTime** |  | [optional] 
**ProposedCase** | **Object** | The proposed test case state. Merging the review applies it to the test case. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

