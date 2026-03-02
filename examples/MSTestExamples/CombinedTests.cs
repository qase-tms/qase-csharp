using System.Text;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Methods;

namespace MSTestExamples;

/// <summary>
/// Demonstrates combining multiple reporter features in realistic end-to-end scenarios:
/// steps + attachments + comments + attributes together.
/// </summary>
[TestClass]
[Fields("layer", "e2e")]
[Suites("E2E Tests", "User Workflows")]
public class CombinedTests
{
    private static readonly string TestDataDir =
        Path.Combine(AppContext.BaseDirectory, "testdata");

    [TestMethod]
    [QaseIds(601)]
    [Title("New user onboarding: register, verify, complete profile")]
    [Fields("severity", "critical")]
    [Fields("priority", "high")]
    [Qase]
    public void UserOnboarding_EndToEndFlow()
    {
        Metadata.Comment("Starting E2E onboarding flow for new user");

        RegisterNewUser("alice@example.com", "SecurePass123!");
        VerifyEmailAddress("alice@example.com");
        CompleteUserProfile("Alice", "Johnson");
        CaptureOnboardingReport();

        Metadata.Comment("Onboarding flow completed successfully");
    }

    [TestMethod]
    [QaseIds(602)]
    [Title("Product search returns relevant results")]
    [Fields("severity", "normal")]
    [DataRow("mechanical keyboard", 15)]
    [DataRow("wireless mouse", 23)]
    [DataRow("usb hub", 8)]
    [Qase]
    public void SearchProducts_ReturnsResults(string query, int expectedMinResults)
    {
        Metadata.Comment($"Searching for: '{query}'");

        PerformSearch(query);
        VerifyResultCount(expectedMinResults);
        CaptureSearchResults(query);

        Metadata.Comment($"Search for '{query}' returned >= {expectedMinResults} results");
    }

    // --- Onboarding steps ---

    [Step]
    [Title("Register new user account")]
    private void RegisterNewUser(string email, string password)
    {
        Assert.IsFalse(string.IsNullOrEmpty(email));
        Assert.IsTrue(password.Length >= 8);

        Metadata.Comment($"Registered user: {email}");
    }

    [Step]
    [Title("Verify email address via confirmation link")]
    private void VerifyEmailAddress(string email)
    {
        var isVerified = true;
        Assert.IsTrue(isVerified, $"Email {email} should be verified");

        // Attach server log showing email delivery
        var logPath = Path.Combine(TestDataDir, "test-log.txt");
        Metadata.Attach(logPath);
    }

    [Step]
    [Title("Complete user profile with personal info")]
    private void CompleteUserProfile(string firstName, string lastName)
    {
        Assert.IsFalse(string.IsNullOrEmpty(firstName));
        Assert.IsFalse(string.IsNullOrEmpty(lastName));

        // Attach profile screenshot
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);
    }

    [Step]
    [Title("Generate and attach onboarding summary")]
    private void CaptureOnboardingReport()
    {
        var report = "{" +
                     "\"user\": \"alice@example.com\", " +
                     "\"steps_completed\": [\"registration\", \"email_verification\", \"profile\"], " +
                     "\"status\": \"onboarding_complete\", " +
                     "\"timestamp\": \"2026-02-19T10:30:00Z\"" +
                     "}";
        Metadata.Attach(Encoding.UTF8.GetBytes(report), "onboarding-report.json");
    }

    // --- Search steps ---

    [Step]
    [Title("Execute product search query")]
    private void PerformSearch(string query)
    {
        Assert.IsFalse(string.IsNullOrEmpty(query), "Search query cannot be empty");
    }

    [Step]
    [Title("Verify search returns minimum expected results")]
    private void VerifyResultCount(int expectedMin)
    {
        var actualCount = expectedMin + 5; // simulated
        Assert.IsTrue(actualCount >= expectedMin,
            $"Expected at least {expectedMin} results, got {actualCount}");
    }

    [Step]
    [Title("Capture search results page")]
    private void CaptureSearchResults(string query)
    {
        var screenshotPath = Path.Combine(TestDataDir, "screenshot.png");
        Metadata.Attach(screenshotPath);

        Metadata.Comment($"Search results page captured for query: '{query}'");
    }
}
