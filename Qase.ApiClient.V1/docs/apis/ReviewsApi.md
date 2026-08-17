# Qase.ApiClient.V1.Api.ReviewsApi

All URIs are relative to *https://api.qase.io/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**BulkCreateReviews**](ReviewsApi.md#bulkcreatereviews) | **POST** /review/{code}/bulk | Create reviews in bulk |
| [**CreateReview**](ReviewsApi.md#createreview) | **POST** /review/{code} | Create a new review |
| [**DeleteReview**](ReviewsApi.md#deletereview) | **DELETE** /review/{code}/{id} | Delete review |
| [**GetReview**](ReviewsApi.md#getreview) | **GET** /review/{code}/{id} | Get a specific review |
| [**GetReviews**](ReviewsApi.md#getreviews) | **GET** /review/{code} | Get all reviews |
| [**UpdateReview**](ReviewsApi.md#updatereview) | **PATCH** /review/{code}/{id} | Update review |

<a id="bulkcreatereviews"></a>
# **BulkCreateReviews**
> ReviewBulkResponse BulkCreateReviews (string code, ReviewBulk reviewBulk)

Create reviews in bulk

This method allows to submit multiple test cases for review in one request.  Returns an error if test case review is disabled in the project settings. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **reviewBulk** | [**ReviewBulk**](ReviewBulk.md) |  |  |

### Return type

[**ReviewBulkResponse**](ReviewBulkResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Per-item outcomes for the submitted reviews. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createreview"></a>
# **CreateReview**
> IdResponse CreateReview (string code, ReviewCreate reviewCreate)

Create a new review

This method allows to submit a test case for review in selected project.  Returns an error if test case review is disabled in the project settings. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **reviewCreate** | [**ReviewCreate**](ReviewCreate.md) |  |  |

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
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deletereview"></a>
# **DeleteReview**
> IdResponse DeleteReview (string code, int id)

Delete review

This method allows to delete a review. Merged reviews cannot be deleted.  Returns an error if test case review is disabled in the project settings. 


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
| **200** | A result. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getreview"></a>
# **GetReview**
> ReviewResponse GetReview (string code, int id)

Get a specific review

This method allows to retrieve a specific review, including its current approval status per reviewer. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |

### Return type

[**ReviewResponse**](ReviewResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A Review. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getreviews"></a>
# **GetReviews**
> ReviewListResponse GetReviews (string code, string status = null, string type = null, long caseId = null, Guid authorUuid = null, Guid reviewerUuid = null, string search = null, int limit = null, int offset = null)

Get all reviews

This method allows to retrieve all test case reviews stored in selected project. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **status** | **string** |  | [optional]  |
| **type** | **string** |  | [optional]  |
| **caseId** | **long** | Filter reviews by the reviewed test case ID. | [optional]  |
| **authorUuid** | **Guid** | Filter reviews by the author who created them (author UUID). | [optional]  |
| **reviewerUuid** | **Guid** | Filter reviews by an assigned reviewer (author UUID). | [optional]  |
| **search** | **string** | Provide a string that will be used to search by review title. | [optional]  |
| **limit** | **int** | A number of entities in result set. | [optional] [default to 10] |
| **offset** | **int** | How many entities should be skipped. | [optional] [default to 0] |

### Return type

[**ReviewListResponse**](ReviewListResponse.md)

### Authorization

[TokenAuth](../README.md#TokenAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A list of all reviews. |  -  |
| **400** | Bad Request. |  -  |
| **401** | Unauthorized. |  -  |
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="updatereview"></a>
# **UpdateReview**
> IdResponse UpdateReview (string code, int id, ReviewUpdate reviewUpdate)

Update review

This method allows to update the assigned reviewers and/or the proposed test case payload of an open review. The reviewed test case cannot be changed.  Returns an error if test case review is disabled in the project settings, or if the review is not open. 


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Code of project, where to search entities. |  |
| **id** | **int** | Identifier. |  |
| **reviewUpdate** | [**ReviewUpdate**](ReviewUpdate.md) |  |  |

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
| **402** | Payment Required. |  -  |
| **403** | Forbidden. |  -  |
| **404** | Not Found. |  -  |
| **422** | Unprocessable Entity. |  -  |
| **429** | Too Many Requests. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

