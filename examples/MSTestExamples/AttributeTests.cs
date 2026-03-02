using Qase.Csharp.Commons.Attributes;

namespace MSTestExamples;

/// <summary>
/// Demonstrates all Qase reporter attributes in realistic test scenarios.
/// Class-level attributes apply to all tests in this class.
/// </summary>
[TestClass]
[Fields("layer", "e2e")]
[Suites("Authentication", "Smoke Tests")]
public class AttributeTests
{
    [TestMethod]
    [QaseIds(101)]
    public void UserRegistration_CreatesNewAccount()
    {
        var username = "john.doe@example.com";
        var isCreated = true; // simulated account creation

        Assert.IsTrue(isCreated, $"Account for {username} should be created");
    }

    [TestMethod]
    [QaseIds(102, 103)]
    public void LoginPage_AcceptsBothEmailAndUsername()
    {
        var emailLogin = true;
        var usernameLogin = true;

        Assert.IsTrue(emailLogin, "Login with email should work");
        Assert.IsTrue(usernameLogin, "Login with username should work");
    }

    [TestMethod]
    [Title("Verify that the login page displays the correct title and branding")]
    public void LoginPage_DisplaysCorrectBranding()
    {
        var pageTitle = "Welcome to Qase";
        var hasLogo = true;

        Assert.AreEqual("Welcome to Qase", pageTitle);
        Assert.IsTrue(hasLogo, "Logo should be visible");
    }

    [TestMethod]
    [Fields("severity", "critical")]
    [Fields("priority", "high")]
    [Fields("layer", "api")]
    public void PasswordReset_SendsEmailToRegisteredUser()
    {
        var email = "john.doe@example.com";
        var isEmailSent = true; // simulated email sending

        Assert.IsTrue(isEmailSent, $"Reset email should be sent to {email}");
    }

    [TestMethod]
    [Suites("User Profile", "Settings")]
    public void UserProfile_UpdatesDisplayName()
    {
        var oldName = "John";
        var newName = "John Doe";
        var currentName = newName; // simulated update

        Assert.AreEqual(newName, currentName);
        Assert.AreNotEqual(oldName, currentName);
    }

    [TestMethod]
    [Qase.Csharp.Commons.Attributes.Ignore]
    public void DeprecatedFeature_LegacyPasswordPolicy()
    {
        // This test is excluded from Qase reporting.
        // The legacy password policy has been replaced by a new one.
        Assert.IsTrue(true);
    }
}
