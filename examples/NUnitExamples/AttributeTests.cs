using Qase.Csharp.Commons.Attributes;

namespace NUnitExamples;

/// <summary>
/// Demonstrates all Qase reporter attributes in realistic test scenarios.
/// Class-level attributes apply to all tests in this class.
/// </summary>
[TestFixture]
[Fields("layer", "UI")]
[Suites("Authentication", "Smoke Tests")]
public class AttributeTests
{
    [Test]
    [QaseIds(101)]
    public void UserRegistration_CreatesNewAccount()
    {
        var username = "john.doe@example.com";
        var isCreated = true; // simulated account creation

        Assert.That(isCreated, Is.True, $"Account for {username} should be created");
    }

    [Test]
    [QaseIds(102, 103)]
    public void LoginPage_AcceptsBothEmailAndUsername()
    {
        var emailLogin = true;
        var usernameLogin = true;

        Assert.That(emailLogin, Is.True, "Login with email should work");
        Assert.That(usernameLogin, Is.True, "Login with username should work");
    }

    [Test]
    [Title("Verify that the login page displays the correct title and branding")]
    public void LoginPage_DisplaysCorrectBranding()
    {
        var pageTitle = "Welcome to Qase";
        var hasLogo = true;

        Assert.That(pageTitle, Is.EqualTo("Welcome to Qase"));
        Assert.That(hasLogo, Is.True, "Logo should be visible");
    }

    [Test]
    [Fields("severity", "critical")]
    [Fields("priority", "high")]
    [Fields("layer", "API")]
    public void PasswordReset_SendsEmailToRegisteredUser()
    {
        var email = "john.doe@example.com";
        var isEmailSent = true; // simulated email sending

        Assert.That(isEmailSent, Is.True, $"Reset email should be sent to {email}");
    }

    [Test]
    [Suites("User Profile", "Settings")]
    public void UserProfile_UpdatesDisplayName()
    {
        var oldName = "John";
        var newName = "John Doe";
        var currentName = newName; // simulated update

        Assert.That(currentName, Is.EqualTo(newName));
        Assert.That(currentName, Is.Not.EqualTo(oldName));
    }

    [Test]
    [Qase.Csharp.Commons.Attributes.Ignore]
    public void DeprecatedFeature_LegacyPasswordPolicy()
    {
        // This test is excluded from Qase reporting.
        // The legacy password policy has been replaced by a new one.
        Assert.That(true, Is.True);
    }
}
