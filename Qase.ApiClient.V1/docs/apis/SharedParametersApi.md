# Qase.ApiClient.V1.Api.SharedParametersApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateSharedParameter**](SharedParametersApi.md#createsharedparameter) | **POST** /shared_parameter | Create a new shared parameter |
| [**DeleteSharedParameter**](SharedParametersApi.md#deletesharedparameter) | **DELETE** /shared_parameter/{id} | Delete shared parameter |
| [**GetSharedParameter**](SharedParametersApi.md#getsharedparameter) | **GET** /shared_parameter/{id} | Get a specific shared parameter |
| [**GetSharedParameters**](SharedParametersApi.md#getsharedparameters) | **GET** /shared_parameter | Get all shared parameters |
| [**UpdateSharedParameter**](SharedParametersApi.md#updatesharedparameter) | **PATCH** /shared_parameter/{id} | Update shared parameter |

<a id="createsharedparameter"></a>
# **CreateSharedParameter**
> UuidResponse CreateSharedParameter (SharedParameterCreate sharedParameterCreate)

Create a new shared parameter

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class CreateSharedParameterExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedParametersApi(config);
            var sharedParameterCreate = new SharedParameterCreate(); // SharedParameterCreate | 

            try
            {
                // Create a new shared parameter
                UuidResponse result = apiInstance.CreateSharedParameter(sharedParameterCreate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedParametersApi.CreateSharedParameter: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSharedParameterWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new shared parameter
    ApiResponse<UuidResponse> response = apiInstance.CreateSharedParameterWithHttpInfo(sharedParameterCreate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedParametersApi.CreateSharedParameterWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sharedParameterCreate** | [**SharedParameterCreate**](SharedParameterCreate.md) |  |  |

### Return type

[**UuidResponse**](UuidResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A shared parameter. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deletesharedparameter"></a>
# **DeleteSharedParameter**
> UuidResponse1 DeleteSharedParameter (Guid id)

Delete shared parameter

Delete shared parameter along with all its usages in test cases and reviews.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class DeleteSharedParameterExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedParametersApi(config);
            var id = "id_example";  // Guid | Identifier.

            try
            {
                // Delete shared parameter
                UuidResponse1 result = apiInstance.DeleteSharedParameter(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedParametersApi.DeleteSharedParameter: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteSharedParameterWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete shared parameter
    ApiResponse<UuidResponse1> response = apiInstance.DeleteSharedParameterWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedParametersApi.DeleteSharedParameterWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |

### Return type

[**UuidResponse1**](UuidResponse1.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsharedparameter"></a>
# **GetSharedParameter**
> SharedParameterResponse GetSharedParameter (Guid id)

Get a specific shared parameter

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetSharedParameterExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedParametersApi(config);
            var id = "id_example";  // Guid | Identifier.

            try
            {
                // Get a specific shared parameter
                SharedParameterResponse result = apiInstance.GetSharedParameter(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedParametersApi.GetSharedParameter: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSharedParameterWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a specific shared parameter
    ApiResponse<SharedParameterResponse> response = apiInstance.GetSharedParameterWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedParametersApi.GetSharedParameterWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |

### Return type

[**SharedParameterResponse**](SharedParameterResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A shared parameter. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsharedparameters"></a>
# **GetSharedParameters**
> SharedParameterListResponse GetSharedParameters (int limit = null, int offset = null, string filtersSearch = null, string filtersType = null, List<string> filtersProjectCodes = null)

Get all shared parameters

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class GetSharedParametersExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedParametersApi(config);
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)
            var filtersSearch = "filtersSearch_example";  // string |  (optional) 
            var filtersType = "single";  // string |  (optional) 
            var filtersProjectCodes = new List<string>(); // List<string> |  (optional) 

            try
            {
                // Get all shared parameters
                SharedParameterListResponse result = apiInstance.GetSharedParameters(limit, offset, filtersSearch, filtersType, filtersProjectCodes);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedParametersApi.GetSharedParameters: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSharedParametersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all shared parameters
    ApiResponse<SharedParameterListResponse> response = apiInstance.GetSharedParametersWithHttpInfo(limit, offset, filtersSearch, filtersType, filtersProjectCodes);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedParametersApi.GetSharedParametersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |
| **filtersSearch** | **string** |  | [optional]  |
| **filtersType** | **string** |  | [optional]  |
| **filtersProjectCodes** | [**List&lt;string&gt;**](string.md) |  | [optional]  |

### Return type

[**SharedParameterListResponse**](SharedParameterListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all shared parameters. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatesharedparameter"></a>
# **UpdateSharedParameter**
> UuidResponse1 UpdateSharedParameter (Guid id, SharedParameterUpdate sharedParameterUpdate)

Update shared parameter

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class UpdateSharedParameterExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SharedParametersApi(config);
            var id = "id_example";  // Guid | Identifier.
            var sharedParameterUpdate = new SharedParameterUpdate(); // SharedParameterUpdate | 

            try
            {
                // Update shared parameter
                UuidResponse1 result = apiInstance.UpdateSharedParameter(id, sharedParameterUpdate);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SharedParametersApi.UpdateSharedParameter: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSharedParameterWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update shared parameter
    ApiResponse<UuidResponse1> response = apiInstance.UpdateSharedParameterWithHttpInfo(id, sharedParameterUpdate);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SharedParametersApi.UpdateSharedParameterWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** | Identifier. |  |
| **sharedParameterUpdate** | [**SharedParameterUpdate**](SharedParameterUpdate.md) |  |  |

### Return type

[**UuidResponse1**](UuidResponse1.md)

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
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

