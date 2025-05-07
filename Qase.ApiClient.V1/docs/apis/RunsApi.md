# Qase.ApiClient.V1.Api.RunsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CompleteRun**](RunsApi.md#completerun) | **POST** /run/{code}/{id}/complete | Complete a specific run |
| [**CreateRun**](RunsApi.md#createrun) | **POST** /run/{code} | Create a new run |
| [**DeleteRun**](RunsApi.md#deleterun) | **DELETE** /run/{code}/{id} | Delete run |
| [**GetRun**](RunsApi.md#getrun) | **GET** /run/{code}/{id} | Get a specific run |
| [**GetRuns**](RunsApi.md#getruns) | **GET** /run/{code} | Get all runs |
| [**UpdateRunPublicity**](RunsApi.md#updaterunpublicity) | **PATCH** /run/{code}/{id}/public | Update publicity of a specific run |

<a id="completerun"></a>
# **CompleteRun**
> BaseResponse CompleteRun (string code, int id)

Complete a specific run

This method allows to complete a specific run. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CompleteRunExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.

            try
            {
                // Complete a specific run
                BaseResponse result = apiInstance.CompleteRun(code, id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.CompleteRun: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CompleteRunWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Complete a specific run
    ApiResponse<BaseResponse> response = apiInstance.CompleteRunWithHttpInfo(code, id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.CompleteRunWithHttpInfo: " + e.Message);
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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateRunExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var runCreate = new RunCreate(); // RunCreate | 

            try
            {
                // Create a new run
                IdResponse result = apiInstance.CreateRun(code, runCreate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.CreateRun: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateRunWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new run
    ApiResponse<IdResponse> response = apiInstance.CreateRunWithHttpInfo(code, runCreate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.CreateRunWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class DeleteRunExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.

            try
            {
                // Delete run
                IdResponse result = apiInstance.DeleteRun(code, id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.DeleteRun: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteRunWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete run
    ApiResponse<IdResponse> response = apiInstance.DeleteRunWithHttpInfo(code, id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.DeleteRunWithHttpInfo: " + e.Message);
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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetRunExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var include = "include_example";  // string | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects  (optional) 

            try
            {
                // Get a specific run
                RunResponse result = apiInstance.GetRun(code, id, include);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.GetRun: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetRunWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a specific run
    ApiResponse<RunResponse> response = apiInstance.GetRunWithHttpInfo(code, id, include);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.GetRunWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **include** | **string** | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects  | [optional]  |

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

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetRunsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var search = "search_example";  // string |  (optional) 
            var status = "status_example";  // string | A list of status values separated by comma. Possible values: in_progress, passed, failed, aborted, active (deprecated), complete (deprecated), abort (deprecated).  (optional) 
            var milestone = 56;  // int |  (optional) 
            var environment = 56;  // int |  (optional) 
            var fromStartTime = 789L;  // long |  (optional) 
            var toStartTime = 789L;  // long |  (optional) 
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)
            var include = "include_example";  // string | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects  (optional) 

            try
            {
                // Get all runs
                RunListResponse result = apiInstance.GetRuns(code, search, status, milestone, environment, fromStartTime, toStartTime, limit, offset, include);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.GetRuns: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetRunsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all runs
    ApiResponse<RunListResponse> response = apiInstance.GetRunsWithHttpInfo(code, search, status, milestone, environment, fromStartTime, toStartTime, limit, offset, include);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.GetRunsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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
| **include** | **string** | Include a list of related entities IDs into response. Should be separated by comma. Possible values: cases, defects  | [optional]  |

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

<a id="updaterunpublicity"></a>
# **UpdateRunPublicity**
> RunPublicResponse UpdateRunPublicity (string code, int id, RunPublic runPublic)

Update publicity of a specific run

This method allows to update a publicity of specific run. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class UpdateRunPublicityExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new RunsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var id = 56;  // int | Identifier.
            var runPublic = new RunPublic(); // RunPublic | 

            try
            {
                // Update publicity of a specific run
                RunPublicResponse result = apiInstance.UpdateRunPublicity(code, id, runPublic);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RunsApi.UpdateRunPublicity: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateRunPublicityWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update publicity of a specific run
    ApiResponse<RunPublicResponse> response = apiInstance.UpdateRunPublicityWithHttpInfo(code, id, runPublic);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RunsApi.UpdateRunPublicityWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

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

