# Qase.ApiClient.V1.Api.CustomFieldsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateCustomField**](CustomFieldsApi.md#createcustomfield) | **POST** /custom_field | Create new Custom Field |
| [**DeleteCustomField**](CustomFieldsApi.md#deletecustomfield) | **DELETE** /custom_field/{id} | Delete Custom Field by id |
| [**GetCustomField**](CustomFieldsApi.md#getcustomfield) | **GET** /custom_field/{id} | Get Custom Field by id |
| [**GetCustomFields**](CustomFieldsApi.md#getcustomfields) | **GET** /custom_field | Get all Custom Fields |
| [**UpdateCustomField**](CustomFieldsApi.md#updatecustomfield) | **PATCH** /custom_field/{id} | Update Custom Field by id |

<a id="createcustomfield"></a>
# **CreateCustomField**
> IdResponse CreateCustomField (CustomFieldCreate customFieldCreate)

Create new Custom Field

This method allows to create custom field. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **customFieldCreate** | [**CustomFieldCreate**](CustomFieldCreate.md) |  |  |

### Return type

[**IdResponse**](IdResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Created Custom Field id. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deletecustomfield"></a>
# **DeleteCustomField**
> BaseResponse DeleteCustomField (int id)

Delete Custom Field by id

This method allows to delete custom field. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Identifier. |  |

### Return type

[**BaseResponse**](BaseResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Custom Field removal result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getcustomfield"></a>
# **GetCustomField**
> CustomFieldResponse GetCustomField (int id)

Get Custom Field by id

This method allows to retrieve custom field. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Identifier. |  |

### Return type

[**CustomFieldResponse**](CustomFieldResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A Custom Field. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getcustomfields"></a>
# **GetCustomFields**
> CustomFieldListResponse GetCustomFields (string entity = null, string type = null, int limit = null, int offset = null)

Get all Custom Fields

This method allows to retrieve and filter custom fields. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **entity** | **string** |  | [optional]  |
| **type** | **string** |  | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**CustomFieldListResponse**](CustomFieldListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Custom Field list. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatecustomfield"></a>
# **UpdateCustomField**
> BaseResponse UpdateCustomField (int id, CustomFieldUpdate customFieldUpdate)

Update Custom Field by id

This method allows to update custom field. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Identifier. |  |
| **customFieldUpdate** | [**CustomFieldUpdate**](CustomFieldUpdate.md) |  |  |

### Return type

[**BaseResponse**](BaseResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Custom Field update result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

