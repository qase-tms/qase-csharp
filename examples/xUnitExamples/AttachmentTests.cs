using System.Text;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Methods;

namespace xUnitExamples;

/// <summary>
/// Demonstrates all attachment types: single file, multiple files,
/// byte array content, and step-level attachments.
/// The [Qase] attribute is required to enable attachment tracking.
/// </summary>
public class AttachmentTests
{
    private static readonly string TestDataDir =
        Path.Combine(AppContext.BaseDirectory, "testdata");

    [Fact]
    [QaseIds(401)]
    [Title("Login page captures screenshot on successful load")]
    [Qase]
    public void LoginPage_CapturesScreenshot()
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");

        // Simulate page load verification
        var pageTitle = "Login - MyApp";
        Assert.Equal("Login - MyApp", pageTitle);

        // Attach screenshot of the login page
        Metadata.Attach(screenshotPath);
    }

    [Fact]
    [QaseIds(402)]
    [Title("API test captures both request log and response screenshot")]
    [Qase]
    public void ApiTest_CapturesRequestAndResponse()
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        var logPath = Path.Combine(TestDataDir, "test-log.txt");

        // Simulate API call
        var statusCode = 200;
        Assert.Equal(200, statusCode);

        // Attach multiple files at once
        Metadata.Attach(new List<string> { screenshotPath, logPath });
    }

    [Fact]
    [QaseIds(403)]
    [Title("Generate HTML report and attach from memory")]
    [Qase]
    public void GenerateReport_AttachFromMemory()
    {
        var html = "<html><body>" +
                   "<h1>Test Summary Report</h1>" +
                   "<table>" +
                   "<tr><td>Total Tests</td><td>42</td></tr>" +
                   "<tr><td>Passed</td><td>40</td></tr>" +
                   "<tr><td>Failed</td><td>2</td></tr>" +
                   "</table>" +
                   "</body></html>";

        var reportBytes = Encoding.UTF8.GetBytes(html);
        Metadata.Attach(reportBytes, "test-summary.html");

        Assert.True(reportBytes.Length > 0, "Report should have content");
    }

    [Fact]
    [QaseIds(404)]
    [Title("Checkout step captures state screenshot at each stage")]
    [Qase]
    public void CheckoutStep_CapturesScreenshotPerStage()
    {
        FillCartAndCapture();
        EnterShippingAndCapture();
        ConfirmAndCapture();
    }

    // --- Step-level attachment demos ---

    [Step]
    [Title("Fill shopping cart")]
    private void FillCartAndCapture()
    {
        var cartTotal = 149.99m;
        Assert.True(cartTotal > 0);

        // Attach screenshot within step context — it will be linked to this step
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
    }

    [Step]
    [Title("Enter shipping details")]
    private void EnterShippingAndCapture()
    {
        var address = "123 Main St, San Francisco, CA";
        Assert.False(string.IsNullOrEmpty(address));

        // Attach log captured during this step
        var logPath = Path.Combine(TestDataDir, "test-log.txt");
        Metadata.Attach(logPath);
    }

    [Step]
    [Title("Confirm order")]
    private void ConfirmAndCapture()
    {
        var orderNumber = "ORD-2026-0042";
        Assert.False(string.IsNullOrEmpty(orderNumber));

        // Attach order confirmation as in-memory content
        var json = $"{{\"order\": \"{orderNumber}\", \"status\": \"confirmed\"}}";
        Metadata.Attach(Encoding.UTF8.GetBytes(json), "order-confirmation.json");
    }
}
