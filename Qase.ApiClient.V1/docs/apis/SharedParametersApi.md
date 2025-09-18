# Qase.ApiClient.V1.Api.SharedParametersApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateSharedParameter**](SharedParametersApi.md#createsharedparameter) | **POST** /shared_parameter | Create a new shared parameter |
| [**DeleteSharedParameter**](SharedParametersApi.md#deletesharedparameter) | **DELETE** /shared_parameter/{id} | Delete shared parameter |
| [**GetSharedParameter**](SharedParametersApi.md#getsharedparameter) | **GET** /shared_parameter/{id} | Get a specific shared parameter |
| [**GetSharedParameters**](SharedParametersApi.md#getsharedparameters) | **GET** /shared_parameter | Get all shared parameters |
| [**UpdateSharedParameter**](SharedParametersApi.md#updatesharedparameter) | **PATCH** /shared_parameter/{id} | Update shared parameter |

<a id="createsharedparameter"></a>
# **CreateSharedParameter**
> UuidResponse CreateSharedParameter (SharedParameterCreate sharedParameterCreate)

Create a new shared parameter


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sharedParameterCreate** | [**SharedParameterCreate**](SharedParameterCreate.md) |  |  |

### Return type

[**UuidResponse**](UuidResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A shared parameter. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deletesharedparameter"></a>
# **DeleteSharedParameter**
> UuidResponse1 DeleteSharedParameter (Guid id)

Delete shared parameter

Delete shared parameter along with all its usages in test cases and reviews.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |

### Return type

[**UuidResponse1**](UuidResponse1.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsharedparameter"></a>
# **GetSharedParameter**
> SharedParameterResponse GetSharedParameter (Guid id)

Get a specific shared parameter


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |

### Return type

[**SharedParameterResponse**](SharedParameterResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A shared parameter. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsharedparameters"></a>
# **GetSharedParameters**
> SharedParameterListResponse GetSharedParameters (int limit = null, int offset = null, string filtersSearch = null, string filtersType = null, List<string> filtersProjectCodes = null)

Get all shared parameters


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |
| **filtersSearch** | **string** |  | [optional]  |
| **filtersType** | **string** |  | [optional]  |
| **filtersProjectCodes** | [**List&lt;string&gt;**](string.md) |  | [optional]  |

### Return type

[**SharedParameterListResponse**](SharedParameterListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all shared parameters. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatesharedparameter"></a>
# **UpdateSharedParameter**
> UuidResponse1 UpdateSharedParameter (Guid id, SharedParameterUpdate sharedParameterUpdate)

Update shared parameter


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |
| **sharedParameterUpdate** | [**SharedParameterUpdate**](SharedParameterUpdate.md) |  |  |

### Return type

[**UuidResponse1**](UuidResponse1.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

