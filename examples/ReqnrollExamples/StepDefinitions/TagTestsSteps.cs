using Reqnroll;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class TagTestsSteps
{
    private bool _accountCreated;
    private bool _loginSucceeded;
    private string _displayName = string.Empty;

    [Given(@"a registration form is submitted for ""(.*)""")]
    public void GivenRegistrationFormSubmitted(string email)
    {
        Assert.That(email, Is.Not.Empty);
        _accountCreated = true;
    }

    [Then(@"the account should be created successfully")]
    public void ThenAccountCreated()
    {
        Assert.That(_accountCreated, Is.True);
    }

    [Given(@"the login page is open")]
    public void GivenLoginPageIsOpen()
    {
        Assert.Pass();
    }

    [When(@"the user logs in with email ""(.*)""")]
    public void WhenLoginWithEmail(string email)
    {
        Assert.That(email, Does.Contain("@"));
        _loginSucceeded = true;
    }

    [When(@"the user logs in with username ""(.*)""")]
    public void WhenLoginWithUsername(string username)
    {
        Assert.That(username, Is.Not.Empty);
        _loginSucceeded = true;
    }

    [Then(@"login should succeed")]
    public void ThenLoginShouldSucceed()
    {
        Assert.That(_loginSucceeded, Is.True);
    }

    [Then(@"the page title should be ""(.*)""")]
    public void ThenPageTitleShouldBe(string expectedTitle)
    {
        Assert.That(expectedTitle, Is.EqualTo("Welcome to Qase"));
    }

    [Then(@"the logo should be visible")]
    public void ThenLogoShouldBeVisible()
    {
        Assert.That(true, Is.True);
    }

    [Given(@"a registered user ""(.*)""")]
    public void GivenRegisteredUser(string email)
    {
        Assert.That(email, Does.Contain("@"));
    }

    [When(@"a password reset is requested")]
    public void WhenPasswordResetRequested()
    {
        Assert.Pass();
    }

    [Then(@"a reset email should be sent")]
    public void ThenResetEmailSent()
    {
        Assert.That(true, Is.True);
    }

    [Given(@"a user with display name ""(.*)""")]
    public void GivenUserWithDisplayName(string name)
    {
        _displayName = name;
    }

    [When(@"the display name is changed to ""(.*)""")]
    public void WhenDisplayNameChanged(string newName)
    {
        _displayName = newName;
    }

    [Then(@"the display name should be ""(.*)""")]
    public void ThenDisplayNameShouldBe(string expected)
    {
        Assert.That(_displayName, Is.EqualTo(expected));
    }

    [Given(@"the legacy password policy is active")]
    public void GivenLegacyPasswordPolicy()
    {
        Assert.Pass();
    }

    [Then(@"the policy check should pass")]
    public void ThenPolicyCheckShouldPass()
    {
        Assert.That(true, Is.True);
    }
}
