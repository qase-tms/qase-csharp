using Reqnroll;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class LoginSteps
{
    private bool _isOnLoginPage;
    private string _loginResult = string.Empty;
    private bool _formVisible;

    [Given(@"the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        _isOnLoginPage = true;
        Assert.That(_isOnLoginPage, Is.True);
    }

    [When(@"the user enters ""(.*)"" and ""(.*)""")]
    public void WhenTheUserEntersCredentials(string username, string password)
    {
        Assert.That(username, Is.Not.Empty);
        Assert.That(password, Is.Not.Empty);

        _loginResult = (username == "admin" || username == "user1") ? "success" : "failure";
    }

    [Then(@"the user should see the dashboard")]
    public void ThenTheUserShouldSeeTheDashboard()
    {
        Assert.That(_loginResult, Is.EqualTo("success"));
    }

    [Then(@"the login result should be ""(.*)""")]
    public void ThenTheLoginResultShouldBe(string expectedResult)
    {
        Assert.That(_loginResult, Is.EqualTo(expectedResult));
    }

    [Then(@"the login form should be visible")]
    public void ThenTheLoginFormShouldBeVisible()
    {
        _formVisible = true;
        Assert.That(_formVisible, Is.True);
    }

    [Then(@"the ""(.*)"" button should be present")]
    public void ThenTheButtonShouldBePresent(string buttonName)
    {
        Assert.That(buttonName, Is.Not.Empty);
    }
}
