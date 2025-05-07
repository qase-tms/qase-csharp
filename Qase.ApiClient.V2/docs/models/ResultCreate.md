# Qase.ApiClient.V2.Model.ResultCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Title** | **string** |  | 
**Execution** | [**ResultExecution**](ResultExecution.md) |  | 
**Id** | **string** | If passed, used as an idempotency key | [optional] 
**Signature** | **string** |  | [optional] 
**TestopsId** | **long** | ID of the test case. Cannot be specified together with testopd_ids. | [optional] 
**TestopsIds** | **List&lt;long&gt;** | IDs of the test cases. Cannot be specified together with testopd_id. | [optional] 
**Fields** | [**ResultCreateFields**](ResultCreateFields.md) |  | [optional] 
**Attachments** | **List&lt;string&gt;** |  | [optional] 
**Steps** | [**List&lt;ResultStep&gt;**](ResultStep.md) |  | [optional] 
**StepsType** | **ResultStepsType** |  | [optional] 
**Params** | **Dictionary&lt;string, string&gt;** |  | [optional] 
**ParamGroups** | **List&lt;List&lt;string&gt;&gt;** | List parameter groups by name only. Add their values in the &#39;params&#39; field | [optional] 
**Relations** | [**ResultRelations**](ResultRelations.md) |  | [optional] 
**Message** | **string** |  | [optional] 
**Defect** | **bool** | If true and the result is failed, the defect associated with the result will be created | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

