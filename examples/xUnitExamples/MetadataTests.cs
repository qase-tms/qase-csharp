using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Methods;

namespace xUnitExamples;

/// <summary>
/// Demonstrates Metadata.Comment() to add contextual comments to test results.
/// </summary>
public class MetadataTests
{
    [Fact]
    [QaseIds(201)]
    [Title("Failed login records attempt details in report")]
    [Qase]
    public void FailedLogin_LogsAttemptDetails()
    {
        var username = "admin@example.com";
        var ipAddress = "192.168.1.42";
        var timestamp = DateTime.UtcNow.ToString("O");

        Metadata.Comment($"Login attempt by '{username}' from IP {ipAddress} at {timestamp}");
        Metadata.Comment("Expected result: login denied for invalid credentials");

        var loginResult = false; // simulated failed login
        Assert.False(loginResult, "Login should be denied for invalid credentials");
    }

    [Fact]
    [QaseIds(202)]
    [Title("API response details are recorded for debugging")]
    [Qase]
    public void ApiResponse_RecordsPayloadDetails()
    {
        var endpoint = "/api/v1/users/42";
        var statusCode = 200;
        var responseBody = """{"id": 42, "name": "John Doe", "email": "john@example.com"}""";

        Metadata.Comment($"GET {endpoint} returned status {statusCode}");
        Metadata.Comment($"Response body: {responseBody}");

        Assert.Equal(200, statusCode);
    }

    [Fact]
    [Title("Order processing adds tracking comment")]
    [Qase]
    public void OrderProcessing_AddsTrackingComment()
    {
        var orderId = "ORD-2026-0042";
        var totalAmount = 149.99m;

        Metadata.Comment($"Processing order {orderId}, total: ${totalAmount}");

        var isProcessed = true; // simulated
        Metadata.Comment($"Order {orderId} processed successfully");

        Assert.True(isProcessed);
    }
}
