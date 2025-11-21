# Qase.ApiClient.V2.Api.CustomFieldsApi

All URIs are relative to *https://api.qase.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetCustomFieldV2**](CustomFieldsApi.md#getcustomfieldv2) | **GET** /custom_field/{id} | Get Custom Field |
| [**GetCustomFieldsV2**](CustomFieldsApi.md#getcustomfieldsv2) | **GET** /custom_field | Get all Custom Fields |

<a id="getcustomfieldv2"></a>
# **GetCustomFieldV2**
> CustomFieldResponse GetCustomFieldV2 (int id)

Get Custom Field

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

<a id="getcustomfieldsv2"></a>
# **GetCustomFieldsV2**
> CustomFieldListResponse GetCustomFieldsV2 (string entity = null, string type = null, int limit = null, int offset = null)

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

