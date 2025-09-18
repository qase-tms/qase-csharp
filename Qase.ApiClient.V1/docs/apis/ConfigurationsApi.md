# Qase.ApiClient.V1.Api.ConfigurationsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateConfiguration**](ConfigurationsApi.md#createconfiguration) | **POST** /configuration/{code} | Create a new configuration in a particular group. |
| [**CreateConfigurationGroup**](ConfigurationsApi.md#createconfigurationgroup) | **POST** /configuration/{code}/group | Create a new configuration group. |
| [**GetConfigurations**](ConfigurationsApi.md#getconfigurations) | **GET** /configuration/{code} | Get all configuration groups with configurations. |

<a id="createconfiguration"></a>
# **CreateConfiguration**
> IdResponse CreateConfiguration (string code, ConfigurationCreate configurationCreate)

Create a new configuration in a particular group.

This method allows to create a configuration in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **configurationCreate** | [**ConfigurationCreate**](ConfigurationCreate.md) |  |  |

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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createconfigurationgroup"></a>
# **CreateConfigurationGroup**
> IdResponse CreateConfigurationGroup (string code, ConfigurationGroupCreate configurationGroupCreate)

Create a new configuration group.

This method allows to create a configuration group in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **configurationGroupCreate** | [**ConfigurationGroupCreate**](ConfigurationGroupCreate.md) |  |  |

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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getconfigurations"></a>
# **GetConfigurations**
> ConfigurationListResponse GetConfigurations (string code)

Get all configuration groups with configurations.

This method allows to retrieve all configurations groups with configurations 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |

### Return type

[**ConfigurationListResponse**](ConfigurationListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all configurations. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

