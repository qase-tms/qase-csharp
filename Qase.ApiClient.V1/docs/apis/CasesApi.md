# Qase.ApiClient.V1.Api.CasesApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**Bulk**](CasesApi.md#bulk) | **POST** /case/{code}/bulk | Create test cases in bulk |
| [**CaseAttachExternalIssue**](CasesApi.md#caseattachexternalissue) | **POST** /case/{code}/external-issue/attach | Attach the external issues to the test cases |
| [**CaseDetachExternalIssue**](CasesApi.md#casedetachexternalissue) | **POST** /case/{code}/external-issue/detach | Detach the external issues from the test cases |
| [**CreateCase**](CasesApi.md#createcase) | **POST** /case/{code} | Create a new test case |
| [**DeleteCase**](CasesApi.md#deletecase) | **DELETE** /case/{code}/{id} | Delete test case |
| [**GetCase**](CasesApi.md#getcase) | **GET** /case/{code}/{id} | Get a specific test case |
| [**GetCases**](CasesApi.md#getcases) | **GET** /case/{code} | Get all test cases |
| [**UpdateCase**](CasesApi.md#updatecase) | **PATCH** /case/{code}/{id} | Update test case |

<a id="bulk"></a>
# **Bulk**
> Bulk200Response Bulk (string code, TestCasebulk testCasebulk)

Create test cases in bulk

This method allows to bulk create new test cases in a project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCasebulk** | [**TestCasebulk**](TestCasebulk.md) |  |  |

### Return type

[**Bulk200Response**](Bulk200Response.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | List of IDs of the created cases. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="caseattachexternalissue"></a>
# **CaseAttachExternalIssue**
> BaseResponse CaseAttachExternalIssue (string code, TestCaseExternalIssues testCaseExternalIssues)

Attach the external issues to the test cases


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCaseExternalIssues** | [**TestCaseExternalIssues**](TestCaseExternalIssues.md) |  |  |

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
| **200** | OK. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="casedetachexternalissue"></a>
# **CaseDetachExternalIssue**
> BaseResponse CaseDetachExternalIssue (string code, TestCaseExternalIssues testCaseExternalIssues)

Detach the external issues from the test cases


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCaseExternalIssues** | [**TestCaseExternalIssues**](TestCaseExternalIssues.md) |  |  |

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
| **200** | OK. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createcase"></a>
# **CreateCase**
> IdResponse CreateCase (string code, TestCaseCreate testCaseCreate)

Create a new test case

This method allows to create a new test case in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCaseCreate** | [**TestCaseCreate**](TestCaseCreate.md) |  |  |

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

<a id="deletecase"></a>
# **DeleteCase**
> IdResponse DeleteCase (string code, int id)

Delete test case

This method completely deletes a test case from repository. 


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
| **200** | A Test Case. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getcase"></a>
# **GetCase**
> TestCaseResponse GetCase (string code, int id, string include = null)

Get a specific test case

This method allows to retrieve a specific test case. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **include** | **string** | A list of entities to include in response separated by comma. Possible values: external_issues.  | [optional]  |

### Return type

[**TestCaseResponse**](TestCaseResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A Test Case. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getcases"></a>
# **GetCases**
> TestCaseListResponse GetCases (string code, string search = null, int milestoneId = null, int suiteId = null, string severity = null, string priority = null, string type = null, string behavior = null, string automation = null, string status = null, string externalIssuesType = null, List<string> externalIssuesIds = null, string include = null, int limit = null, int offset = null)

Get all test cases

This method allows to retrieve all test cases stored in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **search** | **string** | Provide a string that will be used to search by name. | [optional]  |
| **milestoneId** | **int** | ID of milestone. | [optional]  |
| **suiteId** | **int** | ID of test suite. | [optional]  |
| **severity** | **string** | A list of severity values separated by comma. Possible values: undefined, blocker, critical, major, normal, minor, trivial  | [optional]  |
| **priority** | **string** | A list of priority values separated by comma. Possible values: undefined, high, medium, low  | [optional]  |
| **type** | **string** | A list of type values separated by comma. Possible values: other, functional smoke, regression, security, usability, performance, acceptance  | [optional]  |
| **behavior** | **string** | A list of behavior values separated by comma. Possible values: undefined, positive negative, destructive  | [optional]  |
| **automation** | **string** | A list of values separated by comma. Possible values: is-not-automated, automated to-be-automated  | [optional]  |
| **status** | **string** | A list of values separated by comma. Possible values: actual, draft deprecated  | [optional]  |
| **externalIssuesType** | **string** | An integration type.  | [optional]  |
| **externalIssuesIds** | [**List&lt;string&gt;**](string.md) | A list of issue IDs. | [optional]  |
| **include** | **string** | A list of entities to include in response separated by comma. Possible values: external_issues.  | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**TestCaseListResponse**](TestCaseListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all cases. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatecase"></a>
# **UpdateCase**
> IdResponse UpdateCase (string code, int id, TestCaseUpdate testCaseUpdate)

Update test case

This method updates a test case. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **testCaseUpdate** | [**TestCaseUpdate**](TestCaseUpdate.md) |  |  |

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
| **200** | A Test Case. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

