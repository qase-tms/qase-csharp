# Qase.ApiClient.V1.Api.SystemFieldsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetSystemFields**](SystemFieldsApi.md#getsystemfields) | **GET** /system_field | Get all System Fields |

<a id="getsystemfields"></a>
# **GetSystemFields**
> SystemFieldListResponse GetSystemFields ()

Get all System Fields

This method allows to retrieve all system fields. 


### Parameters
This endpoint does not need any parameter.
### Return type

[**SystemFieldListResponse**](SystemFieldListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | System Field list. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

