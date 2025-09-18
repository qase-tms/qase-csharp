# Qase.ApiClient.V1.Api.RunsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CompleteRun**](RunsApi.md#completerun) | **POST** /run/{code}/{id}/complete | Complete a specific run |
| [**CreateRun**](RunsApi.md#createrun) | **POST** /run/{code} | Create a new run |
| [**DeleteRun**](RunsApi.md#deleterun) | **DELETE** /run/{code}/{id} | Delete run |
| [**GetRun**](RunsApi.md#getrun) | **GET** /run/{code}/{id} | Get a specific run |
| [**GetRuns**](RunsApi.md#getruns) | **GET** /run/{code} | Get all runs |
| [**RunUpdateExternalIssue**](RunsApi.md#runupdateexternalissue) | **POST** /run/{code}/external-issue | Update external issues for runs |
| [**UpdateRun**](RunsApi.md#updaterun) | **PATCH** /run/{code}/{id} | Update a specific run |
| [**UpdateRunPublicity**](RunsApi.md#updaterunpublicity) | **PATCH** /run/{code}/{id}/public | Update publicity of a specific run |

<a id="completerun"></a>
# **CompleteRun**
> BaseResponse CompleteRun (string code, int id)

Complete a specific run

This method allows to complete a specific run. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createrun"></a>
# **CreateRun**
> IdResponse CreateRun (string code, RunCreate runCreate)

Create a new run

This method allows to create a run in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **runCreate** | [**RunCreate**](RunCreate.md) |  |  |

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

<a id="deleterun"></a>
# **DeleteRun**
> IdResponse DeleteRun (string code, int id)

Delete run

This method completely deletes a run from repository. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |

### Return type

[**IdResponse**](IdResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A Result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getrun"></a>
# **GetRun**
> RunResponse GetRun (string code, int id, string include = null)

Get a specific run

This method allows to retrieve a specific run. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **include** | **string** | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects, external_issue  | [optional]  |

### Return type

[**RunResponse**](RunResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A run. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getruns"></a>
# **GetRuns**
> RunListResponse GetRuns (string code, string search = null, string status = null, int milestone = null, int environment = null, long fromStartTime = null, long toStartTime = null, int limit = null, int offset = null, string include = null)

Get all runs

This method allows to retrieve all runs stored in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **search** | **string** |  | [optional]  |
| **status** | **string** | A list of status values separated by comma. Possible values: in_progress, passed, failed, aborted, active (deprecated), complete (deprecated), abort (deprecated).  | [optional]  |
| **milestone** | **int** |  | [optional]  |
| **environment** | **int** |  | [optional]  |
| **fromStartTime** | **long** |  | [optional]  |
| **toStartTime** | **long** |  | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |
| **include** | **string** | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects, external_issue  | [optional]  |

### Return type

[**RunListResponse**](RunListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all runs. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="runupdateexternalissue"></a>
# **RunUpdateExternalIssue**
> void RunUpdateExternalIssue (string code, RunExternalIssues runExternalIssues)

Update external issues for runs

This method allows you to update links between test runs and external issues (such as Jira tickets).  You can use this endpoint to: - Link test runs to external issues by providing the external issue identifier (e.g., \"PROJ-1234\") - Update existing links by providing a new external issue identifier - Remove existing links by setting the external_issue field to null  **Important**: Each test run can have only one link with an external issue. If a test run already has an external issue link, providing a new external_issue value will replace the existing link.  The endpoint supports both Jira Cloud and Jira Server integrations. Each request can update multiple test run links in a single operation. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **runExternalIssues** | [**RunExternalIssues**](RunExternalIssues.md) |  |  |

### Return type

void (empty response body)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: Not defined


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

<a id="updaterun"></a>
# **UpdateRun**
> BaseResponse UpdateRun (string code, int id, Runupdate runupdate)

Update a specific run

This method allows to update a specific run. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **runupdate** | [**Runupdate**](Runupdate.md) |  |  |

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
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updaterunpublicity"></a>
# **UpdateRunPublicity**
> RunPublicResponse UpdateRunPublicity (string code, int id, RunPublic runPublic)

Update publicity of a specific run

This method allows to update a publicity of specific run. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **runPublic** | [**RunPublic**](RunPublic.md) |  |  |

### Return type

[**RunPublicResponse**](RunPublicResponse.md)

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
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

