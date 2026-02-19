using Qase.Csharp.Commons.Attributes;

namespace xUnitExamples;

/// <summary>
/// Demonstrates all Qase reporter attributes in realistic test scenarios.
/// Class-level attributes apply to all tests in this class.
/// </summary>
[Fields("layer", "e2e")]
[Suites("Authentication", "Smoke Tests")]
public class AttributeTests
{
    [Fact]
    [QaseIds(101)]
    public void UserRegistration_CreatesNewAccount()
    {
        var username = "john.doe@example.com";
        var isCreated = true; // simulated account creation

        Assert.True(isCreated, $"Account for {username} should be created");
    }

    [Fact]
    [QaseIds(102, 103)]
    public void LoginPage_AcceptsBothEmailAndUsername()
    {
        var emailLogin = true;
        var usernameLogin = true;

        Assert.True(emailLogin, "Login with email should work");
        Assert.True(usernameLogin, "Login with username should work");
    }

    [Fact]
    [Title("Verify that the login page displays the correct title and branding")]
    public void LoginPage_DisplaysCorrectBranding()
    {
        var pageTitle = "Welcome to Qase";
        var hasLogo = true;

        Assert.Equal("Welcome to Qase", pageTitle);
        Assert.True(hasLogo, "Logo should be visible");
    }

    [Fact]
    [Fields("severity", "critical")]
    [Fields("priority", "high")]
    [Fields("layer", "api")]
    public void PasswordReset_SendsEmailToRegisteredUser()
    {
        var email = "john.doe@example.com";
        var isEmailSent = true; // simulated email sending

        Assert.True(isEmailSent, $"Reset email should be sent to {email}");
    }

    [Fact]
    [Suites("User Profile", "Settings")]
    public void UserProfile_UpdatesDisplayName()
    {
        var oldName = "John";
        var newName = "John Doe";
        var currentName = newName; // simulated update

        Assert.Equal(newName, currentName);
        Assert.NotEqual(oldName, currentName);
    }

    [Fact]
    [Ignore]
    public void DeprecatedFeature_LegacyPasswordPolicy()
    {
        // This test is excluded from Qase reporting.
        // The legacy password policy has been replaced by a new one.
        Assert.True(true);
    }
}
