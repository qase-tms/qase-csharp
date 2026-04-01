using System.Text;
using Reqnroll;
using Qase.Csharp.Commons.Methods;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class CombinedTestsSteps
{
    private static readonly string TestDataDir =
        Path.Combine(AppContext.BaseDirectory, "testdata");

    // --- Onboarding steps ---

    [Given(@"a new user registration for ""(.*)"" with password ""(.*)""")]
    public void GivenNewUserRegistration(string email, string password)
    {
        Assert.That(email, Is.Not.Empty);
        Assert.That(password.Length, Is.GreaterThanOrEqualTo(8));
        Metadata.Comment($"Starting E2E onboarding flow for new user");
        Metadata.Comment($"Registered user: {email}");
    }

    [When(@"the email ""(.*)"" is verified")]
    public void WhenEmailVerified(string email)
    {
        Assert.That(true, Is.True, $"Email {email} should be verified");
        var logPath = Path.Combine(TestDataDir, "test-log.txt");
        Metadata.Attach(logPath);
    }

    [When(@"the profile is completed with first name ""(.*)"" and last name ""(.*)""")]
    public void WhenProfileCompleted(string firstName, string lastName)
    {
        Assert.That(firstName, Is.Not.Empty);
        Assert.That(lastName, Is.Not.Empty);
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
    }

    [Then(@"the onboarding report should be generated")]
    public void ThenOnboardingReport()
    {
        var report = "{" +
                     "\"user\": \"alice@example.com\", " +
                     "\"steps_completed\": [\"registration\", \"email_verification\", \"profile\"], " +
                     "\"status\": \"onboarding_complete\", " +
                     "\"timestamp\": \"2026-02-19T10:30:00Z\"" +
                     "}";
        Metadata.Attach(Encoding.UTF8.GetBytes(report), "onboarding-report.json");
        Metadata.Comment("Onboarding flow completed successfully");
    }

    // --- Search steps ---

    [When(@"a search is performed for ""(.*)""")]
    public void WhenSearchPerformed(string query)
    {
        Assert.That(query, Is.Not.Empty, "Search query cannot be empty");
        Metadata.Comment($"Searching for: '{query}'");
    }

    [Then(@"at least (.*) results should be returned")]
    public void ThenMinResults(int expectedMin)
    {
        var actualCount = expectedMin + 5; // simulated
        Assert.That(actualCount, Is.GreaterThanOrEqualTo(expectedMin),
            $"Expected at least {expectedMin} results, got {actualCount}");
    }

    [Then(@"the search results page should be captured")]
    public void ThenSearchResultsCaptured()
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
        Metadata.Comment("Search results page captured");
    }
}
