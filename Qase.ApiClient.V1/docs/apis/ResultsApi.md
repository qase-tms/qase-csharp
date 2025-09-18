# Qase.ApiClient.V1.Api.ResultsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateResult**](ResultsApi.md#createresult) | **POST** /result/{code}/{id} | Create test run result |
| [**CreateResultBulk**](ResultsApi.md#createresultbulk) | **POST** /result/{code}/{id}/bulk | Bulk create test run result |
| [**DeleteResult**](ResultsApi.md#deleteresult) | **DELETE** /result/{code}/{id}/{hash} | Delete test run result |
| [**GetResult**](ResultsApi.md#getresult) | **GET** /result/{code}/{hash} | Get test run result by code |
| [**GetResults**](ResultsApi.md#getresults) | **GET** /result/{code} | Get all test run results |
| [**UpdateResult**](ResultsApi.md#updateresult) | **PATCH** /result/{code}/{id}/{hash} | Update test run result |

<a id="createresult"></a>
# **CreateResult**
> ResultCreateResponse CreateResult (string code, int id, ResultCreate resultCreate)

Create test run result

This method allows to create test run result by Run Id. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createresultbulk"></a>
# **CreateResultBulk**
> BaseResponse CreateResultBulk (string code, int id, ResultCreateBulk resultCreateBulk)

Bulk create test run result

This method allows to create a lot of test run result at once.  If you try to send more than 2,000 results in a single bulk request, you will receive an error with code 413 - Payload Too Large.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **resultCreateBulk** | [**ResultCreateBulk**](ResultCreateBulk.md) |  |  |

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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **413** | Payload Too Large. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deleteresult"></a>
# **DeleteResult**
> HashResponse DeleteResult (string code, int id, string hash)

Delete test run result

This method allows to delete test run result. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **hash** | **string** | Hash. |  |

### Return type

[**HashResponse**](HashResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getresult"></a>
# **GetResult**
> ResultResponse GetResult (string code, string hash)

Get test run result by code

This method allows to retrieve a specific test run result by Hash. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **hash** | **string** | Hash. |  |

### Return type

[**ResultResponse**](ResultResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A test run result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getresults"></a>
# **GetResults**
> ResultListResponse GetResults (string code, string status = null, string run = null, string caseId = null, string member = null, bool api = null, string fromEndTime = null, string toEndTime = null, int limit = null, int offset = null)

Get all test run results

This method allows to retrieve all test run results stored in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **status** | **string** | A single test run result status. Possible values: in_progress, passed, failed, blocked, skipped, invalid.  | [optional]  |
| **run** | **string** | A list of run IDs separated by comma. | [optional]  |
| **caseId** | **string** | A list of case IDs separated by comma. | [optional]  |
| **member** | **string** | A list of member IDs separated by comma. | [optional]  |
| **api** | **bool** |  | [optional]  |
| **fromEndTime** | **string** | Will return all results created after provided datetime. Allowed format: &#x60;Y-m-d H:i:s&#x60;.  | [optional]  |
| **toEndTime** | **string** | Will return all results created before provided datetime. Allowed format: &#x60;Y-m-d H:i:s&#x60;.  | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**ResultListResponse**](ResultListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all test run results. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updateresult"></a>
# **UpdateResult**
> HashResponse UpdateResult (string code, int id, string hash, ResultUpdate resultUpdate)

Update test run result

This method allows to update test run result. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **hash** | **string** | Hash. |  |
| **resultUpdate** | [**ResultUpdate**](ResultUpdate.md) |  |  |

### Return type

[**HashResponse**](HashResponse.md)

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

