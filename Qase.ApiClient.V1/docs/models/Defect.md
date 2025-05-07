# Qase.ApiClient.V1.Model.Defect

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **long** |  | [optional] 
**Title** | **string** |  | [optional] 
**ActualResult** | **string** |  | [optional] 
**Severity** | **string** |  | [optional] 
**Status** | **string** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**CustomFields** | [**List&lt;CustomFieldValue&gt;**](CustomFieldValue.md) |  | [optional] 
**Attachments** | [**List&lt;Attachment&gt;**](Attachment.md) |  | [optional] 
**ResolvedAt** | **DateTime** |  | [optional] 
**MemberId** | **long** | Deprecated, use &#x60;author_id&#x60; instead. | [optional] 
**AuthorId** | **long** |  | [optional] 
**ExternalData** | **string** |  | [optional] 
**Runs** | **List&lt;long&gt;** |  | [optional] 
**Results** | **List&lt;string&gt;** |  | [optional] 
**Tags** | [**List&lt;TagValue&gt;**](TagValue.md) |  | [optional] 
**CreatedAt** | **DateTime** |  | [optional] 
**UpdatedAt** | **DateTime** |  | [optional] 
**Created** | **string** | Deprecated, use the &#x60;created_at&#x60; property instead. | [optional] 
**Updated** | **string** | Deprecated, use the &#x60;updated_at&#x60; property instead. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

