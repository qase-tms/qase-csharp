# Qase.ApiClient.V2.Api.ResultsApi

All URIs are relative to *https://api.qase.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateResultV2**](ResultsApi.md#createresultv2) | **POST** /{project_code}/run/{run_id}/result | Create test run result |
| [**CreateResultsV2**](ResultsApi.md#createresultsv2) | **POST** /{project_code}/run/{run_id}/results | Bulk create test run result |

<a id="createresultv2"></a>
# **CreateResultV2**
> void CreateResultV2 (string projectCode, long runId, ResultCreate resultCreate)

Create test run result

This method allows to create single test run result.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V2.Api;
using Qase.ApiClient.V2.Client;
using Qase.ApiClient.V2.Model;

namespace Example
{
    public class CreateResultV2Example
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v2";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var projectCode = "projectCode_example";  // string | 
            var runId = 789L;  // long | 
            var resultCreate = new ResultCreate(); // ResultCreate | 

            try
            {
                // Create test run result
                apiInstance.CreateResultV2(projectCode, runId, resultCreate);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.CreateResultV2: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateResultV2WithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create test run result
    apiInstance.CreateResultV2WithHttpInfo(projectCode, runId, resultCreate);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.CreateResultV2WithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **projectCode** | **string** |  |  |
| **runId** | **long** |  |  |
| **resultCreate** | [**ResultCreate**](ResultCreate.md) |  |  |

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
| **202** | OK |  -  |
| **400** | Bad Request |  -  |
| **401** | Unauthorized |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |
| **422** | Unprocessable Entity |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createresultsv2"></a>
# **CreateResultsV2**
> void CreateResultsV2 (string projectCode, long runId, CreateResultsRequestV2 createResultsRequestV2)

Bulk create test run result

This method allows to create several test run results at once.  If there is no free space left in your team account, when attempting to upload an attachment, e.g., through reporters, you will receive an error with code 507 - Insufficient Storage. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V2.Api;
using Qase.ApiClient.V2.Client;
using Qase.ApiClient.V2.Model;

namespace Example
{
    public class CreateResultsV2Example
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v2";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new ResultsApi(config);
            var projectCode = "projectCode_example";  // string | 
            var runId = 789L;  // long | 
            var createResultsRequestV2 = new CreateResultsRequestV2(); // CreateResultsRequestV2 | 

            try
            {
                // Bulk create test run result
                apiInstance.CreateResultsV2(projectCode, runId, createResultsRequestV2);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ResultsApi.CreateResultsV2: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateResultsV2WithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Bulk create test run result
    apiInstance.CreateResultsV2WithHttpInfo(projectCode, runId, createResultsRequestV2);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ResultsApi.CreateResultsV2WithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **projectCode** | **string** |  |  |
| **runId** | **long** |  |  |
| **createResultsRequestV2** | [**CreateResultsRequestV2**](CreateResultsRequestV2.md) |  |  |

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
| **202** | OK |  -  |
| **400** | Bad Request |  -  |
| **401** | Unauthorized |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |
| **422** | Unprocessable Entity |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

