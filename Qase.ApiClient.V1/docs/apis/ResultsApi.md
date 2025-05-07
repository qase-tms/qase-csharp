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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateResultExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var resultCreate = new ResultCreate(); // ResultCreate | 

            try
            {
                // Create test run result
                ResultCreateResponse result = apiInstance.CreateResult(code, id, resultCreate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.CreateResult: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateResultWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create test run result
    ApiResponse<ResultCreateResponse> response = apiInstance.CreateResultWithHttpInfo(code, id, resultCreate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.CreateResultWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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
> BaseResponse CreateResultBulk (string code, int id, ResultcreateBulk resultcreateBulk)

Bulk create test run result

This method allows to create a lot of test run result at once.  If you try to send more than 2,000 results in a single bulk request, you will receive an error with code 413 - Payload Too Large.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateResultBulkExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var resultcreateBulk = new ResultcreateBulk(); // ResultcreateBulk | 

            try
            {
                // Bulk create test run result
                BaseResponse result = apiInstance.CreateResultBulk(code, id, resultcreateBulk);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.CreateResultBulk: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateResultBulkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Bulk create test run result
    ApiResponse<BaseResponse> response = apiInstance.CreateResultBulkWithHttpInfo(code, id, resultcreateBulk);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.CreateResultBulkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **resultcreateBulk** | [**ResultcreateBulk**](ResultcreateBulk.md) |  |  |

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class DeleteResultExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var hash = "hash_example";  // string | Hash.

            try
            {
                // Delete test run result
                HashResponse result = apiInstance.DeleteResult(code, id, hash);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.DeleteResult: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteResultWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete test run result
    ApiResponse<HashResponse> response = apiInstance.DeleteResultWithHttpInfo(code, id, hash);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.DeleteResultWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetResultExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var hash = "hash_example";  // string | Hash.

            try
            {
                // Get test run result by code
                ResultResponse result = apiInstance.GetResult(code, hash);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.GetResult: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetResultWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get test run result by code
    ApiResponse<ResultResponse> response = apiInstance.GetResultWithHttpInfo(code, hash);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.GetResultWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetResultsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var status = "status_example";  // string | A single test run result status. Possible values: in_progress, passed, failed, blocked, skipped, invalid.  (optional) 
            var run = "run_example";  // string | A list of run IDs separated by comma. (optional) 
            var caseId = "caseId_example";  // string | A list of case IDs separated by comma. (optional) 
            var member = "member_example";  // string | A list of member IDs separated by comma. (optional) 
            var api = true;  // bool |  (optional) 
            var fromEndTime = "fromEndTime_example";  // string | Will return all results created after provided datetime. Allowed format: `Y-m-d H:i:s`.  (optional) 
            var toEndTime = "toEndTime_example";  // string | Will return all results created before provided datetime. Allowed format: `Y-m-d H:i:s`.  (optional) 
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)

            try
            {
                // Get all test run results
                ResultListResponse result = apiInstance.GetResults(code, status, run, caseId, member, api, fromEndTime, toEndTime, limit, offset);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.GetResults: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetResultsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all test run results
    ApiResponse<ResultListResponse> response = apiInstance.GetResultsWithHttpInfo(code, status, run, caseId, member, api, fromEndTime, toEndTime, limit, offset);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.GetResultsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class UpdateResultExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var hash = "hash_example";  // string | Hash.
            var resultUpdate = new ResultUpdate(); // ResultUpdate | 

            try
            {
                // Update test run result
                HashResponse result = apiInstance.UpdateResult(code, id, hash, resultUpdate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.UpdateResult: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateResultWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update test run result
    ApiResponse<HashResponse> response = apiInstance.UpdateResultWithHttpInfo(code, id, hash, resultUpdate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.UpdateResultWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

