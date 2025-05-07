# Qase.ApiClient.V1.Model.ResultCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Status** | **string** | Can have the following values &#x60;passed&#x60;, &#x60;failed&#x60;, &#x60;blocked&#x60;, &#x60;skipped&#x60;, &#x60;invalid&#x60; + custom statuses | 
**CaseId** | **long** |  | [optional] 
**Case** | [**ResultCreateCase**](ResultCreateCase.md) |  | [optional] 
**StartTime** | **int** |  | [optional] 
**Time** | **long** |  | [optional] 
**TimeMs** | **long** |  | [optional] 
**Defect** | **bool** |  | [optional] 
**Attachments** | **List&lt;string&gt;** |  | [optional] 
**Stacktrace** | **string** |  | [optional] 
**Comment** | **string** |  | [optional] 
**Param** | **Dictionary&lt;string, string&gt;** | A map of parameters (name &#x3D;&gt; value) | [optional] 
**ParamGroups** | **List&lt;List&lt;string&gt;&gt;** | List parameter groups by name only. Add their values in the &#39;param&#39; field | [optional] 
**Steps** | [**List&lt;TestStepResultCreate&gt;**](TestStepResultCreate.md) |  | [optional] 
**AuthorId** | **long** |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

