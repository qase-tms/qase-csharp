# Qase.ApiClient.V1.Model.RunCreate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Title** | **string** |  | 
**Description** | **string** |  | [optional] 
**IncludeAllCases** | **bool** |  | [optional] 
**Cases** | **List&lt;long&gt;** |  | [optional] 
**IsAutotest** | **bool** |  | [optional] 
**EnvironmentId** | **long** |  | [optional] 
**EnvironmentSlug** | **string** |  | [optional] 
**MilestoneId** | **long** |  | [optional] 
**PlanId** | **long** |  | [optional] 
**AuthorId** | **long** |  | [optional] 
**Tags** | **List&lt;string&gt;** |  | [optional] 
**Configurations** | **List&lt;long&gt;** |  | [optional] 
**CustomField** | **Dictionary&lt;string, string&gt;** | A map of custom fields values (id &#x3D;&gt; value) | [optional] 
**StartTime** | **string** |  | [optional] 
**EndTime** | **string** |  | [optional] 
**IsCloud** | **bool** | Indicates if the run is created for the Test Cases produced by AIDEN | [optional] 
**CloudRunConfig** | [**RunCreateCloudRunConfig**](RunCreateCloudRunConfig.md) |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

