# Qase.ApiClient.V1.Api.SharedStepsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateSharedStep**](SharedStepsApi.md#createsharedstep) | **POST** /shared_step/{code} | Create a new shared step |
| [**DeleteSharedStep**](SharedStepsApi.md#deletesharedstep) | **DELETE** /shared_step/{code}/{hash} | Delete shared step |
| [**GetSharedStep**](SharedStepsApi.md#getsharedstep) | **GET** /shared_step/{code}/{hash} | Get a specific shared step |
| [**GetSharedSteps**](SharedStepsApi.md#getsharedsteps) | **GET** /shared_step/{code} | Get all shared steps |
| [**UpdateSharedStep**](SharedStepsApi.md#updatesharedstep) | **PATCH** /shared_step/{code}/{hash} | Update shared step |

<a id="createsharedstep"></a>
# **CreateSharedStep**
> HashResponse CreateSharedStep (string code, SharedStepCreate sharedStepCreate)

Create a new shared step

This method allows to create a shared step in selected project. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateSharedStepExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedStepsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var sharedStepCreate = new SharedStepCreate(); // SharedStepCreate | 

            try
            {
                // Create a new shared step
                HashResponse result = apiInstance.CreateSharedStep(code, sharedStepCreate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedStepsApi.CreateSharedStep: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSharedStepWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new shared step
    ApiResponse<HashResponse> response = apiInstance.CreateSharedStepWithHttpInfo(code, sharedStepCreate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedStepsApi.CreateSharedStepWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **sharedStepCreate** | [**SharedStepCreate**](SharedStepCreate.md) |  |  |

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

<a id="deletesharedstep"></a>
# **DeleteSharedStep**
> HashResponse DeleteSharedStep (string code, string hash)

Delete shared step

This method completely deletes a shared step from repository. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class DeleteSharedStepExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedStepsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var hash = "hash_example";  // string | Hash.

            try
            {
                // Delete shared step
                HashResponse result = apiInstance.DeleteSharedStep(code, hash);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedStepsApi.DeleteSharedStep: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteSharedStepWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete shared step
    ApiResponse<HashResponse> response = apiInstance.DeleteSharedStepWithHttpInfo(code, hash);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedStepsApi.DeleteSharedStepWithHttpInfo: " + e.Message);
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

[**HashResponse**](HashResponse.md)

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

<a id="getsharedstep"></a>
# **GetSharedStep**
> SharedStepResponse GetSharedStep (string code, string hash)

Get a specific shared step

This method allows to retrieve a specific shared step. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetSharedStepExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedStepsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var hash = "hash_example";  // string | Hash.

            try
            {
                // Get a specific shared step
                SharedStepResponse result = apiInstance.GetSharedStep(code, hash);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedStepsApi.GetSharedStep: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSharedStepWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a specific shared step
    ApiResponse<SharedStepResponse> response = apiInstance.GetSharedStepWithHttpInfo(code, hash);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedStepsApi.GetSharedStepWithHttpInfo: " + e.Message);
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

[**SharedStepResponse**](SharedStepResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A shared step. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsharedsteps"></a>
# **GetSharedSteps**
> SharedStepListResponse GetSharedSteps (string code, string search = null, int limit = null, int offset = null)

Get all shared steps

This method allows to retrieve all shared steps stored in selected project. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetSharedStepsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedStepsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var search = "search_example";  // string | Provide a string that will be used to search by name. (optional) 
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)

            try
            {
                // Get all shared steps
                SharedStepListResponse result = apiInstance.GetSharedSteps(code, search, limit, offset);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedStepsApi.GetSharedSteps: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSharedStepsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all shared steps
    ApiResponse<SharedStepListResponse> response = apiInstance.GetSharedStepsWithHttpInfo(code, search, limit, offset);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedStepsApi.GetSharedStepsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **search** | **string** | Provide a string that will be used to search by name. | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**SharedStepListResponse**](SharedStepListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all shared steps. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatesharedstep"></a>
# **UpdateSharedStep**
> HashResponse UpdateSharedStep (string code, string hash, SharedStepUpdate sharedStepUpdate)

Update shared step

This method updates a shared step. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class UpdateSharedStepExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedStepsApi(config);
            var code = "code_example";  // string | Code of project, where to search entities.
            var hash = "hash_example";  // string | Hash.
            var sharedStepUpdate = new SharedStepUpdate(); // SharedStepUpdate | 

            try
            {
                // Update shared step
                HashResponse result = apiInstance.UpdateSharedStep(code, hash, sharedStepUpdate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedStepsApi.UpdateSharedStep: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSharedStepWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update shared step
    ApiResponse<HashResponse> response = apiInstance.UpdateSharedStepWithHttpInfo(code, hash, sharedStepUpdate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedStepsApi.UpdateSharedStepWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **hash** | **string** | Hash. |  |
| **sharedStepUpdate** | [**SharedStepUpdate**](SharedStepUpdate.md) |  |  |

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
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

