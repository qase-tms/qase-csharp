using System.Text;
using Reqnroll;
using Qase.Csharp.Commons.Methods;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class AttachmentTestsSteps
{
    private static readonly string TestDataDir =
        Path.Combine(AppContext.BaseDirectory, "testdata");

    [Given(@"the login page is loaded with title ""(.*)""")]
    public void GivenLoginPageLoaded(string pageTitle)
    {
        Assert.That(pageTitle, Is.EqualTo("Login - MyApp"));
    }

    [Then(@"a screenshot should be attached")]
    public void ThenScreenshotAttached()
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
    }

    [When(@"an API call returns status code (.*)")]
    public void WhenApiCallReturns(int statusCode)
    {
        Assert.That(statusCode, Is.EqualTo(200));
    }

    [Then(@"both the screenshot and log should be attached")]
    public void ThenScreenshotAndLogAttached()
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        var logPath = Path.Combine(TestDataDir, "test-log.txt");

        Metadata.Attach(new List<string> { screenshotPath, logPath });
    }

    [When(@"a test summary report is generated with (.*) total, (.*) passed and (.*) failed")]
    public void WhenReportGenerated(int total, int passed, int failed)
    {
        Assert.That(total, Is.GreaterThan(0));
    }

    [Then(@"the HTML report should be attached as byte array")]
    public void ThenHtmlReportAttached()
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
    }

    [Given(@"a shopping cart with total (.*)")]
    public void GivenShoppingCartWithTotal(decimal cartTotal)
    {
        Assert.That(cartTotal, Is.GreaterThan(0));
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
    }

    [When(@"shipping details are entered for ""(.*)""")]
    public void WhenShippingDetailsEntered(string address)
    {
        Assert.That(address, Is.Not.Empty);
        var logPath = Path.Combine(TestDataDir, "test-log.txt");
        Metadata.Attach(logPath);
    }

    [Then(@"the order ""(.*)"" should be confirmed with attachment")]
    public void ThenOrderConfirmedWithAttachment(string orderNumber)
    {
        Assert.That(orderNumber, Is.Not.Empty);
        var json = $"{{\"order\": \"{orderNumber}\", \"status\": \"confirmed\"}}";
        Metadata.Attach(Encoding.UTF8.GetBytes(json), "order-confirmation.json");
    }
}
