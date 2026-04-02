using Reqnroll;
using Qase.Csharp.Commons.Methods;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class MetadataTestsSteps
{
    [Given(@"a login attempt by ""(.*)"" from IP ""(.*)""")]
    public void GivenLoginAttempt(string username, string ipAddress)
    {
        var timestamp = DateTime.UtcNow.ToString("O");
        Metadata.Comment($"Login attempt by '{username}' from IP {ipAddress} at {timestamp}");
    }

    [When(@"the login is denied")]
    public void WhenLoginDenied()
    {
        Metadata.Comment("Expected result: login denied for invalid credentials");
    }

    [Then(@"the attempt details should be recorded")]
    public void ThenAttemptDetailsRecorded()
    {
        Assert.That(true, Is.True);
    }

    [When(@"a GET request is made to ""(.*)""")]
    public void WhenGetRequest(string endpoint)
    {
        Metadata.Comment($"GET {endpoint} returned status 200");
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenResponseStatusCode(int statusCode)
    {
        Assert.That(statusCode, Is.EqualTo(200));
    }

    [Then(@"the response body should be recorded")]
    public void ThenResponseBodyRecorded()
    {
        var responseBody = "{\"id\": 42, \"name\": \"John Doe\", \"email\": \"john@example.com\"}";
        Metadata.Comment($"Response body: {responseBody}");
    }

    [Given(@"an order ""(.*)"" with total (.*)")]
    public void GivenOrder(string orderId, decimal totalAmount)
    {
        Metadata.Comment($"Processing order {orderId}, total: ${totalAmount}");
    }

    [When(@"the order is processed")]
    public void WhenOrderProcessed()
    {
        Assert.That(true, Is.True);
    }

    [Then(@"the processing result should be recorded")]
    public void ThenProcessingResultRecorded()
    {
        Metadata.Comment("Order processed successfully");
    }
}
