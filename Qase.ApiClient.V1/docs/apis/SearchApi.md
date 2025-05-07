# Qase.ApiClient.V1.Api.SearchApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**Search**](SearchApi.md#search) | **GET** /search | Search entities by Qase Query Language (QQL) |

<a id="search"></a>
# **Search**
> SearchResponse Search (string query, int limit = null, int offset = null)

Search entities by Qase Query Language (QQL)

This method allows to retrieve data sets for various entities using expressions with conditions. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;

namespace Example
{
    public class SearchExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.qase.io/v1";
            // Configure API key authorization: TokenAuth
            config.AddApiKey("Token", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("Token", "Bearer");

            var apiInstance = new SearchApi(config);
            var query = "query_example";  // string | Expression in Qase Query Language.
            var limit = 10;  // int | A number of entities in result set. (optional)  (default to 10)
            var offset = 0;  // int | How many entities should be skipped. (optional)  (default to 0)

            try
            {
                // Search entities by Qase Query Language (QQL)
                SearchResponse result = apiInstance.Search(query, limit, offset);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SearchApi.Search: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SearchWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Search entities by Qase Query Language (QQL)
    ApiResponse<SearchResponse> response = apiInstance.SearchWithHttpInfo(query, limit, offset);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SearchApi.SearchWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **query** | **string** | Expression in Qase Query Language. |  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**SearchResponse**](SearchResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of found entities. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **403** | Forbidden. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

