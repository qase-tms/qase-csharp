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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class BulkExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var testCasebulk = new TestCasebulk(); // TestCasebulk | 

            try
            {
                // Create test cases in bulk
                Bulk200Response result = apiInstance.Bulk(code, testCasebulk);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.Bulk: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the BulkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create test cases in bulk
    ApiResponse<Bulk200Response> response = apiInstance.BulkWithHttpInfo(code, testCasebulk);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.BulkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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
> BaseResponse CaseAttachExternalIssue (string code, TestCaseExternalIssues testCaseexternalIssues)

Attach the external issues to the test cases

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CaseAttachExternalIssueExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var testCaseexternalIssues = new TestCaseExternalIssues(); // TestCaseExternalIssues | 

            try
            {
                // Attach the external issues to the test cases
                BaseResponse result = apiInstance.CaseAttachExternalIssue(code, testCaseexternalIssues);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.CaseAttachExternalIssue: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CaseAttachExternalIssueWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Attach the external issues to the test cases
    ApiResponse<BaseResponse> response = apiInstance.CaseAttachExternalIssueWithHttpInfo(code, testCaseexternalIssues);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.CaseAttachExternalIssueWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCaseexternalIssues** | [**TestCaseExternalIssues**](TestCaseExternalIssues.md) |  |  |

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
> BaseResponse CaseDetachExternalIssue (string code, TestCaseExternalIssues testCaseexternalIssues)

Detach the external issues from the test cases

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CaseDetachExternalIssueExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var testCaseexternalIssues = new TestCaseExternalIssues(); // TestCaseExternalIssues | 

            try
            {
                // Detach the external issues from the test cases
                BaseResponse result = apiInstance.CaseDetachExternalIssue(code, testCaseexternalIssues);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.CaseDetachExternalIssue: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CaseDetachExternalIssueWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Detach the external issues from the test cases
    ApiResponse<BaseResponse> response = apiInstance.CaseDetachExternalIssueWithHttpInfo(code, testCaseexternalIssues);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.CaseDetachExternalIssueWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **testCaseexternalIssues** | [**TestCaseExternalIssues**](TestCaseExternalIssues.md) |  |  |

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateCaseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var testCaseCreate = new TestCaseCreate(); // TestCaseCreate | 

            try
            {
                // Create a new test case
                IdResponse result = apiInstance.CreateCase(code, testCaseCreate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.CreateCase: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateCaseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new test case
    ApiResponse<IdResponse> response = apiInstance.CreateCaseWithHttpInfo(code, testCaseCreate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.CreateCaseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class DeleteCaseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.

            try
            {
                // Delete test case
                IdResponse result = apiInstance.DeleteCase(code, id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.DeleteCase: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteCaseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete test case
    ApiResponse<IdResponse> response = apiInstance.DeleteCaseWithHttpInfo(code, id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.DeleteCaseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetCaseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var include = "include_example";  // string | A list of entities to include in response separated by comma. Possible values: external_issues.  (optional) 

            try
            {
                // Get a specific test case
                TestCaseResponse result = apiInstance.GetCase(code, id, include);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.GetCase: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCaseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a specific test case
    ApiResponse<TestCaseResponse> response = apiInstance.GetCaseWithHttpInfo(code, id, include);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.GetCaseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetCasesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var search = "search_example";  // string | Provide a string that will be used to search by name. (optional) 
            var milestoneId = 56;  // int | ID of milestone. (optional) 
            var suiteId = 56;  // int | ID of test suite. (optional) 
            var severity = "severity_example";  // string | A list of severity values separated by comma. Possible values: undefined, blocker, critical, major, normal, minor, trivial  (optional) 
            var priority = "priority_example";  // string | A list of priority values separated by comma. Possible values: undefined, high, medium, low  (optional) 
            var type = "type_example";  // string | A list of type values separated by comma. Possible values: other, functional smoke, regression, security, usability, performance, acceptance  (optional) 
            var behavior = "behavior_example";  // string | A list of behavior values separated by comma. Possible values: undefined, positive negative, destructive  (optional) 
            var automation = "automation_example";  // string | A list of values separated by comma. Possible values: is-not-automated, automated to-be-automated  (optional) 
            var status = "status_example";  // string | A list of values separated by comma. Possible values: actual, draft deprecated  (optional) 
            var externalIssuesType = "asana";  // string | An integration type.  (optional) 
            var externalIssuesIds = new List<string>(); // List<string> | A list of issue IDs. (optional) 
            var include = "include_example";  // string | A list of entities to include in response separated by comma. Possible values: external_issues.  (optional) 
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)

            try
            {
                // Get all test cases
                TestCaseListResponse result = apiInstance.GetCases(code, search, milestoneId, suiteId, severity, priority, type, behavior, automation, status, externalIssuesType, externalIssuesIds, include, limit, offset);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.GetCases: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCasesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all test cases
    ApiResponse<TestCaseListResponse> response = apiInstance.GetCasesWithHttpInfo(code, search, milestoneId, suiteId, severity, priority, type, behavior, automation, status, externalIssuesType, externalIssuesIds, include, limit, offset);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.GetCasesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class UpdateCaseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new CasesApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var testCaseUpdate = new TestCaseUpdate(); // TestCaseUpdate | 

            try
            {
                // Update test case
                IdResponse result = apiInstance.UpdateCase(code, id, testCaseUpdate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CasesApi.UpdateCase: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateCaseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update test case
    ApiResponse<IdResponse> response = apiInstance.UpdateCaseWithHttpInfo(code, id, testCaseUpdate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CasesApi.UpdateCaseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

