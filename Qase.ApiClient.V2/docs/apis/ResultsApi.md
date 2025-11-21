# Qase.ApiClient.V2.Api.ResultsApi

All URIs are relative to *https://api.qase.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateResultV2**](ResultsApi.md#createresultv2) | **POST** /{project_code}/run/{run_id}/result | Create test run result |
| [**CreateResultsV2**](ResultsApi.md#createresultsv2) | **POST** /{project_code}/run/{run_id}/results | Bulk create test run result |

<a id="createresultv2"></a>
# **CreateResultV2**
> ResultCreateResponse CreateResultV2 (string projectCode, long runId, ResultCreate resultCreate)

Create test run result

This method allows to create single test run result.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **projectCode** | **string** |  |  |
| **runId** | **long** |  |  |
| **resultCreate** | [**ResultCreate**](ResultCreate.md) |  |  |

### Return type

[**ResultCreateResponse**](ResultCreateResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **202** | OK |  -  |
| **400** | Bad Request |  -  |
| **401** | Unauthorized |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |
| **422** | Unprocessable Entity |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createresultsv2"></a>
# **CreateResultsV2**
> ResultCreateBulkResponse CreateResultsV2 (string projectCode, long runId, CreateResultsRequestV2 createResultsRequestV2)

Bulk create test run result

This method allows to create several test run results at once.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **projectCode** | **string** |  |  |
| **runId** | **long** |  |  |
| **createResultsRequestV2** | [**CreateResultsRequestV2**](CreateResultsRequestV2.md) |  |  |

### Return type

[**ResultCreateBulkResponse**](ResultCreateBulkResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **202** | OK |  -  |
| **400** | Bad Request |  -  |
| **401** | Unauthorized |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |
| **422** | Unprocessable Entity |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

