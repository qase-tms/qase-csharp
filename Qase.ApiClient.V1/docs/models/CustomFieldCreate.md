# Qase.ApiClient.V1.Model.CustomFieldCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Title** | **string** |  | 
**Entity** | **int** | Possible values: 0 - case; 1 - run; 2 - defect;  | 
**Type** | **int** | Possible values: 0 - number; 1 - string; 2 - text; 3 - selectbox; 4 - checkbox; 5 - radio; 6 - multiselect; 7 - url; 8 - user; 9 - datetime;  | 
**Value** | [**List&lt;CustomFieldCreateValueInner&gt;**](CustomFieldCreateValueInner.md) | Required if type one of: 3 - selectbox; 5 - radio; 6 - multiselect;  | [optional] 
**Placeholder** | **string** |  | [optional] 
**DefaultValue** | **string** |  | [optional] 
**IsFilterable** | **bool** |  | [optional] 
**IsVisible** | **bool** |  | [optional] 
**IsRequired** | **bool** |  | [optional] 
**IsEnabledForAllProjects** | **bool** |  | [optional] 
**ProjectsCodes** | **List&lt;string&gt;** |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

